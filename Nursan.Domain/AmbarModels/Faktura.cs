namespace Nursan.Domain.AmbarModels;

public partial class Faktura
{
    public long Id { get; set; }

    public long? FaturaBilgileriId { get; set; }

    public DateTime DataCreate { get; set; }

    public long ProductId { get; set; }

    public int Adet { get; set; }

    public decimal? Price { get; set; }

    public decimal? TotalPrice { get; set; }

    public virtual FakturaBilgeri? FaturaBilgileri { get; set; }

    public virtual ICollection<Onay> Onays { get; set; } = new List<Onay>();

    public virtual Product Product { get; set; } = null!;
}
