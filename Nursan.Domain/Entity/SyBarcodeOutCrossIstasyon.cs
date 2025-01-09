using Nursan.Domain.SystemClass;

namespace Nursan.Domain.Entity
{
    [Nursan.Domain.Table.Table(TableName = "SyBarcodeOutCrossIstasyon")]
    public partial class SyBarcodeOutCrossIstasyon : BaseEntity
    {
        public SyBarcodeOutCrossIstasyon()
        {
            SyBarcodeOuts = new HashSet<SyBarcodeOut>();
        }

        public int SysBarcodeOutId { get; set; }
        public int UrIstasyonId { get; set; }
        public virtual UrIstasyon UrIstasyon { get; set; } = null!;
        public virtual ICollection<SyBarcodeOut> SyBarcodeOuts { get; set; }
    }
}
