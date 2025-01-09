namespace Nursan.Domain.AmbarModels;

public partial class Aktarim
{
    public long AktarimId { get; set; }

    public long ProductId { get; set; }

    public int? StoreId { get; set; }

    public decimal KullaniciId { get; set; }

    public int? Adet { get; set; }

    public DateTime? AktarimTarih { get; set; }

    public decimal? IslemId { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual Store? Store { get; set; }
}
