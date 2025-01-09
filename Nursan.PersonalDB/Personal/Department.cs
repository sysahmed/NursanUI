using System;
using System.Collections.Generic;

namespace Nursan.PersonalDB.Personal;

public partial class Department
{
    public long DepartmentId { get; set; }

    public string? Name { get; set; }

    public int? Nomer { get; set; }

    public virtual ICollection<Personal> Personals { get; set; } = new List<Personal>();
}
