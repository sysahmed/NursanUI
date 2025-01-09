namespace Nursan.Domain.AmbarModels;

public partial class FakturaBilgeri
{
    public long Id { get; set; }

    public string DoId { get; set; } = null!;

    public string Bgid { get; set; } = null!;

    public int KontragentId { get; set; }

    public DateTime DataCreate { get; set; }

    public int Adet { get; set; }

    public decimal? Price { get; set; }

    public decimal? TotalPrice { get; set; }

    public virtual ICollection<Faktura> Fakturas { get; set; } = new List<Faktura>();

    public virtual Kontragent Kontragent { get; set; } = null!;
}
