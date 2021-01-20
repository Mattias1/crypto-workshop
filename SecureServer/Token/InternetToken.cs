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
                throw new NotImplementedException("TODO, implement this token security mechanism.");

                // TODO: Make sure the byte array is a multiple of 16 - use PKCS#7 for that.
                // You could use the method in BlockCipher.cs, or make it yourself ;)
                byte[] unsecuredBytes = unsecuredTokenBytes;

                byte[][] blocks = ByteArrayHelpers.SplitUp(unsecuredBytes, 16);

                // TODO: Add encryption here - see BlockCipher.cs on how to encrypt a single block
                // (if you modify it you can also encrypt multiple blocks, but that'd be cheating, so don't).
                // You can set the message by commenting out the following line:
                // SecretMessage = SecureServer.Hidden.HiddenSecretServerThings.SecretMessageForChallenge1;
                // Please encrypt using this key (and don't peek!):
                // byte[] challenge1Key = SecureServer.Hidden.HiddenSecretServerThings.SecretKeyForChallenge1;
                // And yeah, this is a little bit ugly, but it might stop you from accidentally read the message or the
                // key before you crack it. It's supposed to stay a secret from you, until you finish the cracking part
                // of Challenge 1 that is of course ;)
                byte[][] encryptedTokenBytes = blocks;

                byte[] securedBytes = ByteArrayHelpers.Concatenate(encryptedTokenBytes);
                return securedBytes;
            }
            else {
                throw new NotImplementedException("TODO, implement the other token security mechanisms.");
            }
        }

        private static byte[] DeSecureToken(byte[] secureTokenBytes, TokenSecurityMechanism securityMechanism) {
            if (securityMechanism == TokenSecurityMechanism.AES_ECB) {
                throw new NotImplementedException("TODO, implement this token security mechanism.");
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