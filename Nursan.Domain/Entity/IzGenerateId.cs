using Nursan.Domain.SystemClass;

namespace Nursan.Domain.Entity
{
    [Nursan.Domain.Table.Table(TableName = "IzGenerateId")]
    public partial class IzGenerateId : BaseEntity
    {
        public IzGenerateId()
        {
            IzCoaxCableCounts = new HashSet<IzCoaxCableCount>();
            IzDonanimCounts = new HashSet<IzDonanimCount>();
        }
        public int? HarnesModelId { get; set; }
        public string? Barcode { get; set; }
        public string? PFBSocket { get; set; }
        public int? ReferasnLeght { get; set; }
        public int? DonanimIdLeght { get; set; }
        public int? DonanimTorkReferansId { get; set; }
        public int? UrIstasyonId { get; set; }
        public int? AlertNumber { get; set; }
        public bool? Revork { get; set; }

        public virtual OrHarnessModel? HarnesModel { get; set; }
        public virtual UrIstasyon? UrIstasyon { get; set; }
        public virtual ICollection<IzCoaxCableCount> IzCoaxCableCounts { get; set; }
        public virtual ICollection<IzDonanimCount> IzDonanimCounts { get; set; }
    }
}
