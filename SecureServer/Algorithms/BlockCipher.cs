using SecureServer.Helpers;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace SecureServer.Algorithms {
    public static class BlockCipher {
        /// <summary>
        /// Encrypt one block (16 bytes) using AES.
        /// </summary>
        public static byte[] EncryptAesBlock(byte[] input, byte[] key) {
            if (input.Length != 16 || key.Length != 16) {
                throw new InvalidOperationException("The input and key should both be exactly 16 bytes.");
            }
            return EncryptAes(input, key, new byte[16], CipherMode.ECB, PaddingMode.None);
        }

        /// <summary>
        /// Decrypt one block (16 bytes) using AES.
        /// </summary>
        public static byte[] DecryptAesBlock(byte[] input, byte[] key) {
            if (input.Length != 16 || key.Length != 16) {
                throw new InvalidOperationException("The input and key should both be exactly 16 bytes.");
            }
            return DecryptAes(input, key, new byte[16], CipherMode.ECB, PaddingMode.None);
        }

        /// <summary>
        /// This is a proper function to encrypt using AES. Private, because you have to do this yourself :)
        /// </summary>
        private static byte[] EncryptAes(byte[] input, byte[] key, byte[] iv, CipherMode mode, PaddingMode paddingMode) {
            byte[] result;
            using (var cipher = Aes.Create()) {
                cipher.Mode = mode;
                cipher.Padding = paddingMode;

                using (ICryptoTransform encryptor = cipher.CreateEncryptor(key, iv)) {
                    using (MemoryStream to = new MemoryStream()) {
                        using (CryptoStream writer = new CryptoStream(to, encryptor, CryptoStreamMode.Write)) {
                            writer.Write(input, 0, input.Length);
                            writer.FlushFinalBlock();
                            result = to.ToArray();
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// This is a proper function to decrypt using AES. Private, because you have to do this yourself :)
        /// </summary>
        private static byte[] DecryptAes(byte[] input, byte[] key, byte[] iv, CipherMode mode, PaddingMode paddingMode) {
            byte[] result;

            using (var cipher = Aes.Create()) {
                cipher.Mode = mode;
                cipher.Padding = paddingMode == PaddingMode.PKCS7 ? PaddingMode.None : paddingMode;

                try {
                    using (ICryptoTransform decryptor = cipher.CreateDecryptor(key, iv)) {
                        using (MemoryStream from = new MemoryStream(input)) {
                            using (CryptoStream reader = new CryptoStream(from, decryptor, CryptoStreamMode.Read)) {
                                result = new byte[input.Length];
                                reader.Read(result, 0, result.Length);
                            }
                        }
                    }
                }
                catch (Exception ex) {
                    throw ex;
                }
            }
            if (paddingMode == PaddingMode.PKCS7) {
                return UnPkcs7(result);
            }
            return result;
        }

        /// <summary>
        /// Returns an object that contains both the cipher text and the IV. Used for CBC mode.
        /// </summary>
        public static BlockCipherResult Result(byte[] cipher, byte[] iv) {
            return new BlockCipherResult(cipher, iv);
        }

        /// <summary>
        /// Add PKCS#7 padding. This changes the array's length.
        /// </summary>
        public static byte[] Pkcs7(byte[] raw, int blocksize = 16) {
            return ByteArrayHelpers.ForcePadWith(raw, blocksize, (byte)(blocksize - raw.Length % blocksize));
        }

        /// <summary>
        /// Remove PKCS#7 padding. This changes the array's length.
        /// </summary>
        public static byte[] UnPkcs7(byte[] raw) {
            int paddingLength = GetPkcs7Length(raw);
            return ByteArrayHelpers.CopyPartOf(raw, 0, raw.Length - paddingLength);
        }

        /// <summary>
        /// Remove PKCS#7 padding. This time, overwrite the padding with zeroes.
        /// </summary>
        public static byte[] ZeroPkcs7(byte[] raw) {
            int paddingLength = GetPkcs7Length(raw);
            byte[] result = new byte[raw.Length];
            Array.Copy(raw, 0, result, 0, raw.Length - paddingLength);
            return result;
        }

        /// <summary>
        /// Check whether or not the raw array is a properly PKCS7-padded and return the padding length.
        /// Return -1 when not valid.
        /// </summary>
        public static int GetPkcs7Length(byte[] raw) {
            int paddingLength = raw.Last();
            for (int i = 0; i < paddingLength; i++) {
                if (raw[raw.Length - i - 1] != paddingLength) {
                    return -1;
                }
            }
            return paddingLength;
        }

        public static bool CheckPkcs7(byte[] raw) {
            return GetPkcs7Length(raw) > 0;
        }
    }

    public class BlockCipherResult
    {
        public byte[] Cipher;
        public byte[] Iv;

        public int Length => this.Cipher.Length;

        public BlockCipherResult(byte[] cipher, byte[] iv) {
            this.Cipher = cipher;
            this.Iv = iv;
        }
    }
}
