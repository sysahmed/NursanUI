using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nursan.Domain.Entity;

namespace Nursan.API.Services
{
    /// <summary>
    /// Сервис за работа с API Keys в базата данни
    /// </summary>
    public class ApiKeyDbService
    {
        private readonly UretimOtomasyonContext _context;
        private readonly EncryptionService _encryptionService;
        private readonly ILogger<ApiKeyDbService>? _logger;

        public ApiKeyDbService(UretimOtomasyonContext context, EncryptionService encryptionService, ILogger<ApiKeyDbService>? logger = null)
        {
            _context = context;
            _encryptionService = encryptionService;
            _logger = logger;
        }

        /// <summary>
        /// Записва API Key в базата данни (криптиран) за конкретно устройство
        /// </summary>
        public async Task<bool> SaveApiKeyAsync(string deviceId, string apiKey, string? deviceName = null, string? createdBy = null, string? description = null, bool isActive = true)
        {
            try
            {
                // Проверяваме дали вече има ключ за това устройство
                var existing = await _context.ApiKeys
                    .FirstOrDefaultAsync(k => k.DeviceId == deviceId);

                if (existing != null)
                {
                    // Актуализираме съществуващия ключ
                    var encryptedKey = _encryptionService.Encrypt(apiKey);
                    existing.KeyValue = encryptedKey;
                    existing.IsActive = isActive;
                    existing.DeviceName = deviceName ?? existing.DeviceName;
                    existing.Description = description ?? existing.Description;
                    existing.CreatedBy = createdBy ?? existing.CreatedBy;
                }
                else
                {
                    // Криптираме API Key преди запис
                    var encryptedKey = _encryptionService.Encrypt(apiKey);

                    var apiKeyEntity = new ApiKey
                    {
                        DeviceId = deviceId,
                        DeviceName = deviceName,
                        KeyValue = encryptedKey,
                        IsActive = isActive,
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = createdBy,
                        Description = description,
                        RequestCount = 0
                    };

                    _context.ApiKeys.Add(apiKeyEntity);
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Взема активен API Key от базата данни
        /// </summary>
        public async Task<string?> GetActiveApiKeyAsync()
        {
            try
            {
                var apiKeyEntity = await _context.ApiKeys
                    .Where(k => k.IsActive)
                    .OrderByDescending(k => k.CreatedDate)
                    .FirstOrDefaultAsync();

                if (apiKeyEntity == null)
                    return null;

                // Декриптираме API Key
                var decryptedKey = _encryptionService.Decrypt(apiKeyEntity.KeyValue);
                
                // Актуализираме last used date и request count
                apiKeyEntity.LastUsedDate = DateTime.UtcNow;
                apiKeyEntity.RequestCount++;
                
                // Не правим SaveChanges тук за да не бавим всяка заявка
                // Може да се направи асинхронно или на batch

                return decryptedKey;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Взема API Key по ключова стойност
        /// </summary>
        public async Task<ApiKey?> GetApiKeyByValueAsync(string plainApiKey)
        {
            try
            {
                var encryptedKey = _encryptionService.Encrypt(plainApiKey);
                return await _context.ApiKeys
                    .FirstOrDefaultAsync(k => k.KeyValue == encryptedKey);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Взема API Key по DeviceId
        /// </summary>
        public async Task<ApiKey?> GetApiKeyByDeviceIdAsync(string deviceId)
        {
            try
            {
                return await _context.ApiKeys
                    .FirstOrDefaultAsync(k => k.DeviceId == deviceId);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Взема всички API Keys
        /// </summary>
        public async Task<List<ApiKey>> GetAllApiKeysAsync()
        {
            try
            {
                return await _context.ApiKeys
                    .OrderByDescending(k => k.CreatedDate)
                    .ToListAsync();
            }
            catch
            {
                return new List<ApiKey>();
            }
        }

        /// <summary>
        /// Взема статус на API Key (без криптираната стойност) по DeviceId
        /// </summary>
        public async Task<ApiKeyStatus?> GetApiKeyStatusAsync(string? deviceId = null)
        {
            try
            {
                ApiKey? apiKeyEntity;
                
                if (!string.IsNullOrEmpty(deviceId))
                {
                    apiKeyEntity = await _context.ApiKeys
                        .FirstOrDefaultAsync(k => k.DeviceId == deviceId);
                }
                else
                {
                    apiKeyEntity = await _context.ApiKeys
                        .OrderByDescending(k => k.CreatedDate)
                        .FirstOrDefaultAsync();
                }

                if (apiKeyEntity == null)
                    return null;

                return new ApiKeyStatus
                {
                    DeviceId = apiKeyEntity.DeviceId,
                    DeviceName = apiKeyEntity.DeviceName,
                    Value = null, // Не връщаме стойността за сигурност
                    IsActive = apiKeyEntity.IsActive,
                    CreatedDate = apiKeyEntity.CreatedDate,
                    LastUsedDate = apiKeyEntity.LastUsedDate,
                    RequestCount = apiKeyEntity.RequestCount,
                    CreatedBy = apiKeyEntity.CreatedBy,
                    Description = apiKeyEntity.Description
                };
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Деактивира API Key по DeviceId или Id
        /// </summary>
        public async Task<bool> RevokeApiKeyAsync(string? deviceId = null, int? apiKeyId = null)
        {
            try
            {
                ApiKey? apiKeyEntity;
                
                if (!string.IsNullOrEmpty(deviceId))
                {
                    apiKeyEntity = await _context.ApiKeys
                        .FirstOrDefaultAsync(k => k.DeviceId == deviceId);
                }
                else if (apiKeyId.HasValue)
                {
                    apiKeyEntity = await _context.ApiKeys.FindAsync(apiKeyId.Value);
                }
                else
                {
                    // Деактивираме последния активен
                    apiKeyEntity = await _context.ApiKeys
                        .Where(k => k.IsActive)
                        .OrderByDescending(k => k.CreatedDate)
                        .FirstOrDefaultAsync();
                }

                if (apiKeyEntity == null)
                    return false;

                apiKeyEntity.IsActive = false;
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Активира API Key по DeviceId или Id
        /// </summary>
        public async Task<bool> ActivateApiKeyAsync(string? deviceId = null, int? apiKeyId = null)
        {
            try
            {
                ApiKey? apiKeyEntity;
                
                if (!string.IsNullOrEmpty(deviceId))
                {
                    apiKeyEntity = await _context.ApiKeys
                        .FirstOrDefaultAsync(k => k.DeviceId == deviceId);
                }
                else if (apiKeyId.HasValue)
                {
                    apiKeyEntity = await _context.ApiKeys.FindAsync(apiKeyId.Value);
                }
                else
                {
                    // Активираме последния
                    apiKeyEntity = await _context.ApiKeys
                        .OrderByDescending(k => k.CreatedDate)
                        .FirstOrDefaultAsync();
                }

                if (apiKeyEntity == null)
                    return false;

                apiKeyEntity.IsActive = true;
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Изтрива API Key по DeviceId или Id
        /// </summary>
        public async Task<bool> DeleteApiKeyAsync(string? deviceId = null, int? apiKeyId = null)
        {
            try
            {
                ApiKey? apiKeyEntity;
                
                if (!string.IsNullOrEmpty(deviceId))
                {
                    apiKeyEntity = await _context.ApiKeys
                        .FirstOrDefaultAsync(k => k.DeviceId == deviceId);
                }
                else if (apiKeyId.HasValue)
                {
                    apiKeyEntity = await _context.ApiKeys.FindAsync(apiKeyId.Value);
                }
                else
                {
                    // Изтриваме последния
                    apiKeyEntity = await _context.ApiKeys
                        .OrderByDescending(k => k.CreatedDate)
                        .FirstOrDefaultAsync();
                }

                if (apiKeyEntity == null)
                    return false;

                _context.ApiKeys.Remove(apiKeyEntity);
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Валидира API Key (проверява дали съществува и е активен)
        /// </summary>
        public async Task<bool> ValidateApiKeyAsync(string plainApiKey)
        {
            try
            {
                var apiKeyEntity = await GetApiKeyByValueAsync(plainApiKey);
                
                if (apiKeyEntity == null)
                    return false;

                if (!apiKeyEntity.IsActive)
                    return false;

                // Проверяваме дали е изтекъл
                if (apiKeyEntity.ExpiryDate.HasValue && apiKeyEntity.ExpiryDate.Value < DateTime.UtcNow)
                    return false;

                // Актуализираме статистиката
                apiKeyEntity.LastUsedDate = DateTime.UtcNow;
                apiKeyEntity.RequestCount++;
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    /// <summary>
    /// Статус на API Key
    /// </summary>
    public class ApiKeyStatus
    {
        public string? DeviceId { get; set; }
        public string? DeviceName { get; set; }
        public string? Value { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastUsedDate { get; set; }
        public long RequestCount { get; set; }
        public string? CreatedBy { get; set; }
        public string? Description { get; set; }
    }
}

