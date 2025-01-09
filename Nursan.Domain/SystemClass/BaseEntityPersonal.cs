using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nursan.Domain.SystemClass
{
    public abstract class BaseEntityPersonal : IEntity
    {
        public BaseEntityPersonal()
        {
        }
        [ForeignKey("Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }

    }
}
