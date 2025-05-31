using Nursan.Domain.SystemClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nursan.Domain.Entity
{
    [Nursan.Domain.Table.Table(TableName = "OrAlertGk", IdentityColumn = "Id")]
    public partial class OrAlertGk:BaseEntity
    {
        public int Id { get; set; }
        public int AlertNumber { get; set; }
        public DateTime Tarih { get; set; }

    }
}
