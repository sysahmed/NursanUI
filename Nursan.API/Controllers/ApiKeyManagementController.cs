using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nursan.API.Services;
using Nursan.Domain.AmbarModels;
using Nursan.Domain.Entity;

namespace Nursan.API.Controllers
{
    /// <summary>
    /// MVC контролер за управление на API Keys и станции
    /// </summary>
    [Authorize]
    public class ApiKeyManagementController : Controller
    {
        private readonly ILogger<ApiKeyManagementController> _logger;
        private readonly ApiKeyDbService _apiKeyDbService;
        private readonly UretimOtomasyonContext _context;
        private readonly EncryptionService _encryptionService;
        private readonly XmlSyncService _xmlSyncService;

        public ApiKeyManagementController(
            ILogger<ApiKeyManagementController> logger,
            ApiKeyDbService apiKeyDbService,
            UretimOtomasyonContext context,
            EncryptionService encryptionService,
            XmlSyncService xmlSyncService)
        {
            _logger = logger;
            _apiKeyDbService = apiKeyDbService;
            _context = context;
            _encryptionService = encryptionService;
            _xmlSyncService = xmlSyncService;
        }

        /// <summary>
        /// Главна страница - списък с всички станции и техните API Keys
        /// </summary>
        public async Task<IActionResult> Index()
        {
            try
            {
                var apiKeys = await _apiKeyDbService.GetAllApiKeysAsync();
                
                var viewModel = apiKeys.Select(k => new ApiKeyViewModel
                {
                    Id = k.Id,
                    DeviceId = k.DeviceId,
                    DeviceName = k.DeviceName,
                    IsActive = k.IsActive,
                    CreatedDate = k.CreatedDate,
                    LastUsedDate = k.LastUsedDate,
                    RequestCount = k.RequestCount,
                    CreatedBy = k.CreatedBy,
                    Description = k.Description
                }).ToList();

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Грешка при зареждане на API Keys");
                ViewBag.Error = "Грешка при зареждане на данни";
                return View(new List<ApiKeyViewModel>());
            }
        }

        /// <summary>
        /// Страница за създаване на нов API Key за станция
        /// </summary>
        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreateApiKeyViewModel());
        }

        /// <summary>
        /// Обработка на формата за създаване
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateApiKeyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // Генерираме API Key
                var apiKey = GenerateSecureApiKey();
                
                // Записваме в базата
                var username = User.Identity?.Name ?? "Unknown";
                var success = await _apiKeyDbService.SaveApiKeyAsync(
                    model.DeviceId,
                    apiKey,
                    model.DeviceName,
                    username,
                    model.Description,
                    isActive: true);

                if (!success)
                {
                    ViewBag.Error = "Не може да се създаде API Key. Може би устройството вече съществува.";
                    return View(model);
                }

                // Записваме в XML файл (за конкретната станция)
                await _xmlSyncService.SaveApiKeyToXmlForDeviceAsync(model.DeviceId, apiKey);

                TempData["Success"] = $"API Key е създаден успешно за станция '{model.DeviceId}'";
                TempData["ApiKey"] = apiKey; // Временно показваме ключа
                
                return RedirectToAction("Details", new { deviceId = model.DeviceId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Грешка при създаване на API Key");
                ViewBag.Error = $"Грешка: {ex.Message}";
                return View(model);
            }
        }

        /// <summary>
        /// Детайли за конкретна станция
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Details(string deviceId)
        {
            if (string.IsNullOrWhiteSpace(deviceId))
            {
                return NotFound();
            }

            try
            {
                var status = await _apiKeyDbService.GetApiKeyStatusAsync(deviceId);
                if (status == null)
                {
                    return NotFound();
                }

                var viewModel = new ApiKeyDetailsViewModel
                {
                    DeviceId = status.DeviceId ?? deviceId,
                    DeviceName = status.DeviceName,
                    IsActive = status.IsActive,
                    CreatedDate = status.CreatedDate,
                    LastUsedDate = status.LastUsedDate,
                    RequestCount = status.RequestCount,
                    CreatedBy = status.CreatedBy,
                    Description = status.Description
                };

                // Показваме ключа само ако е временно записан в TempData
                if (TempData["ApiKey"] != null)
                {
                    viewModel.ApiKey = TempData["ApiKey"].ToString();
                }

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Грешка при зареждане на детайли");
                return NotFound();
            }
        }

        /// <summary>
        /// Деактивира API Key
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Revoke(string deviceId)
        {
            if (string.IsNullOrWhiteSpace(deviceId))
            {
                return BadRequest();
            }

            try
            {
                var success = await _apiKeyDbService.RevokeApiKeyAsync(deviceId);
                if (success)
                {
                    TempData["Success"] = $"API Key за станция '{deviceId}' е деактивиран успешно";
                }
                else
                {
                    TempData["Error"] = "Не може да се деактивира API Key";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Грешка при деактивиране");
                TempData["Error"] = $"Грешка: {ex.Message}";
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Активира API Key
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Activate(string deviceId)
        {
            if (string.IsNullOrWhiteSpace(deviceId))
            {
                return BadRequest();
            }

            try
            {
                var success = await _apiKeyDbService.ActivateApiKeyAsync(deviceId: deviceId);
                if (success)
                {
                    TempData["Success"] = $"API Key за станция '{deviceId}' е активиран успешно";
                }
                else
                {
                    TempData["Error"] = "Не може да се активира API Key";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Грешка при активиране");
                TempData["Error"] = $"Грешка: {ex.Message}";
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Изтрива API Key
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string deviceId)
        {
            if (string.IsNullOrWhiteSpace(deviceId))
            {
                return BadRequest();
            }

            try
            {
                var success = await _apiKeyDbService.DeleteApiKeyAsync(deviceId);
                if (success)
                {
                    TempData["Success"] = $"API Key за станция '{deviceId}' е изтрит успешно";
                }
                else
                {
                    TempData["Error"] = "Не може да се изтрие API Key";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Грешка при изтриване");
                TempData["Error"] = $"Грешка: {ex.Message}";
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Регенерира API Key за станция
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Regenerate(string deviceId, string? deviceName = null)
        {
            if (string.IsNullOrWhiteSpace(deviceId))
            {
                return BadRequest();
            }

            try
            {
                // Генерираме нов API Key
                var apiKey = GenerateSecureApiKey();
                
                // Записваме в базата
                var username = User.Identity?.Name ?? "Unknown";
                var success = await _apiKeyDbService.SaveApiKeyAsync(
                    deviceId,
                    apiKey,
                    deviceName,
                    username,
                    "Regenerated API Key",
                    isActive: true);

                if (!success)
                {
                    TempData["Error"] = "Не може да се регенерира API Key";
                    return RedirectToAction("Index");
                }

                // Записваме в XML
                await _xmlSyncService.SaveApiKeyToXmlForDeviceAsync(deviceId, apiKey);

                TempData["Success"] = $"Нов API Key е генериран за станция '{deviceId}'";
                TempData["ApiKey"] = apiKey;
                
                return RedirectToAction("Details", new { deviceId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Грешка при регенериране");
                TempData["Error"] = $"Грешка: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// Генерира сигурен API Key
        /// </summary>
        private string GenerateSecureApiKey()
        {
            using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                var bytes = new byte[32]; // 256 бита
                rng.GetBytes(bytes);
                return Convert.ToBase64String(bytes).Replace("+", "-").Replace("/", "_").TrimEnd('=');
            }
        }
    }

    /// <summary>
    /// View модел за списък с API Keys
    /// </summary>
    public class ApiKeyViewModel
    {
        public int Id { get; set; }
        public string DeviceId { get; set; } = string.Empty;
        public string? DeviceName { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastUsedDate { get; set; }
        public long RequestCount { get; set; }
        public string? CreatedBy { get; set; }
        public string? Description { get; set; }
    }

    /// <summary>
    /// View модел за създаване на API Key
    /// </summary>
    public class CreateApiKeyViewModel
    {
        [Required(ErrorMessage = "DeviceId е задължителен")]
        [Display(Name = "Идентификатор на станцията")]
        public string DeviceId { get; set; } = string.Empty;

        [Display(Name = "Име на станцията")]
        public string? DeviceName { get; set; }

        [Display(Name = "Описание")]
        public string? Description { get; set; }
    }

    /// <summary>
    /// View модел за детайли на API Key
    /// </summary>
    public class ApiKeyDetailsViewModel
    {
        public string DeviceId { get; set; } = string.Empty;
        public string? DeviceName { get; set; }
        public string? ApiKey { get; set; } // Показва се само веднъж при генериране
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastUsedDate { get; set; }
        public long RequestCount { get; set; }
        public string? CreatedBy { get; set; }
        public string? Description { get; set; }
    }
}

