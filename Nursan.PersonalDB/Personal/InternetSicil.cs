using System;
using System.Collections.Generic;

namespace Nursan.PersonalDB.Personal;

public partial class InternetSicil
{
    public int Id { get; set; }

    public string? Sigil { get; set; }

    public string? Name { get; set; }

    public string? Surname { get; set; }

    public string? Bdate { get; set; }

    public DateTime? Tarih { get; set; }
}
