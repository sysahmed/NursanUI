using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nursan.Domain.AmbarModels;

public partial class PcName
{
    [Key]
    public decimal Pcid { get; set; }

    [Column("PCNAME")]
    public string? Pcname1 { get; set; }

    public string? Pcipadress { get; set; }

    public string? Pcmak { get; set; }

    public DateTime? Tarih { get; set; }

    public int? Grupa { get; set; }

    public string? PerosonalIme { get; set; }

    public string? PerosnalOtdel { get; set; }

    public bool? Windowslicens { get; set; }

    public bool? Officelicens { get; set; }

    public bool? Antivirus { get; set; }

    public virtual ICollection<Islemler> Islemlers { get; set; } = new List<Islemler>();

    public virtual ICollection<Onay> Onays { get; set; } = new List<Onay>();
}
