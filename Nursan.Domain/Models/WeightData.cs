using System;

namespace Nursan.Domain.Models
{
    /// <summary>
    /// Данни от претегляне
    /// </summary>
    public class WeightData
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
        public DateTime WeightTime { get; set; }

        /// <summary>
        /// Допълнителна информация за претеглянето
        /// </summary>
        public string AdditionalInfo { get; set; }
    }
} 