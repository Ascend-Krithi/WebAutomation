using System.Security.Cryptography;
using System.Text;

namespace WebAutomation.Core.Security;

public static class EncryptionManager
{
    private const int KeySize = 256;
    private const int BlockSize = 128;

    // IMPORTANT: Store these securely (e.g., Azure Key Vault)
    private static readonly byte[] Key = Encoding.UTF8.GetBytes("your-32-byte-long-encryption-key");
    private static readonly byte[] IV = Encoding.UTF8.GetBytes("your-16-byte-iv");

    public static string Encrypt(string plainText)
    {
        using var aes = Aes.Create();
        aes.KeySize = KeySize;
        aes.BlockSize = BlockSize;
        aes.Key = Key;
        aes.IV = IV;

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
        var buffer = Convert.FromBase64String(cipherText);

        using var aes = Aes.Create();
        aes.KeySize = KeySize;
        aes.BlockSize = BlockSize;
        aes.Key = Key;
        aes.IV = IV;

        var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

        using var ms = new MemoryStream(buffer);
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var sr = new StreamReader(cs);

        return sr.ReadToEnd();
    }
}