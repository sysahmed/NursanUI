using System;
using System.Collections.Generic;

namespace Nursan.PersonalDB.Personal;

public partial class Vardiya
{
    public int Id { get; set; }

    public string? Vardiya1 { get; set; }

    public virtual ICollection<KstEk> KstEks { get; set; } = new List<KstEk>();
}
