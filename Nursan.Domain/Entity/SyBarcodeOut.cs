using Nursan.Domain.SystemClass;
using System.ComponentModel.DataAnnotations;

namespace Nursan.Domain.Entity
{
    [Nursan.Domain.Table.Table(TableName = "SyBarcodeOut")]
    public partial class SyBarcodeOut : BaseEntity
    {
        public SyBarcodeOut()
        {
            UrIstasyons = new HashSet<UrIstasyon>();
        }


        public string Name { get; set; } = null!;
        public int? PrinetrId { get; set; }
        public int Leght { get; set; }

        public string? Promenliva { get; set; }
        public string? prefix { get; set; }
        public string? suffix { get; set; }
        public string? PsName { get; set; }
        public string? family { get; set; }
        public string? releace { get; set; }
        public string? eclntcode { get; set; }
        public string? IdDonanim { get; set; }
        public string? Sira1 { get; set; }
        public string? Sira2 { get; set; }
        public string? Sira3 { get; set; }
        [StringLength(1)]
        public string? OzelChar { get; set; }

        public string? BarcodeIcerik { get; set; }
        public string? RegexString { get; set; }
        public string? RegexInt { get; set; }
        public int? PadLeft { get; set; }
        public int? StartingSubstring { get; set; }
        public int? StopingSubstring { get; set; }
        [StringLength(1)]
        public char? ParcalamaChar { get; set; }

        public int? SyBarcodeOutCrossIstasyonId { get; set; }

        public virtual SyPrinter? Prinetr { get; set; } = null!;
        public virtual ICollection<UrIstasyon> UrIstasyons { get; set; }
    }
}
