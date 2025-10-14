using Nursan.Domain.SystemClass;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nursan.Domain.Entity
{
    [Nursan.Domain.Table.Table(TableName = "SyTicketName")]
    public class SyTicketName : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string TiketName { get; set; }
        public string Description { get; set; }
        public int? Role { get; set; }
    }
}
