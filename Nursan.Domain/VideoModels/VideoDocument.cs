using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nursan.Domain.VideoModels
{
    /// <summary>
    /// Модел за връзка между видео и документи
    /// </summary>
    [Table("VideoDocuments")]
    public class VideoDocument
    {
        [Key]
        public int Id { get; set; }

        public int VideoId { get; set; }

        public int DocumentId { get; set; }

        // Navigation properties
        public virtual Video? Video { get; set; }
    }
}
