using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public static class EncryptionHelper
{
    private static readonly string EncryptionKey = "MAKV2SPBNI99212"; // Key must be at least 16 chars for AES
    private static readonly byte[] Salt = new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 };

    public static string Encrypt(string clearText)
    {
        if (string.IsNullOrEmpty(clearText)) return string.Empty;

        byte[] clearBytes = Encoding.UTF8.GetBytes(clearText);

        using (Aes aes = Aes.Create())
        {
            var pdb = new Rfc2898DeriveBytes(EncryptionKey, Salt);
            aes.Key = pdb.GetBytes(32);
            aes.IV = pdb.GetBytes(16);
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using (MemoryStream ms = new MemoryStream())
            using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cs.Write(clearBytes, 0, clearBytes.Length);
                cs.FlushFinalBlock();
                return Convert.ToBase64String(ms.ToArray());
            }
        }
    }

    public static string Decrypt(string cipherText)
    {
        if (string.IsNullOrEmpty(cipherText)) return string.Empty;

        try
        {
            // Step 1: Decode the URL-encoded string before converting to Base64
            cipherText = Uri.UnescapeDataString(cipherText);

            // Step 2: Convert from Base64
            byte[] cipherBytes = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                var pdb = new Rfc2898DeriveBytes(EncryptionKey, Salt);
                aes.Key = pdb.GetBytes(32);
                aes.IV = pdb.GetBytes(16);
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (MemoryStream ms = new MemoryStream())
                using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(cipherBytes, 0, cipherBytes.Length);
                    cs.FlushFinalBlock();
                    return Encoding.UTF8.GetString(ms.ToArray());
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Decryption failed. Invalid encrypted data.", ex);
        }
    }

}
