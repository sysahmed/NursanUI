namespace Nursan.Domain.Personal;

public partial class KonveyorPersonal
{
    public int Id { get; set; }

    public long? Sicil { get; set; }

    public int? Konveyor { get; set; }

    public int? BolgeId { get; set; }

    public long? Padi { get; set; }

    public long? PsoyAdi { get; set; }

    public DateTime? Tarih { get; set; }

    public bool? Durum { get; set; }

    public int? SicilGun { get; set; }
}
