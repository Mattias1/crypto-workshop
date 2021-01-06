using CryptoWorkshop.Helpers;
using Flurl;
using Flurl.Http;
using System;
using System.Threading.Tasks;

namespace CryptoWorkshop
{
    class Program
    {
        //
        // Please have the courtesy to not look inside the "Hidden" folder, it might spoil things :)
        //
        static async Task Main(string[] args)
        {
            bool result = await Challenge1_DecryptEcb();

            Console.WriteLine(result ? "\nSUCCESS!" : "\nFAIL!");
            Console.ReadLine();
        }

        static async Task<bool> Challenge1_DecryptEcb()
        {
            string jsonHexToken = await "http://localhost:5000/token/new"
                .PostJsonAsync(new TokenEmail { Email = "test@example.com" })
                .ReceiveString();

            return false;
        }

        // TODO's
        // - Hints in HTML form
        //   - ?
        // - Api documentation (also html form?)
        // - Challenges (I probably don't want 5 though):
        //   - Modify CBC encrypted token
        //   - Sha1 length extension?
        //   - Sha1 timing leak?
        //   - Predict mersenne twister randomness?
        static bool Challenge2_ModifyEncryptedWebToken() => true;

        public class TokenEmail {
            public string? Email { get; set; }
        }
    }
}
