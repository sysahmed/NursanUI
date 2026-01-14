using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nursan.Domain.VideoModels
{
    /// <summary>
    /// Модел за видео файл
    /// </summary>
    [Table("Videos")]
    public class Video
    {
        /// <summary>
        /// Уникален идентификатор на видеото
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Заглавие на видеото
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Описание на видеото
        /// </summary>
        [StringLength(500)]
        public string? Description { get; set; }

        /// <summary>
        /// Име на файла
        /// </summary>
        [Required]
        public string FileName { get; set; } = string.Empty;

        /// <summary>
        /// URL адрес към видео файла
        /// </summary>
        [Required]
        public string VideoUrl { get; set; } = string.Empty;

        /// <summary>
        /// Абсолютен URL адрес към видео файла
        /// </summary>
        public string? AbsoluteVideoUrl { get; set; }

        /// <summary>
        /// Път към QR кода за видеото
        /// </summary>
        public string? QrCodePath { get; set; }

        /// <summary>
        /// Дата на качване
        /// </summary>
        public DateTime UploadDate { get; set; }

        /// <summary>
        /// Потребител, качил видеото
        /// </summary>
        [StringLength(256)]
        public string UploadedBy { get; set; } = string.Empty;

        /// <summary>
        /// Дали видеото е активно
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Дали видеото е оптимизирано за мобилни устройства
        /// </summary>
        public bool IsMobileOptimized { get; set; } = false;

        /// <summary>
        /// Връзки към документи
        /// </summary>
        public virtual ICollection<VideoDocument>? VideoDocuments { get; set; }
    }
}
