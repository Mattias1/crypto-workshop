using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace CryptoWorkshop.Hidden
{
    public class Challenge1 : Challenge
    {
        private static byte[] keyStorage;
        private static byte[] key => keyStorage ?? (keyStorage = RandomHelpers.RandomByteArray(16));
        private static string secretMessage
        {
            get => ConversionHelpers.ToUTF8String(Convert.FromBase64String("Q29uZ3JhdHVsYXRpb25zLCB5b3UgZm91bmQgdGhlIHNlY3JldCBtZXNzYWdlIQ=="));
        }

        public static byte[] SendSecretMessage(string userInput)
        {
            byte[] plain = ConversionHelpers.FromUTF8String(userInput + secretMessage);
            return BlockCipher.EncryptAES(plain, key, null, CipherMode.ECB, PaddingMode.Zeros);
        }

        public static bool CheckIfSecretMessageIsCorrect(string answer)
        {
            AssertCheckOnce();
            return answer.Contains(secretMessage);
        }
    }
}
