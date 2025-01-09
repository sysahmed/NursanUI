using System;
using System.Collections.Generic;

namespace Nursan.Domain.NursanBarcode;

public partial class Yazici
{
    public int Id { get; set; }

    public string Ip { get; set; } = null!;

    public byte Yetki { get; set; }

    public string KullaniciAdi { get; set; } = null!;

    public string? OrtamKitapligi { get; set; }

    public string? OrtamDosyasi { get; set; }

    public virtual ICollection<YaziciKuyrugu> YaziciKuyrugus { get; set; } = new List<YaziciKuyrugu>();
}
