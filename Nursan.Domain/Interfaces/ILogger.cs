using System;

namespace Nursan.Domain.Interfaces
{
    /// <summary>
    /// Интерфейс за логване на събития в приложението
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Логва съобщение на ниво Debug
        /// </summary>
        /// <param name="message">Съобщение за логване</param>
        void LogDebug(string message);

        /// <summary>
        /// Логва съобщение на ниво Information
        /// </summary>
        /// <param name="message">Съобщение за логване</param>
        void LogInformation(string message);

        /// <summary>
        /// Логва съобщение на ниво Warning
        /// </summary>
        /// <param name="message">Съобщение за логване</param>
        void LogWarning(string message);

        /// <summary>
        /// Логва съобщение на ниво Error
        /// </summary>
        /// <param name="message">Съобщение за логване</param>
        void LogError(string message);

        /// <summary>
        /// Логва съобщение на ниво Critical
        /// </summary>
        /// <param name="message">Съобщение за логване</param>
        void LogCritical(string message);

        /// <summary>
        /// Логва изключение с съобщение на ниво Error
        /// </summary>
        /// <param name="exception">Изключение за логване</param>
        /// <param name="message">Съобщение за логване</param>
        void LogException(Exception exception, string message = null);
    }
} 