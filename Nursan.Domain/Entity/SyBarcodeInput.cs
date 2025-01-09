using Nursan.Domain.SystemClass;
using System.ComponentModel.DataAnnotations;

namespace Nursan.Domain.Entity
{
    [Nursan.Domain.Table.Table(TableName = "SyBarcodeInput")]
    public partial class SyBarcodeInput : BaseEntity
    {
        public SyBarcodeInput()
        {
            SyBarcodeInCrossIstasyons = new HashSet<SyBarcodeInCrossIstasyon>();
        }

        public string? Name { get; set; }
        public int? PrinterId { get; set; }
        public int? Leght { get; set; }
        public string? BarcodeIcerik { get; set; }
        public int? SyBarcodeInCrossIstasyonId { get; set; }
        public string? RegexString { get; set; }
        public string? RegexInt { get; set; }
        public int? PadLeft { get; set; }
        public int? StartingSubstring { get; set; }
        public int? StopingSubstring { get; set; }
        [StringLength(1)]
        public char? ParcalamaChar { get; set; }
        [StringLength(1)]
        public string? OzelChar { get; set; }

        public virtual SyPrinter? Printer { get; set; } = null!;
        public virtual ICollection<SyBarcodeInCrossIstasyon> SyBarcodeInCrossIstasyons { get; set; }
    }
}
