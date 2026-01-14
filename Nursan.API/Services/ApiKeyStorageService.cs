using System.IO;

namespace Nursan.API.Services
{
    /// <summary>
    /// Сервис за работа с криптирано съхранение на API Keys
    /// </summary>
    public class ApiKeyStorageService
    {
        private readonly EncryptionService _encryptionService;
        private readonly string _storagePath;
        private const string FileName = "apikeys.encrypted";

        public ApiKeyStorageService(EncryptionService encryptionService)
        {
            _encryptionService = encryptionService;
            _storagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FileName);
        }

        /// <summary>
        /// Записва API Key данни в криптиран файл
        /// </summary>
        public bool SaveApiKey(ApiKeyData data)
        {
            try
            {
                var encryptedData = _encryptionService.EncryptApiKeyData(data);
                System.IO.File.WriteAllText(_storagePath, encryptedData);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Чете API Key данни от криптиран файл
        /// </summary>
        public ApiKeyData? LoadApiKey()
        {
            try
            {
                if (!System.IO.File.Exists(_storagePath))
                    return null;

                var encryptedData = System.IO.File.ReadAllText(_storagePath);
                if (string.IsNullOrEmpty(encryptedData))
                    return null;

                return _encryptionService.DecryptApiKeyData(encryptedData);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Проверява дали съществува API Key файл
        /// </summary>
        public bool ApiKeyExists()
        {
            return System.IO.File.Exists(_storagePath);
        }

        /// <summary>
        /// Изтрива API Key файл
        /// </summary>
        public bool DeleteApiKey()
        {
            try
            {
                if (!System.IO.File.Exists(_storagePath))
                    return false;

                System.IO.File.Delete(_storagePath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Взема само API Key стойността (ако е активен)
        /// </summary>
        public string? GetApiKey()
        {
            var data = LoadApiKey();
            if (data == null || !data.IsActive)
                return null;

            return data.Value;
        }

        /// <summary>
        /// Взема статус на API Key от файл
        /// </summary>
        public ApiKeyFileStatus? GetApiKeyStatus()
        {
            var data = LoadApiKey();
            if (data == null)
                return null;

            return new ApiKeyFileStatus
            {
                Value = data.Value,
                IsActive = data.IsActive,
                CreatedDate = data.CreatedDate
            };
        }

        /// <summary>
        /// Деактивира API Key
        /// </summary>
        public bool RevokeApiKey()
        {
            var data = LoadApiKey();
            if (data == null)
                return false;

            data.IsActive = false;
            return SaveApiKey(data);
        }

        /// <summary>
        /// Активира API Key
        /// </summary>
        public bool ActivateApiKey()
        {
            var data = LoadApiKey();
            if (data == null)
                return false;

            data.IsActive = true;
            return SaveApiKey(data);
        }
    }

    /// <summary>
    /// Статус на API Key от файловата система (за съвместимост с XML версията)
    /// Забележка: Използва се ApiKeyStatus от ApiKeyDbService за база данни версията
    /// </summary>
    public class ApiKeyFileStatus
    {
        public string? Value { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}

