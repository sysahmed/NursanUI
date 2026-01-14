using System.Security.Cryptography;
using System.Text;

namespace Nursan.API.Services
{
    /// <summary>
    /// Сервис за криптиране и декриптиране на данни
    /// </summary>
    public class EncryptionService
    {
        private readonly byte[] _key;
        private readonly byte[] _iv;
        private const int KeySize = 256; // AES-256
        private const int BlockSize = 128;

        public EncryptionService(IConfiguration configuration)
        {
            // Генерираме или взимаме ключ от конфигурацията
            var encryptionKey = configuration["Encryption:Key"] 
                ?? "NursanSuperSecretEncryptionKeyForApiKeys2024!ChangeInProduction!"; // 64 символа за 256-битов ключ

            // Използваме SHA256 за да създадем фиксиран ключ от стринга
            using (var sha256 = SHA256.Create())
            {
                _key = sha256.ComputeHash(Encoding.UTF8.GetBytes(encryptionKey));
            }

            // Използваме първите 16 байта от ключа като IV (в production трябва да е различен)
            // За по-сигурно, може да се използва PBKDF2 или да се запазва IV отделно
            _iv = _key.Take(16).ToArray();
        }

        /// <summary>
        /// Криптира стринг данни
        /// </summary>
        public string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                return string.Empty;

            using (var aes = Aes.Create())
            {
                aes.KeySize = KeySize;
                aes.BlockSize = BlockSize;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = _key;
                aes.IV = _iv;

                using (var encryptor = aes.CreateEncryptor())
                {
                    byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                    byte[] encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
                    return Convert.ToBase64String(encryptedBytes);
                }
            }
        }

        /// <summary>
        /// Декриптира стринг данни
        /// </summary>
        public string Decrypt(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText))
                return string.Empty;

            try
            {
                using (var aes = Aes.Create())
                {
                    aes.KeySize = KeySize;
                    aes.BlockSize = BlockSize;
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;
                    aes.Key = _key;
                    aes.IV = _iv;

                    using (var decryptor = aes.CreateDecryptor())
                    {
                        byte[] cipherBytes = Convert.FromBase64String(cipherText);
                        byte[] decryptedBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
                        return Encoding.UTF8.GetString(decryptedBytes);
                    }
                }
            }
            catch
            {
                // Ако декриптирането не успее, връщаме празен стринг
                return string.Empty;
            }
        }

        /// <summary>
        /// Криптира API Key данни (обект)
        /// </summary>
        public string EncryptApiKeyData(ApiKeyData data)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(data);
            return Encrypt(json);
        }

        /// <summary>
        /// Декриптира API Key данни (обект)
        /// </summary>
        public ApiKeyData? DecryptApiKeyData(string encryptedData)
        {
            try
            {
                var json = Decrypt(encryptedData);
                if (string.IsNullOrEmpty(json))
                    return null;

                return System.Text.Json.JsonSerializer.Deserialize<ApiKeyData>(json);
            }
            catch
            {
                return null;
            }
        }
    }

    /// <summary>
    /// Модел за API Key данни
    /// </summary>
    public class ApiKeyData
    {
        public string Value { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
    }
}

