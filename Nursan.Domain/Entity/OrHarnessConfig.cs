using Nursan.Domain.SystemClass;

namespace Nursan.Domain.Entity
{
    [Nursan.Domain.Table.Table(TableName = "OrHarnessConfig")]
    public partial class OrHarnessConfig : BaseEntity
    {
        public string? ConfigTork { get; set; }
        public int? OrHarnessModelId { get; set; }
        public string PFBSocket { get; set; }

        public virtual OrHarnessModel? OrHarnessModel { get; set; }
    }
}
