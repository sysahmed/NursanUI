using Nursan.Domain.Interfaces;
using System;
using System.Threading.Tasks;

namespace Nursan.Core.Services
{
    /// <summary>
    /// Базов клас за всички услуги в приложението
    /// </summary>
    public abstract class BaseService
    {
        /// <summary>
        /// Логър за записване на събития
        /// </summary>
        protected readonly ILogger _logger;

        /// <summary>
        /// Инициализира нова инстанция на базовия клас с логър
        /// </summary>
        /// <param name="logger">Логър за записване на събития</param>
        protected BaseService(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Изпълнява функция с обработка на грешки
        /// </summary>
        /// <typeparam name="T">Тип на резултата</typeparam>
        /// <param name="func">Функция за изпълнение</param>
        /// <param name="defaultValue">Стойност по подразбиране в случай на грешка</param>
        /// <param name="errorMessage">Съобщение за грешка</param>
        /// <returns>Резултат от изпълнението на функцията или стойност по подразбиране</returns>
        protected async Task<T> ExecuteWithErrorHandling<T>(Func<Task<T>> func, T defaultValue, string errorMessage = "Възникна грешка")
        {
            try
            {
                return await func();
            }
            catch (Exception ex)
            {
                _logger.LogError($"{errorMessage}: {ex.Message}");
                return defaultValue;
            }
        }

        /// <summary>
        /// Проверява дали обект не е null и записва грешка, ако е null
        /// </summary>
        /// <typeparam name="T">Тип на обекта</typeparam>
        /// <param name="obj">Обект за проверка</param>
        /// <param name="paramName">Име на параметъра</param>
        /// <param name="errorMessage">Съобщение за грешка</param>
        /// <returns>true ако обектът не е null, false в противен случай</returns>
        protected bool ValidateNotNull<T>(T obj, string paramName, string errorMessage = null)
        {
            if (obj == null)
            {
                _logger.LogError($"{errorMessage ?? $"Параметърът '{paramName}' не може да бъде null"}");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Валидира дали низ не е празен или null и записва съобщение при неуспех
        /// </summary>
        /// <param name="value">Низ за проверка</param>
        /// <param name="paramName">Име на параметъра</param>
        /// <param name="errorMessage">Съобщение за грешка</param>
        /// <returns>true ако низът не е празен или null, false в противен случай</returns>
        protected bool ValidateNotNullOrEmpty(string value, string paramName, string errorMessage)
        {
            if (string.IsNullOrEmpty(value))
            {
                _logger.LogError($"{errorMessage}: {paramName} е празен или null");
                return false;
            }
            return true;
        }
    }
} 