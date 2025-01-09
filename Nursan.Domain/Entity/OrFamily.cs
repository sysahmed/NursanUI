using Nursan.Domain.SystemClass;

namespace Nursan.Domain.Entity
{
    [Nursan.Domain.Table.Table(TableName = "OrFamily")]
    public partial class OrFamily : BaseEntity
    {
        public OrFamily()
        {
            OrHarnessModels = new HashSet<OrHarnessModel>();
            UrIstasyons = new HashSet<UrIstasyon>();
            IzDonanimHedefs = new HashSet<IzDonanimHedef>();
        }

        public string FamilyName { get; set; }

        public virtual ICollection<OrHarnessModel> OrHarnessModels { get; set; }
        public virtual ICollection<UrIstasyon> UrIstasyons { get; set; }
        public virtual ICollection<IzDonanimHedef> IzDonanimHedefs { get; set; }
    }
}
