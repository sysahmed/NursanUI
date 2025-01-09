namespace Nursan.Domain.Personal;

public partial class Scaflar
{
    public int Id { get; set; }

    public int? Nomer { get; set; }

    public string? Sicil { get; set; }

    public int? Locasyon { get; set; }

    public DateTime? VerilmeTarih { get; set; }

    public bool? Verik { get; set; }

    public virtual Locasyon? LocasyonNavigation { get; set; }
}
