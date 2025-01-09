using System;
using System.Collections.Generic;

namespace Nursan.Domain.NursanBarcode;

public partial class SiraNo
{
    public int Id { get; set; }

    public short SiraNo1 { get; set; }

    public string ProjeKodu { get; set; } = null!;
}
