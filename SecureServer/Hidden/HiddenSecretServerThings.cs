using SecureServer.Helpers;

namespace SecureServer.Hidden {
    public static class HiddenSecretServerThings {
        // In here you find the secret keys, messages and stuff. Lots of spoilers, but xorred, to prevent accidental viewing.
        // Not secure of course, if you give the attacker (that's you in this case, hi there)
        // access to a file like this you've lost anyways. But please be sportsmanlike and don't peek, ok?
        public static byte[] SecretKeyForChallenge1 => ByteArrayHelpers.Xor(XoredChallenge1Key, AccidentalSafeGuard);
        public static string SecretMessageForChallenge1 => ConversionHelpers.ToUTF8String(
            ByteArrayHelpers.Xor(XoredChallenge1Msg, AccidentalSafeGuard));
        public static byte[] XoredChallenge1Key = ConversionHelpers.FromHexString("0x98aa4f8a9a110e7459a84db0af573b29");
        public static byte[] XoredChallenge1Msg = ConversionHelpers.FromHexString(
            "0x14b57019c1de7317e3c31901156388972eb47c19c2d23e528c8c0e0912229a827a");

        public static byte[] SecretKeyForChallenge2 => ByteArrayHelpers.Xor(XoredChallenge2Key, AccidentalSafeGuard);
        public static byte[] XoredChallenge2Key = ConversionHelpers.FromHexString("0x2bbda4f616fb16b2746ea538644578bf");

        public static byte[] SecretKeyForChallenge3 => ByteArrayHelpers.Xor(XoredChallenge3Key, AccidentalSafeGuard);
        public static byte[] XoredChallenge3Key = ConversionHelpers.FromHexString("0xb7f0044d756aa652a8ba3a4728834443");

        public static byte[] AccidentalSafeGuard = ConversionHelpers.FromHexString("0x5bda1839afb71072cfe3606e6043eef8");
    }
}