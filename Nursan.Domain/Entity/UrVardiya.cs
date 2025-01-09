using Nursan.Domain.SystemClass;

namespace Nursan.Domain.Entity
{
    [Nursan.Domain.Table.Table(TableName = "UrVardiya")]
    public partial class UrVardiya : BaseEntity
    {
        public UrVardiya()
        {
            IzCoaxCableCounts = new HashSet<IzCoaxCableCount>();
            //  IzDonanimCountArhivs = new HashSet<IzDonanimCountArhiv>();
            IzDonanimCounts = new HashSet<IzDonanimCount>();
            UrIstasyons = new HashSet<UrIstasyon>();
        }
        public string Name { get; set; }

        public virtual ICollection<IzCoaxCableCount> IzCoaxCableCounts { get; set; }
        //public virtual ICollection<IzDonanimCountArhiv> IzDonanimCountArhivs { get; set; }
        public virtual ICollection<UrIstasyon> UrIstasyons { get; set; }
        public virtual ICollection<IzDonanimCount> IzDonanimCounts { get; set; }
    }
}
