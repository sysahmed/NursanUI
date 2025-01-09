using System;
using System.Collections.Generic;

namespace Nursan.Domain.NursanBarcode;

public partial class YaziciKuyrugu
{
    public int Id { get; set; }

    public int FkYaziciid { get; set; }

    public int FkEtiketArsivid { get; set; }

    public bool? Yazdirildi { get; set; }

    public bool? As400akt { get; set; }

    public virtual EtiketArsiv FkEtiketArsiv { get; set; } = null!;

    public virtual Yazici FkYazici { get; set; } = null!;
}
