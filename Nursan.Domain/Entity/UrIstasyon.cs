namespace Nursan.Domain.Entity;



[Nursan.Domain.Table.Table(TableName = "UrIstasyon")]
public partial class UrIstasyon
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int? ModulerYapiId { get; set; }

    public short? FabrikaId { get; set; }

    public int? MashinId { get; set; }

    public int? VardiyaId { get; set; }

    public int? Toplam { get; set; }

    public string? Calismasati { get; set; }

    public string? Durus { get; set; }

    public int? FamilyId { get; set; }

    public int? Hedef { get; set; }

    public decimal? Orta { get; set; }

    public int? Realadet { get; set; }

    public int? Sayi { get; set; }

    public int? Sayicarp { get; set; }

    public bool? Sifirla { get; set; }

    public DateTime? Sonokuma { get; set; }

    public int? SyBarcodeOutId { get; set; }

    public string? UnikId { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public bool? Activ { get; set; }

    public virtual OrFamily? Family { get; set; }

    public virtual ICollection<IzCoaxCableCount> IzCoaxCableCounts { get; set; } = new List<IzCoaxCableCount>();

    public virtual ICollection<IzDonanimCountArhiv> IzDonanimCountArhivs { get; set; } = new List<IzDonanimCountArhiv>();

    public virtual ICollection<IzDonanimCount> IzDonanimCounts { get; set; } = new List<IzDonanimCount>();

    public virtual ICollection<IzDonanimHedef> IzDonanimHedefs { get; set; } = new List<IzDonanimHedef>();

    public virtual ICollection<IzGenerateId> IzGenerateIds { get; set; } = new List<IzGenerateId>();

    public virtual OpMashin? Mashin { get; set; }

    public virtual UrModulerYapi? ModulerYapi { get; set; }

    public virtual ICollection<SyBarcodeInCrossIstasyon> SyBarcodeInCrossIstasyons { get; set; } = new List<SyBarcodeInCrossIstasyon>();

    public virtual SyBarcodeOut? SyBarcodeOut { get; set; }

    public virtual UrVardiya? Vardiya { get; set; }
}
