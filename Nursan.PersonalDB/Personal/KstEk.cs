using System;
using System.Collections.Generic;

namespace Nursan.PersonalDB.Personal;

public partial class KstEk
{
    public int Id { get; set; }

    public long? Sicil { get; set; }

    public int? KstEkVardiya { get; set; }

    public int? BolgelerId { get; set; }

    public long? Padi { get; set; }

    public long? PsoyAdi { get; set; }

    public DateTime? Tarih { get; set; }

    public bool? Durum { get; set; }

    public int? SicilGun { get; set; }

    public virtual BolgeKstEk? Bolgeler { get; set; }

    public virtual Vardiya? KstEkVardiyaNavigation { get; set; }
}
