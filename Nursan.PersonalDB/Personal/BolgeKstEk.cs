using System;
using System.Collections.Generic;

namespace Nursan.PersonalDB.Personal;

public partial class BolgeKstEk
{
    public int BolgeId { get; set; }

    public string? BolgeName { get; set; }

    public virtual ICollection<KstEk> KstEks { get; set; } = new List<KstEk>();
}
