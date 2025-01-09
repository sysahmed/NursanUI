using Nursan.Domain.SystemClass;

namespace Nursan.Domain.Entity
{
    [Nursan.Domain.Table.Table(TableName = "SyBarcodeInCrossIstasyon")]
    public partial class SyBarcodeInCrossIstasyon : BaseEntity
    {
        public SyBarcodeInCrossIstasyon()
        {

        }

        public int? SysBarcodeInId { get; set; }
        public int? UrIstasyonId { get; set; }

        public virtual SyBarcodeInput? SysBarcodeIn { get; set; }
        public virtual UrIstasyon? UrIstasyon { get; set; }
    }
}
