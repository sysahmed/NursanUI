using System;
using System.Collections.Generic;

namespace Nursan.PersonalDB.Personal;

public partial class Faktura
{
    public long Id { get; set; }

    public string DoId { get; set; } = null!;

    public decimal Bgid { get; set; }

    public int Kontargent { get; set; }

    public DateTime DataCreate { get; set; }

    public long ProductId { get; set; }

    public int Adet { get; set; }
}
