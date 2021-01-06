using Newtonsoft.Json;
using SecureServer.Helpers;
using System;

namespace SecureServer.Token {
    public class JsonInternetToken {
        public enum TokenSecurityMechanism { AES_ECB, AES_CBC, SHA1 };

        public string Email { get; }
        public string Role { get; }
        public string SecretMessage { get; }
        public TokenSecurityMechanism SecurityMechanism { get; }

        private JsonInternetToken(string email, string role, string secretMessage, TokenSecurityMechanism securityMechanism) {
            Email = email;
            Role = role;
            SecretMessage = secretMessage;
            SecurityMechanism = securityMechanism;
        }

        public string ToJsonHexString() {
            byte[] bytes = ToJsonBytes();
            return ConversionHelpers.ToHexString(bytes, true);
        }
        public byte[] ToJsonBytes() {
            string plainJsonString = ToPlainJsonString();
            byte[] plainJsonBytes = ConversionHelpers.FromUTF8String(plainJsonString);
            byte[] securedJsonBytes = SecureToken(plainJsonBytes);
            return securedJsonBytes;
        }
        private string ToPlainJsonString() {
            return $"{{ \"email\": \"{Email}\", \"role\": \"{Role}\", \"secretMessage\": \"{SecretMessage}\" }}";
        }

        public static JsonInternetToken CreateAesEcb(string email, string role, string secretMessage = "") {
            return new JsonInternetToken(email, role, secretMessage, TokenSecurityMechanism.AES_ECB);
        }
        public static JsonInternetToken CreateAesCbc(string email, string role, string secretMessage = "") {
            return new JsonInternetToken(email, role, secretMessage, TokenSecurityMechanism.AES_CBC);
        }
        public static JsonInternetToken CreateSha1(string email, string role, string secretMessage = "") {
            return new JsonInternetToken(email, role, secretMessage, TokenSecurityMechanism.SHA1);
        }

        public static JsonInternetToken FromJsonHexString(string jsonHexString) {
            byte[] jsonBytes = ConversionHelpers.FromHexString(jsonHexString);
            return FromJsonBytes(jsonBytes);
        }
        public static JsonInternetToken FromJsonBytes(byte[] securedJsonBytes) {
            byte[] plainJsonBytes = DeSecureToken(securedJsonBytes);
            string plainJsonString = ConversionHelpers.ToUTF8String(plainJsonBytes);
            return FromPlainJsonString(plainJsonString);
        }
        private static JsonInternetToken FromPlainJsonString(string jsonString) {
            return JsonConvert.DeserializeObject<JsonInternetToken>(jsonString);
        }

        private byte[] SecureToken(byte[] unsecureJsonBytes) {
            if (SecurityMechanism == TokenSecurityMechanism.AES_ECB) {
                throw new NotImplementedException("TODO, implement this token security mechanism.");

                // TODO: Make sure the byte array is a multiple of 16 - use PKCS#7 for that.
                byte[] unsecuredBytes = plainJsonBytes;

                byte[][] blocks = ByteArrayHelpers.SplitUp(unsecuredBytes, 16);

                // TODO: Add encryption here
                encryptedJsonBytes = blocks;

                byte[] securedBytes = ByteArrayHelpers.Concatenate(encryptedJsonBytes);
                return securedBytes;
            }
            else {
                throw new NotImplementedException("TODO, implement this token security mechanism.");
            }
        }

        private byte[] DeSecureToken(byte[] secureJsonBytes) {
            if (SecurityMechanism == TokenSecurityMechanism.AES_ECB) {
                throw new NotImplementedException("TODO, implement this token security mechanism.");
            }
            else {
                throw new NotImplementedException("TODO, implement this token security mechanism.");
            }
        }
    }
}