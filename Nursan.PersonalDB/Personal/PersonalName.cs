using System;
using System.Collections.Generic;

namespace Nursan.PersonalDB.Personal;

public partial class PersonalName
{
    public int Id { get; set; }

    public int SicilId { get; set; }

    public string Adi { get; set; } = null!;

    public string SoyAdi { get; set; } = null!;

    public string? SicilGelen { get; set; }

    public bool? Icerde { get; set; }

    public DateTime? Tarih { get; set; }

    public DateTime? GirisAn { get; set; }

    public DateTime? CikisAn { get; set; }

    public int? Saat { get; set; }
}
