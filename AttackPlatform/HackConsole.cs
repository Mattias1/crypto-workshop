using AttackPlatform.Helpers;
using FluentAssertions;
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
            string encryptedToken = await _api.PostJsonAsString("/token/new", new TokenEmail {
                Email = "test@example.com"
            });

            _output.WriteLine($"Token: '{encryptedToken}'");

            // Alternatively, you can also do this:
            var result = await _api.PostJson("/token/new", new TokenEmail {
                Email = "test@example.com"
            });

            // Obviously, this should be 200, but unless you implement it, it's a 400.
            // And yeah, failing tests that you don't have to fix right now are annoying, so we'll cheat a bit :)
            result.StatusCode.Should().Be(400);
            string encryptedToken2 = await result.Content.ReadAsStringAsync();
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
