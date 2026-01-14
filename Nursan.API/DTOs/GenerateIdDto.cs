namespace Nursan.API.DTOs
{
    /// <summary>
    /// DTO за IzGenerateId
    /// </summary>
    public class GenerateIdDto
    {
        public int? Id { get; set; }
        public int? HarnesModelId { get; set; }
        public string? Barcode { get; set; }
        public string? PFBSocket { get; set; }
        public int? ReferasnLeght { get; set; }
        public int? DonanimIdLeght { get; set; }
        public int? DonanimTorkReferansId { get; set; }
        public int? UrIstasyonId { get; set; }
        public int? AlertNumber { get; set; }
        public bool? Revork { get; set; }
    }
}
