using System;
using System.Collections.Generic;

namespace Nursan.Domain.NursanBarcode;

public partial class Koliicidetay
{
    public int Id { get; set; }

    public int FkEtiketArsivid { get; set; }

    public DateTime An { get; set; }

    public string TestSeriNo { get; set; } = null!;

    public virtual EtiketArsiv FkEtiketArsiv { get; set; } = null!;
}
