using Nursan.Domain.Interfaces;
using Nursan.Logging.Messages;
using System;

namespace Nursan.Core.Services
{
    /// <summary>
    /// Имплементация на ILogger, използваща Messaglama
    /// </summary>
    public class LoggerService : ILogger
    {
        private readonly Messaglama _messaglama;

        public LoggerService()
        {
            _messaglama = new Messaglama();
        }

        /// <summary>
        /// Логва съобщение на ниво Debug
        /// </summary>
        /// <param name="message">Съобщение за логване</param>
        public void LogDebug(string message)
        {
            _messaglama.messanger($"[DEBUG] {message}");
        }

        /// <summary>
        /// Логва съобщение на ниво Information
        /// </summary>
        /// <param name="message">Съобщение за логване</param>
        public void LogInformation(string message)
        {
            _messaglama.messanger($"[INFO] {message}");
        }

        /// <summary>
        /// Логва съобщение на ниво Warning
        /// </summary>
        /// <param name="message">Съобщение за логване</param>
        public void LogWarning(string message)
        {
            _messaglama.messanger($"[WARN] {message}");
        }

        /// <summary>
        /// Логва съобщение на ниво Error
        /// </summary>
        /// <param name="message">Съобщение за логване</param>
        public void LogError(string message)
        {
            _messaglama.messanger($"[ERROR] {message}");
        }

        /// <summary>
        /// Логва съобщение на ниво Critical
        /// </summary>
        /// <param name="message">Съобщение за логване</param>
        public void LogCritical(string message)
        {
            _messaglama.messanger($"[CRITICAL] {message}");
        }

        /// <summary>
        /// Логва изключение с съобщение на ниво Error
        /// </summary>
        /// <param name="exception">Изключение за логване</param>
        /// <param name="message">Съобщение за логване</param>
        public void LogException(Exception exception, string message = null)
        {
            string fullMessage = string.IsNullOrEmpty(message)
                ? $"[EXCEPTION] {exception.Message}"
                : $"[EXCEPTION] {message}: {exception.Message}";
                
            _messaglama.messanger(fullMessage);
        }

        #region Допълнителни методи за обратна съвместимост

        /// <summary>
        /// Логва общо съобщение (обратна съвместимост)
        /// </summary>
        /// <param name="message">Съобщение за логване</param>
        public void Log(string message)
        {
            LogInformation(message);
        }

        /// <summary>
        /// Логва съобщение с идентификатор (обратна съвместимост)
        /// </summary>
        /// <param name="message">Съобщение за логване</param>
        /// <param name="id">Идентификатор</param>
        public void LogWithId(string message, string id)
        {
            LogInformation($"{message} [ID: {id}]");
        }

        /// <summary>
        /// Логва съобщение за грешка с идентификатор (обратна съвместимост)
        /// </summary>
        /// <param name="message">Съобщение за логване</param>
        /// <param name="id">Идентификатор</param>
        public void LogErrorWithId(string message, string id)
        {
            LogError($"{message} [ID: {id}]");
        }

        #endregion
    }
} 