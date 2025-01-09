using Nursan.Domain.SystemClass;

namespace Nursan.Domain.Entity
{
    [Nursan.Domain.Table.Table(TableName = "ErTestHatalari")]
    public partial class ErTestHatalari : BaseEntity
    {
        public int? IdDonanim { get; set; }
        public string? Referans { get; set; }
        public string? Konveyor { get; set; }
        public DateTime? KonVeyorTarih { get; set; }
        public string? Vardiya { get; set; }
        public string? Bolge1 { get; set; }
        public string? Bolge2 { get; set; }
        public string? Onarma { get; set; }
        public int? HataCodu { get; set; }
        public string? Goz1 { get; set; }
        public string? Goz2 { get; set; }
        public string? Operator { get; set; }
        public int? SicilNo { get; set; }
        public int? Gosterge { get; set; }

    }
}
