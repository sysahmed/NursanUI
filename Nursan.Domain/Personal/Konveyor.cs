namespace Nursan.Domain.Personal;

public partial class Konveyor
{
    public int Id { get; set; }

    public long? Sicil { get; set; }

    public int? Konveyor1 { get; set; }

    public int? BolgeId { get; set; }

    public long? Padi { get; set; }

    public long? PsoyAdi { get; set; }

    public DateTime? Tarih { get; set; }

    /// <summary>
    /// 0
    /// </summary>
    public bool? Durum { get; set; }

    public int? SicilGun { get; set; }

    public virtual Hat? Konveyor1Navigation { get; set; }
}
