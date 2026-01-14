using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nursan.API.Services;
using Nursan.Business.Logging;

namespace Nursan.API.Middleware
{
    /// <summary>
    /// Middleware за валидация на API Key (от криптиран файл)
    /// </summary>
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        private readonly StructuredLogger _logger;
        private const string API_KEY_HEADER = "X-API-Key";
        private const string API_KEY_CONFIG_KEY = "ApiSettings:ApiKey";

        public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
            _logger = new StructuredLogger("ApiKeyMiddleware");
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                var path = context.Request.Path.Value?.ToLower() ?? "";
                
                // Пропускаме всички MVC routes (които не започват с /api/)
                // MVC routes: /Home/*, /ApiKeyManagement/*, / (главна страница), static files
                if (!path.StartsWith("/api/"))
                {
                    // Пропускаме всички non-API routes - те са MVC Views и не нуждаят API Key
                    await _next(context);
                    return;
                }
            
            // От тук нататък само API routes (които започват с /api/)
            
            // Пропускаме Swagger, health check и authentication endpoints
            // Също така версионираните пътища: /api/v1/..., /api/v2/...
            if (path.Contains("/swagger") || 
                path.Contains("/health") || 
                path.Contains("/api/auth/login") ||
                path.Contains("/api/auth/validate-api-key") ||
                (path.StartsWith("/api/v") && path.Contains("/auth/")))
            {
                await _next(context);
                return;
            }
            
            // /api/auth/generate-api-key и други API Key management endpoints 
            // изискват JWT аутентикация, не API Key
            if (path.Contains("/api/auth/generate-api-key") ||
                path.Contains("/api/auth/api-key-status") ||
                path.Contains("/api/auth/revoke-api-key") ||
                path.Contains("/api/auth/activate-api-key") ||
                path.Contains("/api/auth/api-key")) // DELETE endpoint
            {
                // Проверяваме за JWT токен в Authorization header
                if (context.Request.Headers.ContainsKey("Authorization"))
                {
                    await _next(context);
                    return;
                }
                // Ако няма JWT токен, връщаме 401
                context.Response.StatusCode = 401;
                context.Response.ContentType = "text/plain; charset=utf-8";
                await context.Response.WriteAsync("JWT токен е задължителен за този endpoint. Моля, извършете login първо.");
                return;
            }

            // За всички останали API routes изискваме API Key и DeviceId
            if (!context.Request.Headers.TryGetValue(API_KEY_HEADER, out var extractedApiKey))
            {
                _logger.LogWarning("ApiKeyMissing", new Dictionary<string, string>
                {
                    { "Path", context.Request.Path },
                    { "Method", context.Request.Method },
                    { "RemoteIP", context.Connection.RemoteIpAddress?.ToString() ?? "Unknown" }
                });
                
                context.Response.StatusCode = 401;
                context.Response.ContentType = "text/plain; charset=utf-8";
                await context.Response.WriteAsync("API Key не е предоставен. Моля, добавете 'X-API-Key' header.");
                return;
            }

            // DeviceId е опционален - ако не е предоставен, опитваме се да валидираме само с API Key
            context.Request.Headers.TryGetValue("X-Device-Id", out var extractedDeviceId);
            string? deviceId = extractedDeviceId.ToString();

            // Валидираме API Key срещу базата данни
            var isValid = await ValidateApiKeyFromDatabaseAsync(context, extractedApiKey.ToString(), deviceId, _logger);
            
            if (!isValid)
            {
                _logger.LogWarning("ApiKeyValidationFailed", new Dictionary<string, string>
                {
                    { "Path", context.Request.Path },
                    { "Method", context.Request.Method },
                    { "DeviceId", deviceId ?? "NotProvided" },
                    { "RemoteIP", context.Connection.RemoteIpAddress?.ToString() ?? "Unknown" }
                });
                
                context.Response.StatusCode = 401;
                context.Response.ContentType = "text/plain; charset=utf-8";
                await context.Response.WriteAsync("Невалиден или неактивен API Key за предоставения Device ID.");
                return;
            }
            
            // Успешна валидация - логваме
            _logger.LogInfo("ApiKeyValidated", new Dictionary<string, string>
            {
                { "Path", context.Request.Path },
                { "Method", context.Request.Method },
                { "DeviceId", deviceId ?? "NotProvided" },
                { "RemoteIP", context.Connection.RemoteIpAddress?.ToString() ?? "Unknown" }
            });

            await _next(context);
            }
            catch (Exception ex)
            {
                // Логваме грешката в StructuredLogger
                _logger.LogError("ApiKeyMiddlewareException", new Dictionary<string, string>
                {
                    { "Message", ex.Message },
                    { "StackTrace", ex.StackTrace ?? "N/A" },
                    { "Path", context.Request.Path },
                    { "Method", context.Request.Method }
                });
                
                // Връщаме 500 error ако все още не сме започнали response
                if (!context.Response.HasStarted)
                {
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "text/plain; charset=utf-8";
                    await context.Response.WriteAsync("Вътрешна грешка на сървъра. Моля, опитайте отново по-късно.");
                }
            }
        }

        /// <summary>
        /// Безопасно сравнение на strings против timing attacks
        /// </summary>
        private bool SecureCompare(string a, string b)
        {
            if (a == null || b == null || a.Length != b.Length)
                return false;

            int result = 0;
            for (int i = 0; i < a.Length; i++)
            {
                result |= a[i] ^ b[i];
            }
            return result == 0;
        }

        /// <summary>
        /// Валидира API Key срещу базата данни
        /// </summary>
        private static async Task<bool> ValidateApiKeyFromDatabaseAsync(HttpContext context, string plainApiKey, string? deviceId = null, StructuredLogger? logger = null)
        {
            try
            {
                // Използваме service locator pattern за да вземем ApiKeyDbService
                var apiKeyDbService = context.RequestServices.GetService(typeof(ApiKeyDbService)) as ApiKeyDbService;
                
                if (apiKeyDbService == null)
                {
                    // Fallback към стария метод ако няма service
                    var apiKeyStorage = context.RequestServices.GetService(typeof(ApiKeyStorageService)) as ApiKeyStorageService;
                    var storedKey = apiKeyStorage?.GetApiKey();
                    
                    if (string.IsNullOrEmpty(storedKey))
                        return false;

                    // Просто сравнение за fallback
                    return SecureCompareFallback(plainApiKey, storedKey);
                }

                // Валидираме през базата данни
                // Ако има DeviceId, проверяваме дали API Key принадлежи на това устройство
                if (!string.IsNullOrEmpty(deviceId))
                {
                    var apiKeyEntity = await apiKeyDbService.GetApiKeyByValueAsync(plainApiKey);
                    if (apiKeyEntity != null && apiKeyEntity.DeviceId != deviceId)
                    {
                        // API Key не принадлежи на това устройство
                        return false;
                    }
                }

                // Валидираме API Key (включва проверка за активност и декриптиране)
                return await apiKeyDbService.ValidateApiKeyAsync(plainApiKey);
            }
            catch (Exception ex)
            {
                // Логваме грешката за диагностика
                logger ??= new StructuredLogger("ApiKeyMiddleware");
                logger.LogError("ApiKeyValidationError", new Dictionary<string, string>
                {
                    { "Message", ex.Message },
                    { "StackTrace", ex.StackTrace ?? "N/A" },
                    { "DeviceId", deviceId ?? "NotProvided" }
                });
                return false;
            }
        }

        /// <summary>
        /// Fallback метод за сравнение (ако няма database service)
        /// </summary>
        private static bool SecureCompareFallback(string a, string b)
        {
            if (a == null || b == null || a.Length != b.Length)
                return false;

            int result = 0;
            for (int i = 0; i < a.Length; i++)
            {
                result |= a[i] ^ b[i];
            }
            return result == 0;
        }
    }

    /// <summary>
    /// Extension метод за регистрация на middleware
    /// </summary>
    public static class ApiKeyMiddlewareExtensions
    {
        public static IApplicationBuilder UseApiKeyAuthentication(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApiKeyMiddleware>();
        }
    }
}
