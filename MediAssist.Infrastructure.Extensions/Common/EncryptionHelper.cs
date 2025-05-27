using System.Security.Cryptography;
using System.Text;

namespace MediAssist.Infrastructure.Extensions.Common
{
    public static class EncryptionHelper
    {
        public static string Encrypt(string plainText, string key, string iv)
        {
            if (string.IsNullOrEmpty(plainText)) return plainText;
            using Aes aes = Aes.Create();

            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = Encoding.UTF8.GetBytes(iv);

            using MemoryStream memoryStream = new();
            using CryptoStream cryptoStream = new(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write);
            using StreamWriter streamWriter = new(cryptoStream);

            streamWriter.Write(plainText);
            streamWriter.Flush();
            cryptoStream.FlushFinalBlock();

            return Convert.ToBase64String(memoryStream.ToArray());
        }

        public static string Decrypt(string base64CipherText, string key, string iv)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(base64CipherText)) return base64CipherText;

                byte[] cipherBytes = Convert.FromBase64String(base64CipherText);
                using Aes aes = Aes.Create();

                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = Encoding.UTF8.GetBytes(iv);

                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using MemoryStream memoryStream = new(cipherBytes);
                using CryptoStream cryptoStream = new(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Read);
                using StreamReader streamReader = new(cryptoStream);

                return streamReader.ReadToEnd();
            }
            catch
            {
                throw; 
            }
        }

        public static string CleanHex(string hex)
        {
            return new string(hex.Where(c => Uri.IsHexDigit(c)).ToArray());
        }

    }
}
