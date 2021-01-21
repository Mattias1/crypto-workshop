using SecureServer.Algorithms;
using SecureServer.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SecureServer.Token {
    public class InternetToken {
        public enum TokenSecurityMechanism { AES_ECB, AES_CBC, SHA1_MAC, SHA1_HMAC };

        private Dictionary<string, string> _values;

        public string Email => _values.TryGetValue("email", out string? value) ? value : "";
        public string Role => _values.TryGetValue("role", out string? value) ? value : "";
        public string SecretMessage => _values.TryGetValue("secretMessage", out string? value) ? value : "";
        public TokenSecurityMechanism SecurityMechanism => ParseEnum(_values.TryGetValue("securityMechanism", out string? value) ? value : "");

        private InternetToken(string email, string role, string secretMessage, TokenSecurityMechanism securityMechanism) {
            _values = new Dictionary<string, string>{
                { "email", email },
                { "role", role },
                { "secretMessage", secretMessage },
                { "securityMechanism", securityMechanism.ToString() }
            };
        }
        private InternetToken(Dictionary<string, string> values) {
            _values = values;
        }

        public string ToTokenHexString() {
            byte[] bytes = ToTokenBytes();
            return ConversionHelpers.ToHexString(bytes, true);
        }
        public byte[] ToTokenBytes() {
            _values["secretMessage"] = Hidden.HiddenSecretServerThings.SecretMessageForChallenge1;
            string plainTokenString = ToPlainTokenString();
            byte[] plainTokenBytes = ConversionHelpers.FromUTF8String(plainTokenString);
            byte[] securedTokenBytes = SecureToken(plainTokenBytes);
            return securedTokenBytes;
        }
        private string ToPlainTokenString() {
            return $"email={Email};role={Role};secretMessage={SecretMessage}";
        }

        public static InternetToken CreateAesEcb(string email, string role, string secretMessage = "") {
            return new InternetToken(email, role, secretMessage, TokenSecurityMechanism.AES_ECB);
        }
        public static InternetToken CreateSha1(string email, string role, string secretMessage = "") {
            return new InternetToken(email, role, secretMessage, TokenSecurityMechanism.SHA1_MAC);
        }

        public static InternetToken FromAesEcbTokenHexString(string tokenHexString) {
            byte[] tokenBytes = ConversionHelpers.FromHexString(tokenHexString);
            return FromAesEcbTokenBytes(tokenBytes);
        }
        public static InternetToken FromAesEcbTokenBytes(byte[] securedTokenBytes) {
            byte[] plainTokenBytes = DeSecureToken(securedTokenBytes, TokenSecurityMechanism.AES_ECB);
            string plainTokenString = ConversionHelpers.ToUTF8String(plainTokenBytes);
            return FromPlainTokenString(plainTokenString);
        }
        private static InternetToken FromPlainTokenString(string tokenString) {
            var values = new Dictionary<string, string>();
            foreach (var keyvaluePair in tokenString.Split(';')) {
                var keyvalueArray = keyvaluePair.Split('=');
                if (keyvalueArray.Length == 2) {
                    values[keyvalueArray[0]] = keyvalueArray[1];
                    // values.Add(keyvalueArray[0], keyvalueArray[1]);
                }
            }
            return new InternetToken(values);
        }

        private byte[] SecureToken(byte[] unsecuredTokenBytes) {
            if (SecurityMechanism == TokenSecurityMechanism.AES_ECB) {
                byte[] unsecuredBytes = BlockCipher.Pkcs7(unsecuredTokenBytes);
                byte[][] blocks = ByteArrayHelpers.SplitUp(unsecuredBytes, 16);
                byte[][] encryptedTokenBytes = blocks
                    .Select(block => BlockCipher.EncryptAesBlock(block, SecureServer.Hidden.HiddenSecretServerThings.SecretKeyForChallenge1))
                    .ToArray();
                return ByteArrayHelpers.Concatenate(encryptedTokenBytes);
            }
            else {
                throw new NotImplementedException("TODO, implement the other token security mechanisms.");
            }
        }

        private static byte[] DeSecureToken(byte[] secureTokenBytes, TokenSecurityMechanism securityMechanism) {
            if (securityMechanism == TokenSecurityMechanism.AES_ECB) {
                byte[][] blocks = ByteArrayHelpers.SplitUp(secureTokenBytes, 16);
                byte[][] decryptedTokenBytes = blocks
                    .Select(block => BlockCipher.DecryptAesBlock(block, SecureServer.Hidden.HiddenSecretServerThings.SecretKeyForChallenge1))
                    .ToArray();
                return BlockCipher.UnPkcs7(ByteArrayHelpers.Concatenate(decryptedTokenBytes));
            }
            else {
                throw new NotImplementedException("TODO, implement the other token security mechanisms.");
            }
        }

        private static TokenSecurityMechanism ParseEnum(string securityMechanism) => securityMechanism switch {
            "AES_ECB" => TokenSecurityMechanism.AES_ECB,
            "AES_CBC" => TokenSecurityMechanism.AES_CBC,
            "SHA1_MAC" => TokenSecurityMechanism.SHA1_MAC,
            "SHA1_HMAC" => TokenSecurityMechanism.SHA1_HMAC,
            _ => throw new InvalidOperationException("Unknown TokenSecurityMechanism value")
        };
    }
}