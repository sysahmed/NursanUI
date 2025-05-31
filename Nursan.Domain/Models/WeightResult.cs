using System;

namespace Nursan.Domain.Models
{
    /// <summary>
    /// Статус на претегляне
    /// </summary>
    public enum WeightStatus
    {
        /// <summary>
        /// Обработено
        /// </summary>
        Processed,
        
        /// <summary>
        /// Чакащо
        /// </summary>
        Pending,
        
        /// <summary>
        /// Грешка
        /// </summary>
        Error,
        
        /// <summary>
        /// Отказано
        /// </summary>
        Rejected
    }

    /// <summary>
    /// Резултат от претегляне
    /// </summary>
    public class WeightResult
    {
        /// <summary>
        /// Идентификатор на везната
        /// </summary>
        public string ScaleId { get; set; }

        /// <summary>
        /// Тегло в килограми
        /// </summary>
        public decimal Weight { get; set; }

        /// <summary>
        /// Материал или описание на претегляния товар
        /// </summary>
        public string Material { get; set; }

        /// <summary>
        /// Час и дата на претеглянето
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Допълнителна информация за претеглянето
        /// </summary>
        public string AdditionalInfo { get; set; }

        /// <summary>
        /// Индикатор дали претеглянето е успешно
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Съобщение за грешка, ако има такава
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Статус на претеглянето
        /// </summary>
        public WeightStatus Status { get; set; }

        /// <summary>
        /// Флаг дали е отпечатан етикет
        /// </summary>
        public bool IsLabelPrinted { get; set; }
    }
} 