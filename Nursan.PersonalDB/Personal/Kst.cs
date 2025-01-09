using System;
using System.Collections.Generic;

namespace Nursan.PersonalDB.Personal;

public partial class Kst
{
    public int Id { get; set; }

    public long? Sicil { get; set; }

    public int? KstVardiya { get; set; }

    public int? BolgelerId { get; set; }

    public long? Padi { get; set; }

    public long? PsoyAdi { get; set; }

    public DateTime? Tarih { get; set; }

    public bool? Durum { get; set; }

    public int? SicilGun { get; set; }

    public int? SoftKey { get; set; }
}
