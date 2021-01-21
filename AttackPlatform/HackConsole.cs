using AttackPlatform.Helpers;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace AttackPlatform
{
    public class HackConsole
    {
        private readonly ApiTester _api;
        private readonly ITestOutputHelper _output;

        public HackConsole(ITestOutputHelper outputHelper) {
            _api = new ApiTester();
            _output = outputHelper;
        }

        [Fact]
        public void Challenge1_ImplementEcbEncryption() {
            // This should really not be in the attack platform, as it's a test to test server side things, but oh well.
            var token = SecureServer.Token.InternetToken.CreateAesEcb("some-email@example.com", "user");
            token.Email.Should().Be("some-email@example.com");
            token.Role.Should().Be("user");

            string hexString = token.ToTokenHexString();
            var plainToken = SecureServer.Token.InternetToken.FromAesEcbTokenHexString(hexString);
            plainToken.Email.Should().Be("some-email@example.com");
            plainToken.Role.Should().Be("user");
        }

        [Fact]
        public async Task Challenge1_DecryptEcb()
        {
            _output.WriteLine("");
            var characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ=; -_,.?\"'{}[]|<>/\\".ToCharArray();

            string prefix = new string('B', 10);

            List<char> result = new List<char>();
            for (int hackPosition = 0; hackPosition < 66; hackPosition++) {
                int blockPosition = hackPosition / 16;

                // Build input for attempt
                char[] input = (prefix + new string('A', 16 * blockPosition + 15)).ToCharArray();
                for (int i=0; i < result.Count; i++) {
                    input[input.Length - result.Count + i] = result[i];
                }

                // Build the block with the character from the original token, that we try to find
                var blocksToAttack = (await GetBlocks(prefix + new string('A', 16 * blockPosition + 15 - result.Count)));
                var blockToAttack = blocksToAttack[blockPosition + 1];

                _output.WriteLine($"Attempt char {hackPosition}; input: {new string(input)} ({result.Count})");

                // PrintBlocks("-- ", blocksToAttack, 1);
                foreach (char c in characters) {
                    var attempt = (await GetBlocks(input, c));
                    var attemptBlock = attempt[blockPosition + 1];

                    // PrintBlocks($"{c}: ", attempt, 1);

                    if (ByteArrayHelpers.IsEqual(attemptBlock, blockToAttack)) {
                        result.Add(c);
                        _output.WriteLine($"The next byte is: {c}. Result so far: {new string(result.ToArray())}");
                        break;
                    }
                }
            }
        }

        private async Task<byte[][]> GetBlocks(char[] input, char c) => await GetBlocks(new string(input) + c);
        private async Task<byte[][]> GetBlocks(string email) {
            string hex = await _api.PostJsonAsString("/token/new", new TokenEmail {
                Email = email
            });
            return ByteArrayHelpers.SplitUp(ConversionHelpers.FromHexString(hex), 16);
        }

        private void PrintBlocks(string prefix, byte[][] blocks, params int[] indices) {
            string output = "";
            for (int i=0; i < blocks.Length; i++) {
                if (indices.Contains(i)) {
                    output += ConversionHelpers.ToHexString(blocks[i]) + ", ";
                }
            }
            _output.WriteLine(prefix + output);
        }

        [Fact]
        public async Task JustATestForTheHomePage() {
            string msg = SecureServer.Hidden.HiddenSecretServerThings.SecretMessageForChallenge1;

            string result = await _api.GetAsString("/");
            result.Should().Contain("<h1>Crypto workshops</h1>");
        }

        public class TokenEmail {
            public string? Email { get; set; }
        }
    }
}
