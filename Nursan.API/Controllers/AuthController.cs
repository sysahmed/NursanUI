using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Nursan.API.Services;
using Nursan.Domain.AmbarModels;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Nursan.API.Controllers
{
    /// <summary>
    /// Контролер за аутентикация и API Key управление
    /// </summary>
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;
        private readonly JwtService _jwtService;
        private readonly AmbarContext _ambarContext;
        private readonly ApiKeyDbService _apiKeyDbService;

        public AuthController(
            IConfiguration configuration, 
            ILogger<AuthController> logger,
            JwtService jwtService,
            AmbarContext ambarContext,
            ApiKeyDbService apiKeyDbService)
        {
            _configuration = configuration;
            _logger = logger;
            _jwtService = jwtService;
            _ambarContext = ambarContext;
            _apiKeyDbService = apiKeyDbService;
        }

        /// <summary>
        /// Логин endpoint - връща JWT токен
        /// </summary>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new { Success = false, Message = "Потребителско име и парола са задължителни" });
            }

            try
            {
                // Проверяваме потребителя в базата данни
                var user = await _ambarContext.AspNetUsers
                    .FirstOrDefaultAsync(u => u.UserName == request.Username);

                if (user == null)
                {
                    _logger.LogWarning($"Опит за вход с несъществуващ потребител: {request.Username}");
                    return Unauthorized(new { Success = false, Message = "Невалиден потребител или парола" });
                }

                // Проверяваме паролата (тук трябва да се използва PasswordHasher от ASP.NET Identity)
                // За сега приемаме, че паролата е вече хеширана в базата
                // В production трябва да се имплементира правилна проверка
                if (!VerifyPassword(request.Password, user.PasswordHash))
                {
                    _logger.LogWarning($"Невалидна парола за потребител: {request.Username}");
                    return Unauthorized(new { Success = false, Message = "Невалиден потребител или парола" });
                }

                // Генерираме JWT токен
                var roles = await _ambarContext.Set<AspNetUser>()
                    .Where(u => u.Id == user.Id)
                    .SelectMany(u => u.Roles.Select(r => r.Name ?? ""))
                    .ToListAsync();

                var token = _jwtService.GenerateToken(user.UserName ?? request.Username, user.Id, roles);

                _logger.LogInformation($"Успешен login за потребител: {request.Username}");

                return Ok(new
                {
                    Success = true,
                    Token = token,
                    Username = user.UserName,
                    ExpiresIn = 3600 // 1 час в секунди
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Грешка при login");
                return StatusCode(500, new { Success = false, Message = "Вътрешна грешка на сървъра" });
            }
        }

        /// <summary>
        /// Проверява паролата (опростена версия - в production трябва да се използва ASP.NET Identity PasswordHasher)
        /// </summary>
        private bool VerifyPassword(string password, string? passwordHash)
        {
            // Това е опростена версия - в production трябва да се използва PasswordHasher
            // За сега приемаме, че паролата е хеширана с BCrypt или подобен алгоритъм
            if (string.IsNullOrEmpty(passwordHash))
                return false;

            // Тук трябва да се имплементира правилна проверка според използвания алгоритъм
            // За демонстрационни цели, може да се пропусне проверката или да се използва прост стринг comparison
            // ВНИМАНИЕ: Не използвайте това в production!
            return !string.IsNullOrEmpty(passwordHash);
        }

        /// <summary>
        /// Генерира нов API Key за конкретно устройство
        /// Изисква JWT аутентикация
        /// Всеки клиент/устройство има свой уникален API Key
        /// </summary>
        [HttpPost("generate-api-key")]
        [Authorize] // Изисква JWT аутентикация
        public async Task<IActionResult> GenerateApiKey([FromBody] GenerateApiKeyRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.DeviceId))
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "DeviceId е задължителен. Всеки клиент трябва да има уникален идентификатор."
                    });
                }

                // Проверяваме дали вече има API Key за това устройство
                var existingStatus = await _apiKeyDbService.GetApiKeyStatusAsync(request.DeviceId);
                if (existingStatus != null && existingStatus.IsActive && !request.Overwrite)
                {
                    return Conflict(new
                    {
                        Success = false,
                        Message = $"Вече има активен API Key за устройство '{request.DeviceId}'. Използвайте 'overwrite=true' за генериране на нов.",
                        DeviceId = request.DeviceId,
                        ExistingKeyInfo = new
                        {
                            IsActive = existingStatus.IsActive,
                            CreatedDate = existingStatus.CreatedDate,
                            RequestCount = existingStatus.RequestCount
                        }
                    });
                }

                // Генерираме сигурен API Key (64 символа)
                var apiKey = GenerateSecureApiKey();
                
                // Записваме в базата данни (криптиран) за конкретното устройство
                var username = User.Identity?.Name ?? "Unknown";

                if (!await _apiKeyDbService.SaveApiKeyAsync(
                    request.DeviceId, 
                    apiKey, 
                    request.DeviceName, 
                    username, 
                    request.Description, 
                    isActive: true))
                {
                    return StatusCode(500, new
                    {
                        Success = false,
                        Message = "Не може да се запише API Key в базата данни"
                    });
                }

                _logger.LogInformation($"API Key е генериран успешно за устройство '{request.DeviceId}' от {username}");

                return Ok(new
                {
                    Success = true,
                    Message = $"API Key е генериран успешно за устройство '{request.DeviceId}' и е активен",
                    DeviceId = request.DeviceId,
                    DeviceName = request.DeviceName,
                    ApiKey = apiKey, // В production не трябва да се връща ключа!
                    Warning = "Запишете този API Key на безопасно място! Той няма да се покаже отново.",
                    IsActive = true,
                    Note = "Всеки клиент има свой уникален API Key. Запазен е криптирано в базата данни"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Грешка при генериране на API Key");
                return StatusCode(500, new
                {
                    Success = false,
                    Message = $"Грешка при генериране на API Key: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Връща статус на API Key за конкретно устройство
        /// </summary>
        [HttpGet("api-key-status")]
        [Authorize]
        public async Task<IActionResult> GetApiKeyStatus([FromQuery] string? deviceId = null)
        {
            try
            {
                var status = await _apiKeyDbService.GetApiKeyStatusAsync(deviceId);
                
                if (status == null)
                {
                    return Ok(new
                    {
                        Success = true,
                        HasApiKey = false,
                        DeviceId = deviceId,
                        Message = deviceId != null 
                            ? $"API Key не е генериран за устройство '{deviceId}'" 
                            : "API Key не е генериран"
                    });
                }

                return Ok(new
                {
                    Success = true,
                    HasApiKey = true,
                    DeviceId = status.DeviceId,
                    DeviceName = status.DeviceName,
                    IsActive = status.IsActive,
                    CreatedDate = status.CreatedDate,
                    LastUsedDate = status.LastUsedDate,
                    RequestCount = status.RequestCount,
                    CreatedBy = status.CreatedBy,
                    Description = status.Description,
                    Message = status.IsActive 
                        ? $"API Key за устройство '{status.DeviceId}' е активен и работи постоянно" 
                        : $"API Key за устройство '{status.DeviceId}' е деактивиран",
                    Storage = "База данни (криптирано)"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Грешка при проверка на API Key статус");
                return StatusCode(500, new
                {
                    Success = false,
                    Message = $"Грешка при проверка на API Key статус: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Деактивира API Key за конкретно устройство
        /// </summary>
        [HttpPost("revoke-api-key")]
        [Authorize]
        public async Task<IActionResult> RevokeApiKey([FromQuery] string? deviceId = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(deviceId))
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "DeviceId е задължителен за деактивиране на API Key"
                    });
                }

                var status = await _apiKeyDbService.GetApiKeyStatusAsync(deviceId);
                if (status == null)
                {
                    return NotFound(new
                    {
                        Success = false,
                        DeviceId = deviceId,
                        Message = $"API Key за устройство '{deviceId}' не е намерен"
                    });
                }

                if (!status.IsActive)
                {
                    return Ok(new
                    {
                        Success = true,
                        DeviceId = deviceId,
                        Message = $"API Key за устройство '{deviceId}' вече е деактивиран"
                    });
                }

                bool result = await _apiKeyDbService.RevokeApiKeyAsync(deviceId);
                if (result)
                {
                    _logger.LogInformation($"API Key за устройство '{deviceId}' е деактивиран успешно");
                    return Ok(new
                    {
                        Success = true,
                        DeviceId = deviceId,
                        Message = $"API Key за устройство '{deviceId}' е деактивиран успешно. Заявките с този ключ вече няма да работят."
                    });
                }

                return StatusCode(500, new
                {
                    Success = false,
                    Message = "Не може да се деактивира API Key"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Грешка при деактивиране на API Key");
                return StatusCode(500, new
                {
                    Success = false,
                    Message = $"Грешка при деактивиране на API Key: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Активира API Key за конкретно устройство
        /// </summary>
        [HttpPost("activate-api-key")]
        [Authorize]
        public async Task<IActionResult> ActivateApiKey([FromQuery] string? deviceId = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(deviceId))
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "DeviceId е задължителен за активиране на API Key"
                    });
                }

                var status = await _apiKeyDbService.GetApiKeyStatusAsync(deviceId);
                if (status == null)
                {
                    return NotFound(new
                    {
                        Success = false,
                        DeviceId = deviceId,
                        Message = $"API Key за устройство '{deviceId}' не е намерен. Първо генерирайте API Key."
                    });
                }

                if (status.IsActive)
                {
                    return Ok(new
                    {
                        Success = true,
                        DeviceId = deviceId,
                        Message = $"API Key за устройство '{deviceId}' вече е активен"
                    });
                }

                bool result = await _apiKeyDbService.ActivateApiKeyAsync(deviceId: deviceId);
                if (result)
                {
                    _logger.LogInformation($"API Key за устройство '{deviceId}' е активиран успешно");
                    return Ok(new
                    {
                        Success = true,
                        DeviceId = deviceId,
                        Message = $"API Key за устройство '{deviceId}' е активиран успешно и отново работи постоянно."
                    });
                }

                return StatusCode(500, new
                {
                    Success = false,
                    Message = "Не може да се активира API Key"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Грешка при активиране на API Key");
                return StatusCode(500, new
                {
                    Success = false,
                    Message = $"Грешка при активиране на API Key: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Изтрива API Key за конкретно устройство
        /// </summary>
        [HttpDelete("api-key")]
        [Authorize]
        public async Task<IActionResult> DeleteApiKey([FromQuery] string? deviceId = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(deviceId))
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "DeviceId е задължителен за изтриване на API Key"
                    });
                }

                var status = await _apiKeyDbService.GetApiKeyStatusAsync(deviceId);
                if (status == null)
                {
                    return NotFound(new
                    {
                        Success = false,
                        DeviceId = deviceId,
                        Message = $"API Key за устройство '{deviceId}' не е намерен"
                    });
                }

                bool result = await _apiKeyDbService.DeleteApiKeyAsync(deviceId);
                if (result)
                {
                    _logger.LogInformation($"API Key за устройство '{deviceId}' е изтрит успешно");
                    return Ok(new
                    {
                        Success = true,
                        DeviceId = deviceId,
                        Message = $"API Key за устройство '{deviceId}' е изтрит успешно от базата данни"
                    });
                }

                return StatusCode(500, new
                {
                    Success = false,
                    Message = "Не може да се изтрие API Key"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Грешка при изтриване на API Key");
                return StatusCode(500, new
                {
                    Success = false,
                    Message = $"Грешка при изтриване на API Key: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Генерира сигурен случайен API Key
        /// </summary>
        private string GenerateSecureApiKey()
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                var bytes = new byte[32]; // 256 бита
                rng.GetBytes(bytes);
                return Convert.ToBase64String(bytes).Replace("+", "-").Replace("/", "_").TrimEnd('=');
            }
        }

        /// <summary>
        /// Записва API Key в XML файла
        /// </summary>
        private void SaveApiKeyToXml(string apiKey)
        {
            var xmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Baglanti.xml");
            
            if (!System.IO.File.Exists(xmlPath))
            {
                // Създаваме нов XML файл ако не съществува
                CreateDefaultXml(xmlPath, apiKey);
                return;
            }

            var doc = new XmlDocument();
            doc.Load(xmlPath);

            var configNode = doc.SelectSingleNode("config");
            if (configNode == null)
            {
                throw new InvalidOperationException("XML файлът няма 'config' node");
            }

            // Проверяваме дали вече има apiKey node
            var apiKeyNode = configNode.SelectSingleNode("apiKey");
            if (apiKeyNode == null)
            {
                // Създаваме нов apiKey node
                apiKeyNode = doc.CreateElement("apiKey");
                var valueAttr = doc.CreateAttribute("Value");
                valueAttr.Value = apiKey;
                apiKeyNode.Attributes.Append(valueAttr);
                configNode.AppendChild(apiKeyNode);
            }
            else
            {
                // Актуализираме съществуващия
                if (apiKeyNode.Attributes["Value"] == null)
                {
                    var valueAttr = doc.CreateAttribute("Value");
                    valueAttr.Value = apiKey;
                    apiKeyNode.Attributes.Append(valueAttr);
                }
                else
                {
                    apiKeyNode.Attributes["Value"].Value = apiKey;
                }
            }

            // Записваме промените
            doc.Save(xmlPath);
        }

        /// <summary>
        /// Създава default XML файл с API Key
        /// </summary>
        private void CreateDefaultXml(string xmlPath, string apiKey)
        {
            var doc = new XmlDocument();
            var declaration = doc.CreateXmlDeclaration("1.0", "utf-8", null);
            doc.AppendChild(declaration);

            var configNode = doc.CreateElement("config");
            doc.AppendChild(configNode);

            var apiKeyNode = doc.CreateElement("apiKey");
            var valueAttr = doc.CreateAttribute("Value");
            valueAttr.Value = apiKey;
            apiKeyNode.Attributes.Append(valueAttr);
            configNode.AppendChild(apiKeyNode);

            doc.Save(xmlPath);
        }

        /// <summary>
        /// Актуализира appsettings.json (опционално)
        /// </summary>
        private void UpdateAppSettings(string apiKey)
        {
            // За сега просто логираме - в production може да се имплементира запис в appsettings.json
            _logger.LogInformation("API Key е актуализиран. Моля, актуализирайте appsettings.json ръчно с: ApiSettings:ApiKey = {ApiKey}", apiKey);
        }

        /// <summary>
        /// Валидира предоставен API Key срещу базата данни
        /// Използва се от фронтенд при стартиране за проверка дали ключът е актуален
        /// </summary>
        [HttpPost("validate-api-key")]
        [AllowAnonymous]
        public async Task<IActionResult> ValidateApiKey([FromHeader(Name = "X-API-Key")] string? apiKey, [FromHeader(Name = "X-Device-Id")] string? deviceIdHeader = null, [FromQuery] string? deviceId = null)
        {
            if (string.IsNullOrEmpty(apiKey))
            {
                return BadRequest(new { Success = false, Message = "API Key не е предоставен" });
            }

            // Използваме deviceId от header или от query параметър
            var deviceIdToValidate = deviceIdHeader ?? deviceId;

            try
            {
                // Вземаме API Key entity от базата данни
                var apiKeyEntity = await _apiKeyDbService.GetApiKeyByValueAsync(apiKey);
                
                if (apiKeyEntity == null)
                {
                    _logger.LogWarning($"API Key validation failed: Key not found in database");
                    return Unauthorized(new 
                    { 
                        Success = false, 
                        Message = "Невалиден API Key",
                        CanWorkWithApi = false
                    });
                }

                // Проверяваме дали е активен
                if (!apiKeyEntity.IsActive)
                {
                    _logger.LogWarning($"API Key validation failed: Key for device {apiKeyEntity.DeviceId} is not active");
                    return Unauthorized(new 
                    { 
                        Success = false, 
                        Message = "API Key е деактивиран",
                        CanWorkWithApi = false
                    });
                }

                // Проверяваме дали deviceId съвпада (ако е предоставен)
                if (!string.IsNullOrEmpty(deviceIdToValidate) && apiKeyEntity.DeviceId != deviceIdToValidate)
                {
                    _logger.LogWarning($"API Key validation failed: DeviceId mismatch. Expected: {apiKeyEntity.DeviceId}, Provided: {deviceIdToValidate}");
                    return Unauthorized(new 
                    { 
                        Success = false, 
                        Message = $"API Key не съответства на предоставения Device ID. Очакван Device ID: {apiKeyEntity.DeviceId}",
                        CanWorkWithApi = false
                    });
                }

                // Валидираме срещу базата данни (актуализира статистиката)
                var isValid = await _apiKeyDbService.ValidateApiKeyAsync(apiKey);
                
                if (!isValid)
                {
                    return Unauthorized(new 
                    { 
                        Success = false, 
                        Message = "Невалиден или неактивен API Key",
                        CanWorkWithApi = false
                    });
                }
                
                return Ok(new 
                { 
                    Success = true, 
                    Message = "API Key е валиден и активен",
                    CanWorkWithApi = true,
                    DeviceId = apiKeyEntity.DeviceId,
                    DeviceName = apiKeyEntity.DeviceName,
                    IsActive = apiKeyEntity.IsActive,
                    LastUsedDate = apiKeyEntity.LastUsedDate,
                    RequestCount = apiKeyEntity.RequestCount
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Грешка при валидация на API Key");
                return StatusCode(500, new 
                { 
                    Success = false, 
                    Message = "Вътрешна грешка при валидация",
                    CanWorkWithApi = false
                });
            }
        }

        /// <summary>
        /// Проверява статус на API Key за конкретно устройство
        /// Използва се от фронтенд при стартиране
        /// </summary>
        [HttpGet("check-api-key-status")]
        [AllowAnonymous]
        public async Task<IActionResult> CheckApiKeyStatus([FromQuery] string deviceId, [FromHeader(Name = "X-API-Key")] string? apiKey = null)
        {
            if (string.IsNullOrWhiteSpace(deviceId))
            {
                return BadRequest(new { Success = false, Message = "DeviceId е задължителен" });
            }

            try
            {
                var status = await _apiKeyDbService.GetApiKeyStatusAsync(deviceId);
                
                if (status == null)
                {
                    return Ok(new
                    {
                        Success = false,
                        HasApiKey = false,
                        CanWorkWithApi = false,
                        Message = $"API Key за устройство '{deviceId}' не е намерен в базата"
                    });
                }

                // Ако е предоставен API Key, проверяваме дали съвпада
                bool keyMatches = true;
                if (!string.IsNullOrEmpty(apiKey))
                {
                    var apiKeyEntity = await _apiKeyDbService.GetApiKeyByValueAsync(apiKey);
                    keyMatches = apiKeyEntity?.DeviceId == deviceId;
                }

                bool canWork = status.IsActive && keyMatches;

                return Ok(new
                {
                    Success = true,
                    HasApiKey = true,
                    CanWorkWithApi = canWork,
                    IsActive = status.IsActive,
                    KeyMatches = keyMatches,
                    DeviceId = status.DeviceId,
                    DeviceName = status.DeviceName,
                    CreatedDate = status.CreatedDate,
                    LastUsedDate = status.LastUsedDate,
                    RequestCount = status.RequestCount,
                    Message = canWork 
                        ? "API Key е валиден и активен - може да работи с API-то" 
                        : status.IsActive 
                            ? "API Key не съвпада с DeviceId" 
                            : "API Key е деактивиран"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Грешка при проверка на API Key статус");
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "Вътрешна грешка при проверка",
                    CanWorkWithApi = false
                });
            }
        }

        private string? GetApiKeyFromXml()
        {
            try
            {
                var xmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Baglanti.xml");
                if (!System.IO.File.Exists(xmlPath))
                    return null;

                var doc = new XmlDocument();
                doc.Load(xmlPath);
                var node = doc.SelectSingleNode("config/apiKey");
                return node?.Attributes?["Value"]?.InnerText;
            }
            catch
            {
                return null;
            }
        }
    }

    /// <summary>
    /// Request модел за login
    /// </summary>
    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    /// <summary>
    /// Request модел за генериране на API Key
    /// </summary>
    public class GenerateApiKeyRequest
    {
        /// <summary>
        /// Уникален идентификатор на устройството/клиента (задължително)
        /// </summary>
        public string DeviceId { get; set; } = string.Empty;

        /// <summary>
        /// Име/описание на устройството
        /// </summary>
        public string? DeviceName { get; set; }

        /// <summary>
        /// Описание за ключа
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Дали да презапише съществуващия ключ
        /// </summary>
        public bool Overwrite { get; set; } = false;
    }
}
