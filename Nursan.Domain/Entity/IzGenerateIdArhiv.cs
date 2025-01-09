namespace Nursan.Domain.Entity;

public partial class IzGenerateIdArhiv
{
    public int Id { get; set; }

    public int? HarnesModelId { get; set; }

    public string? Barcode { get; set; }

    public string? Pfbsocket { get; set; }

    public int? ReferasnLeght { get; set; }

    public int? DonanimIdLeght { get; set; }

    public int? DonanimTorkReferansId { get; set; }

    public int? UrIstasyonId { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public int? AlertNumber { get; set; }

    public bool? Activ { get; set; }
    public bool? Revork { get; set; }
}
