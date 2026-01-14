namespace Nursan.API.DTOs
{
    /// <summary>
    /// DTO за IzDonanimCount
    /// </summary>
    public class DonanimCountDto
    {
        public int? Id { get; set; }
        public string? DonanimReferans { get; set; }
        public int IdDonanim { get; set; }
        public string? OrHarnessModel { get; set; }
        public int? AlertNumber { get; set; }
        public int UrIstasyonId { get; set; }
        public int? MasaId { get; set; }
        public int? MashinId { get; set; }
        public int VardiyaId { get; set; }
        public bool? Revork { get; set; }
    }
}
