using Nursan.Domain.SystemClass;

namespace Nursan.Domain.Entity
{
    [Nursan.Domain.Table.Table(TableName = "IzDonanimHedef")]
    public partial class IzDonanimHedef : BaseEntity
    {
        public int Id { get; set; }
        public int? HarnesModelId { get; set; }
        public int? Adet { get; set; }
        public int? Hedef { get; set; }
        public int? IstasyonId { get; set; }
        public int? FamilyId { get; set; }
        public virtual UrIstasyon? Istasuon { get; set; }
        public virtual OrFamily? OrFamily { get; set; }
        public virtual OrHarnessModel? OrHarnessModel { get; set; }
    }
}
