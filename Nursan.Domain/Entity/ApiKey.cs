using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nursan.Domain.Entity
{
    /// <summary>
    /// Модел за API Keys в базата данни
    /// </summary>
    [Table("ApiKeys")]
    public class ApiKey
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Уникален идентификатор на устройството/клиента
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string DeviceId { get; set; } = string.Empty;

        /// <summary>
        /// Име/описание на устройството
        /// </summary>
        [MaxLength(200)]
        public string? DeviceName { get; set; }

        /// <summary>
        /// API Key стойност (криптирана)
        /// </summary>
        [Required]
        [MaxLength(500)]
        public string KeyValue { get; set; } = string.Empty;

        /// <summary>
        /// Дали ключът е активен
        /// </summary>
        [Required]
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Дата на създаване
        /// </summary>
        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Потребител който е създал ключа
        /// </summary>
        [MaxLength(100)]
        public string? CreatedBy { get; set; }

        /// <summary>
        /// Дата на последно използване
        /// </summary>
        public DateTime? LastUsedDate { get; set; }

        /// <summary>
        /// Описание/бележки за ключа
        /// </summary>
        [MaxLength(500)]
        public string? Description { get; set; }

        /// <summary>
        /// Дата на изтичане (опционално)
        /// </summary>
        public DateTime? ExpiryDate { get; set; }

        /// <summary>
        /// Брой заявки с този ключ
        /// </summary>
        public long RequestCount { get; set; } = 0;
    }
}

