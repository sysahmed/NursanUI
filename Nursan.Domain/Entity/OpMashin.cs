using Nursan.Domain.SystemClass;

namespace Nursan.Domain.Entity
{
    [Nursan.Domain.Table.Table(TableName = "OpMashin")]
    public partial class OpMashin : BaseEntity
    {
        public OpMashin()
        {
            UrIstasyons = new HashSet<UrIstasyon>();
            IzDonanimCounts = new HashSet<IzDonanimCount>();
            IzCoaxCableCounts = new HashSet<IzCoaxCableCount>();
        }


        public string? MasineName { get; set; }
        public string? IpAddress { get; set; }
        public string? OperationSystems { get; set; }

        public virtual ICollection<IzCoaxCableCount> IzCoaxCableCounts { get; set; }
        public virtual ICollection<UrIstasyon> UrIstasyons { get; set; }
        public virtual ICollection<IzDonanimCount> IzDonanimCounts { get; set; }
    }
}
