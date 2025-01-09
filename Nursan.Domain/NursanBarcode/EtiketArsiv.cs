using System;
using System.Collections.Generic;

namespace Nursan.Domain.NursanBarcode;

public partial class EtiketArsiv
{
    public int Id { get; set; }

    public string Alckodu { get; set; } = null!;

    public DateTime Tarih { get; set; }

    public bool Yazdirildi { get; set; }

    public string SeriNo { get; set; } = null!;

    public string MalzemeKodu { get; set; } = null!;

    public string PartName { get; set; } = null!;

    public string Model { get; set; } = null!;

    public string SupplyArea { get; set; } = null!;

    public string BinNo { get; set; } = null!;

    public string SigortaNo { get; set; } = null!;

    public short Miktar { get; set; }

    public string UserId { get; set; } = null!;

    public string SiraNo { get; set; } = null!;

    public string Aciklama { get; set; } = null!;

    public bool As400akt { get; set; }

    public DateTime As400aktan { get; set; }

    public DateTime YazdirildiAn { get; set; }

    public virtual ICollection<Koliicidetay> Koliicidetays { get; set; } = new List<Koliicidetay>();

    public virtual ICollection<YaziciKuyrugu> YaziciKuyrugus { get; set; } = new List<YaziciKuyrugu>();
}
