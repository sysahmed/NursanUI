using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nursan.Domain.SystemClass
{
    [Nursan.Domain.Table.Table(TableName = "BaseEntity", PrimaryColumn = "Id")]
    public abstract class BaseEntity : IEntity
    {
        public BaseEntity()
        {
        }
        [ForeignKey("Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool? Activ { get; set; }
    }
}