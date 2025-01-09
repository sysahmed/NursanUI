using System;
using System.Collections.Generic;

namespace Nursan.Domain.NursanBarcode;

public partial class Aile
{
    public int Id { get; set; }

    public string Ad { get; set; } = null!;

    public bool ToplamNewKasa { get; set; }
}
