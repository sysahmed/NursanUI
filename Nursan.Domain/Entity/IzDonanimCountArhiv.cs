namespace Nursan.Domain.Entity;

public partial class IzDonanimCountArhiv
{
    public int Id { get; set; }

    public string? DonanimReferans { get; set; }

    public int? IdDonanim { get; set; }

    public string? OrHarnessModel { get; set; }

    public int? AlertNumber { get; set; }

    public int? UrIstasyonId { get; set; }

    public int? MasaId { get; set; }

    public int? MashinId { get; set; }

    public int? VardiyaId { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public bool? Activ { get; set; }

    public bool? Revork { get; set; }

    public virtual UrKonveyorNumara? Masa { get; set; }

    public virtual OpMashin? Mashin { get; set; }

    public virtual UrIstasyon? UrIstasyon { get; set; }

    public virtual UrVardiya? Vardiya { get; set; }
}
