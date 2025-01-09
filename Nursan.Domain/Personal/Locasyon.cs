namespace Nursan.Domain.Personal;

public partial class Locasyon
{
    public int Id { get; set; }

    public string? Locasyon1 { get; set; }

    public virtual ICollection<Scaflar> Scaflars { get; set; } = new List<Scaflar>();
}
