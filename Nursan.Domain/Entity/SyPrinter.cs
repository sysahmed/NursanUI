using Nursan.Domain.SystemClass;

namespace Nursan.Domain.Entity
{
    [Nursan.Domain.Table.Table(TableName = "SyPrinter")]
    public partial class SyPrinter : BaseEntity
    {
        public SyPrinter()
        {
            SyBarcodeInputs = new HashSet<SyBarcodeInput>();
            SyBarcodeOuts = new HashSet<SyBarcodeOut>();
        }
        public string? Name { get; set; }
        public string? Ip { get; set; }
        public string? PrintngFile { get; set; }
        public string? Interface { get; set; }
        public virtual ICollection<SyBarcodeInput> SyBarcodeInputs { get; set; }
        public virtual ICollection<SyBarcodeOut> SyBarcodeOuts { get; set; }
    }
}
