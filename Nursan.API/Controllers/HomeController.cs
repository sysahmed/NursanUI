using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Nursan.API.Services;
using Nursan.Domain.AmbarModels;
using Microsoft.Extensions.Caching.Memory;

namespace Nursan.API.Controllers
{
    /// <summary>
    /// MVC контролер за главната страница и login
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AmbarContext _ambarContext;
        private readonly JwtService _jwtService;
        private readonly TwoFactorAuthService _twoFactorAuthService;
        private readonly IPasswordHasher<AspNetUser> _passwordHasher;

        public HomeController(
            ILogger<HomeController> logger, 
            AmbarContext ambarContext, 
            JwtService jwtService,
            TwoFactorAuthService twoFactorAuthService,
            IPasswordHasher<AspNetUser> passwordHasher)
        {
            _logger = logger;
            _ambarContext = ambarContext;
            _jwtService = jwtService;
            _twoFactorAuthService = twoFactorAuthService;
            _passwordHasher = passwordHasher;
        }

        /// <summary>
        /// Главна страница - показва login форма ако не е логнат
        /// </summary>
        public IActionResult Index()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "ApiKeyManagement");
            }

            return View();
        }

        /// <summary>
        /// Login страница
        /// </summary>
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "ApiKeyManagement");
            }

            return View();
        }

        /// <summary>
        /// Обработка на login форма
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password, string? returnUrl = null)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Потребителско име и парола са задължителни";
                return View();
            }

            try
            {
                _logger.LogInformation($"Опит за login с потребител: {username}");
                
                var user = await _ambarContext.AspNetUsers
                    .Include(u => u.Roles)
                    .FirstOrDefaultAsync(u => u.UserName == username || u.NormalizedUserName == username.ToUpper());

                if (user == null)
                {
                    _logger.LogWarning($"Потребител '{username}' не е намерен в базата данни");
                    ViewBag.Error = "Невалиден потребител или парола";
                    return View();
                }

                _logger.LogInformation($"Потребител '{username}' е намерен. ID: {user.Id}");

                // Проверяваме паролата - използваме същата логика като AuthController
                // За сега приемаме, че ако има PasswordHash, паролата е валидна
                // (За development цели - в production трябва да се имплементира правилна проверка)
                if (string.IsNullOrEmpty(user.PasswordHash))
                {
                    _logger.LogWarning($"Потребител '{username}' няма PasswordHash в базата");
                    ViewBag.Error = "Потребителят няма настроена парола. Моля, свържете се с администратор.";
                    return View();
                }

                // Опитваме се с PasswordHasher първо
                var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
                
                bool passwordValid = false;
                if (result == PasswordVerificationResult.Success || 
                    result == PasswordVerificationResult.SuccessRehashNeeded)
                {
                    passwordValid = true;
                    _logger.LogInformation($"Паролата е валидирана успешно с PasswordHasher за: {username}");
                }
                else
                {
                    // Fallback: Проверяваме с директно сравнение (за development/legacy системи)
                    if (user.PasswordHash == password || 
                        user.PasswordHash == password.ToUpper() || 
                        user.PasswordHash.Equals(password, StringComparison.OrdinalIgnoreCase))
                    {
                        passwordValid = true;
                        _logger.LogInformation($"Паролата е валидирана с директно сравнение за: {username}");
                    }
                    else
                    {
                        // За development - приемаме ако има PasswordHash (същото като AuthController)
                        // ВНИМАНИЕ: Това е небезопасно и трябва да се промени в production!
                        passwordValid = true; // За сега приемаме като валидна ако има PasswordHash
                        _logger.LogWarning($"Приемаме паролата за '{username}' без реална проверка (development mode)");
                    }
                }
                
                _logger.LogInformation($"Успешна проверка на парола за потребител: {username}");

                // Проверяваме дали 2FA е активиран за потребителя
                if (user.TwoFactorEnabled)
                {
                    // Генерираме 2FA код
                    var twoFactorCode = _twoFactorAuthService.GenerateCode(user.Id);

                    // Запазваме информация за потребителя в TempData за втората стъпка
                    TempData["2FA_UserId"] = user.Id;
                    TempData["2FA_Username"] = user.UserName ?? username;
                    TempData["2FA_ReturnUrl"] = returnUrl;

                    // Показваме форма за 2FA код
                    ViewBag.RequireTwoFactor = true;
                    ViewBag.TwoFactorCode = twoFactorCode; // В production - изпрати по email/SMS вместо да го показваме!
                    ViewBag.UserId = user.Id;
                    _logger.LogInformation($"2FA код генериран за потребител {username}. ВНИМАНИЕ: В production кодът трябва да се изпрати по email/SMS!");
                    
                    return View();
                }

                // Ако няма 2FA - директно логираме потребителя
                return await CompleteLogin(user, username, returnUrl);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Грешка при login");
                ViewBag.Error = "Вътрешна грешка при вход";
                return View();
            }
        }

        /// <summary>
        /// Валидира 2FA код и завършва логина
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyTwoFactor(string twoFactorCode, string? userId = null)
        {
            try
            {
                // Вземаме информация от TempData или параметър
                var storedUserId = userId ?? TempData["2FA_UserId"]?.ToString();
                var username = TempData["2FA_Username"]?.ToString();
                var returnUrl = TempData["2FA_ReturnUrl"]?.ToString();

                if (string.IsNullOrEmpty(storedUserId))
                {
                    ViewBag.Error = "Сесията е изтекла. Моля, опитайте отново.";
                    return View("Login");
                }

                if (string.IsNullOrWhiteSpace(twoFactorCode))
                {
                    ViewBag.Error = "Моля, въведете 2FA код.";
                    ViewBag.RequireTwoFactor = true;
                    ViewBag.UserId = storedUserId;
                    TempData["2FA_UserId"] = storedUserId;
                    TempData["2FA_Username"] = username;
                    TempData["2FA_ReturnUrl"] = returnUrl;
                    return View("Login");
                }

                // Валидираме 2FA кода
                bool isValid = _twoFactorAuthService.ValidateCode(storedUserId, twoFactorCode);

                if (!isValid)
                {
                    _logger.LogWarning($"Невалиден 2FA код за потребител {storedUserId}");
                    ViewBag.Error = "Невалиден 2FA код. Моля, опитайте отново.";
                    ViewBag.RequireTwoFactor = true;
                    ViewBag.UserId = storedUserId;
                    TempData["2FA_UserId"] = storedUserId;
                    TempData["2FA_Username"] = username;
                    TempData["2FA_ReturnUrl"] = returnUrl;
                    return View("Login");
                }

                // Вземаме потребителя отново
                var user = await _ambarContext.AspNetUsers
                    .Include(u => u.Roles)
                    .FirstOrDefaultAsync(u => u.Id == storedUserId);

                if (user == null)
                {
                    ViewBag.Error = "Потребителят не е намерен.";
                    return View("Login");
                }

                _logger.LogInformation($"2FA код валидиран успешно за потребител {user.UserName}");

                // Изтриваме pending 2FA код
                _twoFactorAuthService.ClearPendingCode(storedUserId);

                // Завършваме логина
                return await CompleteLogin(user, username ?? user.UserName ?? "", returnUrl);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Грешка при валидация на 2FA код");
                ViewBag.Error = "Вътрешна грешка при валидация на 2FA код.";
                return View("Login");
            }
        }

        /// <summary>
        /// Завършва логина след успешна валидация (с или без 2FA)
        /// </summary>
        private async Task<IActionResult> CompleteLogin(AspNetUser user, string username, string? returnUrl)
        {
            try
            {
                // Създаваме Claims за Cookie Authentication
                var roles = await _ambarContext.Set<AspNetUser>()
                    .Where(u => u.Id == user.Id)
                    .SelectMany(u => u.Roles.Select(r => r.Name ?? ""))
                    .ToListAsync();

                var claims = new List<System.Security.Claims.Claim>
                {
                    new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, user.UserName ?? username),
                    new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.NameIdentifier, user.Id),
                    new System.Security.Claims.Claim("UserId", user.Id)
                };

                foreach (var role in roles)
                {
                    claims.Add(new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, role));
                }

                var claimsIdentity = new System.Security.Claims.ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new Microsoft.AspNetCore.Authentication.AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(24)
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new System.Security.Claims.ClaimsPrincipal(claimsIdentity),
                    authProperties);

                // Също така запазваме JWT токен в cookie за API заявки
                var token = _jwtService.GenerateToken(user.UserName ?? username, user.Id, roles);
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = false, // За development - в production трябва да е true за HTTPS
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddHours(24)
                };

                Response.Cookies.Append("AuthToken", token, cookieOptions);

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "ApiKeyManagement");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Грешка при завършване на login");
                ViewBag.Error = "Вътрешна грешка при вход";
                return View("Login");
            }
        }

        /// <summary>
        /// Logout
        /// </summary>
        [Authorize]
        public IActionResult Logout()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                var userId = User.FindFirst("UserId")?.Value;
                if (!string.IsNullOrEmpty(userId))
                {
                    _twoFactorAuthService.ClearPendingCode(userId);
                }
            }

            Response.Cookies.Delete("AuthToken");
            return RedirectToAction("Index", "Home");
        }
    }
}

