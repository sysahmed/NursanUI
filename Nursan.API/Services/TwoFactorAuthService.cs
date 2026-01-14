using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Caching.Memory;

namespace Nursan.API.Services
{
    /// <summary>
    /// Сервис за 2-факторна аутентикация (2FA)
    /// Използва TOTP (Time-based One-Time Password) алгоритъм за генериране на 6-цифрени кодове
    /// </summary>
    public class TwoFactorAuthService
    {
        private readonly IMemoryCache _cache;
        private readonly ILogger<TwoFactorAuthService> _logger;
        private const int CODE_EXPIRY_MINUTES = 5; // Кодът е валиден 5 минути
        private const int CODE_LENGTH = 6; // 6-цифрен код

        public TwoFactorAuthService(IMemoryCache cache, ILogger<TwoFactorAuthService> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        /// <summary>
        /// Генерира 6-цифрен TOTP код за потребител
        /// </summary>
        /// <param name="userId">ID на потребителя</param>
        /// <param name="userSecret">Тайния ключ на потребителя (от базата или генериран)</param>
        /// <returns>6-цифрен код</returns>
        public string GenerateCode(string userId, string? userSecret = null)
        {
            try
            {
                // Ако няма secret, генерираме временен secret за сесията
                if (string.IsNullOrEmpty(userSecret))
                {
                    userSecret = GenerateSecretKey(userId);
                }

                // Генерираме TOTP код
                var code = GenerateTotpCode(userSecret);

                // Запазваме кода в cache с expiry (ключ: userId, стойност: code)
                var cacheKey = $"2FA_{userId}_{code}";
                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(CODE_EXPIRY_MINUTES),
                    SlidingExpiration = TimeSpan.FromMinutes(2) // Слайдваме expiry при използване
                };

                // Запазваме информация за кода: код, timestamp, secret
                var codeInfo = new TwoFactorCodeInfo
                {
                    Code = code,
                    UserId = userId,
                    Secret = userSecret,
                    GeneratedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(CODE_EXPIRY_MINUTES)
                };

                _cache.Set(cacheKey, codeInfo, cacheOptions);

                // Също запазваме по userId за по-лесно търсене
                _cache.Set($"2FA_USER_{userId}", codeInfo, cacheOptions);

                _logger.LogInformation($"2FA код генериран за потребител {userId}: {code.Substring(0, 2)}**** (първите 2 цифри)");

                return code;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Грешка при генериране на 2FA код за потребител {userId}");
                throw;
            }
        }

        /// <summary>
        /// Валидира 2FA код за потребител
        /// </summary>
        /// <param name="userId">ID на потребителя</param>
        /// <param name="code">Кодът за валидация</param>
        /// <returns>true ако кодът е валиден</returns>
        public bool ValidateCode(string userId, string code)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(code) || code.Length != CODE_LENGTH)
                {
                    _logger.LogWarning($"Невалиден формат на 2FA код за потребител {userId}: кодът трябва да е {CODE_LENGTH} цифри");
                    return false;
                }

                // Опитваме се да вземем от cache
                var cacheKey = $"2FA_{userId}_{code}";
                if (_cache.TryGetValue(cacheKey, out TwoFactorCodeInfo? codeInfo))
                {
                    if (codeInfo != null && codeInfo.Code == code && codeInfo.UserId == userId)
                    {
                        // Кодът е валиден - изтриваме го от cache (едноразово използване)
                        _cache.Remove(cacheKey);
                        _cache.Remove($"2FA_USER_{userId}");

                        _logger.LogInformation($"2FA код валидиран успешно за потребител {userId}");
                        return true;
                    }
                }

                // Опитваме се да валидираме срещу текущия TOTP код (за по-гъвкавост)
                // Това позволява кодът да работи в рамките на 30 секунди window
                var userCodeInfo = _cache.Get<TwoFactorCodeInfo>($"2FA_USER_{userId}");
                if (userCodeInfo != null && !string.IsNullOrEmpty(userCodeInfo.Secret))
                {
                    // Генерираме текущия TOTP код и проверяваме
                    var currentCode = GenerateTotpCode(userCodeInfo.Secret);
                    var previousCode = GenerateTotpCode(userCodeInfo.Secret, -1); // Предишния период
                    var nextCode = GenerateTotpCode(userCodeInfo.Secret, 1); // Следващия период

                    if (code == currentCode || code == previousCode || code == nextCode)
                    {
                        // Кодът е валиден - изтриваме го
                        _cache.Remove($"2FA_{userId}_{code}");
                        _cache.Remove($"2FA_USER_{userId}");

                        _logger.LogInformation($"2FA код валидиран успешно за потребител {userId} (TOTP validation)");
                        return true;
                    }
                }

                _logger.LogWarning($"Невалиден 2FA код за потребител {userId}");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Грешка при валидация на 2FA код за потребител {userId}");
                return false;
            }
        }

        /// <summary>
        /// Генерира TOTP код използвайки HMAC-SHA1 алгоритъм
        /// </summary>
        private string GenerateTotpCode(string secret, int timeStepOffset = 0)
        {
            // TOTP използва 30 секунди time step
            var timeStep = (long)(DateTime.UtcNow.AddSeconds(timeStepOffset * 30) - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds / 30;

            // Конвертираме secret в bytes
            var secretBytes = Encoding.UTF8.GetBytes(secret);
            if (secretBytes.Length < 16)
            {
                // Padding ако secret е твърде къс
                var padded = new byte[16];
                Array.Copy(secretBytes, padded, secretBytes.Length);
                secretBytes = padded;
            }

            // Конвертираме timeStep в 8 bytes (big-endian)
            var timeStepBytes = BitConverter.GetBytes(timeStep);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(timeStepBytes);
            }

            // Изчисляваме HMAC-SHA1
            using (var hmac = new HMACSHA1(secretBytes))
            {
                var hash = hmac.ComputeHash(timeStepBytes);

                // Динамично truncation (RFC 4226)
                var offset = hash[hash.Length - 1] & 0x0F;
                var binaryCode = ((hash[offset] & 0x7F) << 24) |
                                 ((hash[offset + 1] & 0xFF) << 16) |
                                 ((hash[offset + 2] & 0xFF) << 8) |
                                 (hash[offset + 3] & 0xFF);

                var code = binaryCode % (int)Math.Pow(10, CODE_LENGTH);
                return code.ToString().PadLeft(CODE_LENGTH, '0');
            }
        }

        /// <summary>
        /// Генерира уникален secret key за потребител
        /// </summary>
        private string GenerateSecretKey(string userId)
        {
            // Генерираме базиран на userId + timestamp за уникалност
            var input = $"{userId}_{DateTime.UtcNow:yyyyMMddHHmmss}_{Guid.NewGuid()}";
            using (var sha256 = SHA256.Create())
            {
                var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                return Convert.ToBase64String(hash);
            }
        }

        /// <summary>
        /// Проверява дали потребителят има активен 2FA код в процес
        /// </summary>
        public bool HasPendingCode(string userId)
        {
            return _cache.TryGetValue($"2FA_USER_{userId}", out _);
        }

        /// <summary>
        /// Изтрива pending 2FA код (за logout или cancel)
        /// </summary>
        public void ClearPendingCode(string userId)
        {
            var userCodeInfo = _cache.Get<TwoFactorCodeInfo>($"2FA_USER_{userId}");
            if (userCodeInfo != null)
            {
                _cache.Remove($"2FA_{userId}_{userCodeInfo.Code}");
                _cache.Remove($"2FA_USER_{userId}");
                _logger.LogInformation($"Pending 2FA код изтрит за потребител {userId}");
            }
        }

        /// <summary>
        /// Информация за генериран 2FA код
        /// </summary>
        private class TwoFactorCodeInfo
        {
            public string Code { get; set; } = string.Empty;
            public string UserId { get; set; } = string.Empty;
            public string Secret { get; set; } = string.Empty;
            public DateTime GeneratedAt { get; set; }
            public DateTime ExpiresAt { get; set; }
        }
    }
}

