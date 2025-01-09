using System;
using System.Collections.Generic;

namespace Nursan.Domain.NursanBarcode;

public partial class KaseUser
{
    public int Model { get; set; }

    public string? Username { get; set; }

    public DateTime? Datet { get; set; }

    public string? KasenoOld { get; set; }

    public string? KaseNew { get; set; }
}
