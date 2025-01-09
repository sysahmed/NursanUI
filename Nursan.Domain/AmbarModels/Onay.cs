namespace Nursan.Domain.AmbarModels;

public partial class Onay
{
    public int OnayIid { get; set; }

    public decimal PcId { get; set; }

    public int? StoreId { get; set; }

    public long? ProductId { get; set; }

    public int? OnaylayiciId { get; set; }

    public int? Adet { get; set; }

    public bool? Onayladi { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public long? FaturaId { get; set; }

    public int? KontragentId { get; set; }

    public bool? Activ { get; set; }

    public virtual Faktura? Fatura { get; set; }

    public virtual Kontragent? Kontragent { get; set; }

    public virtual Onaylayici? Onaylayici { get; set; }

    public virtual PcName Pc { get; set; } = null!;

    public virtual Product? Product { get; set; }

    public virtual Store? Store { get; set; }
}
