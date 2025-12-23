using System.Security.Cryptography;
using System.Text;

namespace WebAutomation.Core.Security
{
    public static class EncryptionManager
    {
        private const int KeySize = 256;
        private const int BlockSize = 128;

        public static string Encrypt(string plainText, string keyBase64, string ivBase64)
        {
            var key = Convert.FromBase64String(keyBase64);
            var iv = Convert.FromBase64String(ivBase64);

            using var aes = Aes.Create();
            aes.KeySize = KeySize;
            aes.BlockSize = BlockSize;
            aes.Key = key;
            aes.IV = iv;

            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using var ms = new MemoryStream();
            using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
            using (var sw = new StreamWriter(cs))
            {
                sw.Write(plainText);
            }

            return Convert.ToBase64String(ms.ToArray());
        }

        public static string Decrypt(string cipherText)
        {
            // This is a placeholder. In a real scenario, the key and IV
            // would be retrieved from a secure location (e.g., Azure Key Vault).
            // For this example, we'll use hardcoded values.
            // IMPORTANT: DO NOT use hardcoded keys in production.
            const string keyBase64 = "your-secure-base64-encoded-256-bit-key-here"; // Replace with a real key
            const string ivBase64 = "your-secure-base64-encoded-128-bit-iv-here";   // Replace with a real IV

            var key = Convert.FromBase64String(keyBase64);
            var iv = Convert.FromBase64String(ivBase64);
            var buffer = Convert.FromBase64String(cipherText);

            using var aes = Aes.Create();
            aes.KeySize = KeySize;
            aes.BlockSize = BlockSize;
            aes.Key = key;
            aes.IV = iv;

            var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using var ms = new MemoryStream(buffer);
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);

            return sr.ReadToEnd();
        }
    }
}