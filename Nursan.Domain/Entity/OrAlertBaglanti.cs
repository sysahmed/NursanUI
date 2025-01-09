using Nursan.Domain.SystemClass;

namespace Nursan.Domain.Entity
{
    [Nursan.Domain.Table.Table(TableName = "OrAlertBaglanti")]
    public partial class OrAlertBaglanti : BaseEntity
    {

        public int AlertId { get; set; }
        public int HarnessId { get; set; }
        public int? AlertNumber { get; set; }
        public bool? AlertAccess { get; set; }

        public virtual OrAlert? Alert { get; set; }
        public virtual OrHarnessModel? Harness { get; set; }
    }
}
