using System.Security.Cryptography;
using System.Text;

namespace WebAutomation.Core.Security
{
    public static class EncryptionManager
    {
        private const int KeySize = 256;
        private const int BlockSize = 128;
        private const int IvSize = BlockSize / 8; // 16 bytes

        // IMPORTANT: Replace with a secure key from environment variables or a secret manager.
        // This key is for demonstration purposes only.
        private static readonly byte[] Key = Encoding.UTF8.GetBytes("0123456789abcdef0123456789abcdef");

        public static string Encrypt(string plainText)
        {
            using var aes = Aes.Create();
            aes.KeySize = KeySize;
            aes.BlockSize = BlockSize;
            aes.Key = Key;
            aes.GenerateIV(); // Generate a new IV for each encryption

            var iv = aes.IV;

            using var encryptor = aes.CreateEncryptor(aes.Key, iv);
            using var ms = new MemoryStream();

            // Prepend IV to the ciphertext
            ms.Write(iv, 0, iv.Length);

            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            {
                using var sw = new StreamWriter(cs);
                sw.Write(plainText);
            }

            return Convert.ToBase64String(ms.ToArray());
        }

        public static string Decrypt(string cipherText)
        {
            var fullCipher = Convert.FromBase64String(cipherText);

            using var aes = Aes.Create();
            aes.KeySize = KeySize;
            aes.BlockSize = BlockSize;
            aes.Key = Key;

            // Extract IV from the beginning of the ciphertext
            var iv = new byte[IvSize];
            Array.Copy(fullCipher, 0, iv, 0, iv.Length);
            aes.IV = iv;

            using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream();

            // Write the actual ciphertext (after the IV) to the memory stream
            ms.Write(fullCipher, IvSize, fullCipher.Length - IvSize);
            ms.Position = 0; // Reset stream position to the beginning

            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);

            return sr.ReadToEnd();
        }
    }
}