using System;
using System.Linq;
using CryptoWorkshop.Hidden;

namespace CryptoWorkshop
{
    class Program
    {
        //
        // Please have the courtesy to not look inside the "Hidden" folder :)
        //
        static void Main(string[] args)
        {
            bool result = Challenge1_DecryptEcb();

            Console.WriteLine(result ? "\nSUCCESS!" : "\nFAIL!");
            Console.ReadLine();
        }

        static bool Challenge1_DecryptEcb()
        {
            for (int dummyByte = 0; dummyByte < 256; dummyByte++)
            {
                byte[] userInput = new byte[] { (byte)dummyByte };
                byte[] cipher = Challenge1.SendSecretMessage(ConversionHelpers.ToUTF8String(userInput));

                byte[][] blocks = ByteArrayHelpers.SplitUp(cipher, 16);

                string probablyNotTheAnswer = ConversionHelpers.ToUTF8String(cipher);
            }

            return Challenge1.CheckIfSecretMessageIsCorrect("<insert the decrypted secret message here>");
        }

        // Todo :)
        static bool Challenge2_ModifyEncryptedWebToken() => true;

        static bool Challenge3_ForgeWebTokenHash()
        {
            (string token, byte[] hash) = Challenge3.GetToken();

            bool hashIsCorrect = Challenge3.CheckHashWithTimingLeak(token, hash, out int time);
            bool hasAdminRole = Challenge3.HasAdminRole(token);

            Console.WriteLine($"Token: {token}, hash: {hash}, hash has admin role: {hasAdminRole}, hash is correct: {hashIsCorrect}");

            return Challenge3.CheckForgedAdminHash(token, hash);
        }

        // Todo :)
        static bool Challenge4_PredictRandomToken() => true;
    }
}
