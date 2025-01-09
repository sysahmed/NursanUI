using System.ComponentModel.DataAnnotations;

namespace Nursan.Domain.AmbarModels;

public partial class Islemler
{
    [Key]
    public int IslemId { get; set; }

    public int? IslemPersonal { get; set; }

    public string? Ariza { get; set; }

    public string? Islem { get; set; }

    public string? Bolge { get; set; }

    public long? ParcaDegisimi { get; set; }

    public decimal? PcId { get; set; }

    public DateTime? Tarih { get; set; }

    public DateTime? BitisTarihi { get; set; }

    public int? Durus { get; set; }

    public int? GirilenDurus { get; set; }

    public bool? Active { get; set; }

    public int? Role { get; set; }

    public int? Sicil { get; set; }

    public string? BakimSicil { get; set; }

    public bool? MailAt { get; set; }

    public bool? WhatsappAt { get; set; }

    public bool? PerodikBakim { get; set; }

    public virtual PcName? Pc { get; set; }

    public virtual ICollection<ProductToplam> ProductToplams { get; set; } = new List<ProductToplam>();
}
