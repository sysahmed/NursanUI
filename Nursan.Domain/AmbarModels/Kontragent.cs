namespace Nursan.Domain.AmbarModels;

public partial class Kontragent
{
    public int Id { get; set; }

    public string? KontragentName { get; set; }

    public string? Bgid { get; set; }

    public virtual ICollection<FakturaBilgeri> FakturaBilgeris { get; set; } = new List<FakturaBilgeri>();

    public virtual ICollection<Onay> Onays { get; set; } = new List<Onay>();
}
