namespace Nursan.Domain.Personal;

public partial class Arkaplan
{
    public int Id { get; set; }

    public long? Sicil { get; set; }

    public int? ArkaplanVardiya { get; set; }

    public int? BolgelerId { get; set; }

    public long? Padi { get; set; }

    public long? PsoyAdi { get; set; }

    public DateTime? Tarih { get; set; }

    public bool? Durum { get; set; }

    public int? SicilGun { get; set; }
}
