using Nursan.Domain.SystemClass;

namespace Nursan.Domain.Entity
{
    [Nursan.Domain.Table.Table(TableName = "IzCoaxCableCount")]
    public partial class IzCoaxCableCount : BaseEntity
    {

        public int? HarnessModelId { get; set; }
        public int? DonanimRederansId { get; set; }
        public string? CoaxCableName { get; set; }
        public int? UrIstasyonId { get; set; }
        public int? OrPcNameId { get; set; }
        public int? VardiyaId { get; set; }
        public long? CoaxTutulanId { get; set; }
        public int? CoaxCableId { get; set; }

        public virtual IzCoaxCableCross? CoaxCable { get; set; }
        public virtual IzGenerateId? DonanimRederans { get; set; }
        public virtual OrHarnessModel? HarnessModel { get; set; }
        public virtual OpMashin? OrPcName { get; set; }
        public virtual UrIstasyon? UrIstasyon { get; set; }
        public virtual UrVardiya? UrVardiyaNavigation { get; set; }
    }
}
