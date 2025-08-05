using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Northwind.Utilities.Helper
{
    public static class AesEncryptionHelper
    {
        public static string EncryptObject<T>(T data, string keyBase64, string ivBase64)
        {
            var json = JsonSerializer.Serialize(data);
            return Encrypt(json, keyBase64, ivBase64);
        }

        public static T DecryptToObject<T>(string encryptedBase64, string keyBase64, string ivBase64)
        {
            var json = Decrypt(encryptedBase64, keyBase64, ivBase64);
            return JsonSerializer.Deserialize<T>(json);
        }

        public static string Encrypt(string plainText, string keyBase64, string ivBase64)
        {
            var key = Convert.FromBase64String(keyBase64);
            var iv = Convert.FromBase64String(ivBase64);

            using var aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;
            aes.Padding = PaddingMode.PKCS7;

            using var encryptor = aes.CreateEncryptor();
            var plainBytes = Encoding.UTF8.GetBytes(plainText);
            var cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

            return Convert.ToBase64String(cipherBytes);
        }

        public static string Decrypt(string encryptedBase64, string keyBase64, string ivBase64)
        {
            var key = Convert.FromBase64String(keyBase64);
            var iv = Convert.FromBase64String(ivBase64);

            using var aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;
            aes.Padding = PaddingMode.PKCS7;

            using var decryptor = aes.CreateDecryptor();
            var cipherBytes = Convert.FromBase64String(encryptedBase64);
            var plainBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);

            return Encoding.UTF8.GetString(plainBytes);
        }
    }
}

