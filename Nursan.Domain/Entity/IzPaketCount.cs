using Nursan.Domain.SystemClass;

namespace Nursan.Domain.Entity
{
    [Nursan.Domain.Table.Table(TableName = "IzPaketCount", IdentityColumn = "Id")]
    public class IzPaketCount : BaseEntity
    {
        public string DonanimReferans { get; set; }
        public int? AlertNumber { get; set; }
        public int? UrIstasyonId { get; set; }
        public int? MachinId { get; set; }
        public int? VardiyaId { get; set; }
        public string? Koli { get; set; }
        public DateTime? KoliCreateDate { get; set; }
        public string? KasaSerialNo { get; set; }
        public DateTime? KasaCreateDate { get; set; }

    }
}
