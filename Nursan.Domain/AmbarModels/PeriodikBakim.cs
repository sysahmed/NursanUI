namespace Nursan.Domain.AmbarModels;

public partial class PeriodikBakim
{
    public int Id { get; set; }

    public decimal? Makine { get; set; }

    public int MakineGrubu { get; set; }

    public string Islem { get; set; } = null!;

    public DateTime? PeriodikBakimTarih { get; set; }

    public DateTime? PeriodikBakimKapanisTarih { get; set; }

    public bool? YapilanBakim { get; set; }

    public int? Ekip { get; set; }
}
