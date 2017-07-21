using System;

namespace CryptoWorkshop.Hidden
{
    public class Challenge3 : Challenge
    {
        private static byte[] keyStorage;
        private static byte[] key => keyStorage ?? (keyStorage = RandomHelpers.RandomByteArray(16));

        public static (string webtoken, byte[] hash) GetToken()
        {
            string webtoken = "{ user=henk, role=normaluser }";
            byte[] message = ConversionHelpers.FromUTF8String(webtoken);
            byte[] hash = Sha1.Hmac(key, message);

            return (webtoken, hash);
        }

        public static bool CheckHashWithTimingLeak(string webtoken, byte[] hash, out int time)
        {
            byte[] message = ConversionHelpers.FromUTF8String(webtoken);
            byte[] expected = Sha1.Mac(key, message);

            if (expected.Length != hash.Length)
            {
                throw new Exception($"De Hash lengte moet {expected.Length} zijn!");
            }

            for (int i = 0; i < expected.Length; i++)
            {
                if (expected[i] != hash[i])
                {
                    time = normalDeviationOnTime(i);
                    return false;
                }
            }

            time = normalDeviationOnTime(expected.Length);
            return true;
        }

        public static bool HasAdminRole(string webtoken)
        {
            return webtoken.Replace(" ", "").Replace("\n", "").Replace("\r", "").Replace("\t", "").ToLower().Contains("role=admin");
        }

        private static int normalDeviationOnTime(int index)
        {
            const int averageComparisonLength = 8;
            int tijd = index * averageComparisonLength + 40;

            const int maxDeviation = 10;
            int d = RandomHelpers.Random.Next(0, maxDeviation) + RandomHelpers.Random.Next(0, maxDeviation) - maxDeviation; // Fake normal distribution

            return tijd + d;
        }

        public static bool CheckForgedAdminHash(string webtoken, byte[] hash)
        {
            AssertCheckOnce();

            if (!HasAdminRole(webtoken))
            {
                return false;
            }

            byte[] message = ConversionHelpers.FromUTF8String(webtoken);
            byte[] expectedHash = Sha1.Mac(key, message);
            return MiscHelpers.Equals(expectedHash, hash);
        }
    }
}
