using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Nursan.Business.Logging
{
    /// <summary>
    /// Осигурява структурирано логване с маскиране на чувствителни данни.
    /// </summary>
    public sealed class StructuredLogger
    {
        private readonly object writeLock;
        private readonly string category;
        private readonly string logDirectory;
        private readonly JsonSerializerOptions serializerOptions;

        /// <summary>
        /// Инициализира нов екземпляр на StructuredLogger.
        /// </summary>
        /// <param name="categoryName">Име на категорията.</param>
        public StructuredLogger(string categoryName)
        {
            writeLock = new object();
            category = string.IsNullOrWhiteSpace(categoryName) ? "General" : categoryName;
            logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LOGS");
            serializerOptions = new JsonSerializerOptions
            {
                WriteIndented = false
            };

            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }
        }

        /// <summary>
        /// Логва информационно събитие.
        /// </summary>
        /// <param name="eventName">Име на събитието.</param>
        /// <param name="data">Контекстни данни.</param>
        public void LogInfo(string eventName, IDictionary<string, string> data)
        {
            Log("Information", eventName, data);
        }

        /// <summary>
        /// Логва предупреждение.
        /// </summary>
        /// <param name="eventName">Име на събитието.</param>
        /// <param name="data">Контекстни данни.</param>
        public void LogWarning(string eventName, IDictionary<string, string> data)
        {
            Log("Warning", eventName, data);
        }

        /// <summary>
        /// Логва грешка.
        /// </summary>
        /// <param name="eventName">Име на събитието.</param>
        /// <param name="data">Контекстни данни.</param>
        public void LogError(string eventName, IDictionary<string, string> data)
        {
            Log("Error", eventName, data);
        }

        private void Log(string level, string eventName, IDictionary<string, string> data)
        {
            StructuredLogEntry entry = new StructuredLogEntry
            {
                TimestampUtc = DateTime.Now.ToString("o", CultureInfo.InvariantCulture),
                Level = level,
                Category = category,
                EventName = string.IsNullOrWhiteSpace(eventName) ? "UnknownEvent" : eventName,
                Data = data ?? new Dictionary<string, string>()
            };

            string fileName = string.Format(
                CultureInfo.InvariantCulture,
                "STRUCTURED_{0:yyyyMMdd}.log",
                DateTime.Now);

            string filePath = Path.Combine(logDirectory, fileName);
            string jsonLine = JsonSerializer.Serialize(entry, serializerOptions) + Environment.NewLine;

            lock (writeLock)
            {
                File.AppendAllText(filePath, jsonLine, Encoding.ASCII);
            }
        }
    }

    /// <summary>
    /// Представлява един ред от структурирания лог.
    /// </summary>
    public sealed class StructuredLogEntry
    {
        /// <summary>
        /// Времева отметка в UTC.
        /// </summary>
        public string TimestampUtc { get; set; }

        /// <summary>
        /// Ниво на логване.
        /// </summary>
        public string Level { get; set; }

        /// <summary>
        /// Категория.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Име на събитието.
        /// </summary>
        public string EventName { get; set; }

        /// <summary>
        /// Допълнителни данни.
        /// </summary>
        public IDictionary<string, string> Data { get; set; }
    }
}

