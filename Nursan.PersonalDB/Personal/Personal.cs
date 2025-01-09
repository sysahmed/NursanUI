using System;
using System.Collections.Generic;

namespace Nursan.PersonalDB.Personal;

public partial class Personal
{
    public long Id { get; set; }

    public long? UserId { get; set; }

    public string? UserCode { get; set; }

    public string? FirstName { get; set; }

    public string? SurName { get; set; }

    public string? LastName { get; set; }

    public long? CardId { get; set; }

    public long Department { get; set; }

    public long JobPosition { get; set; }

    public bool? LastDir { get; set; }

    public DateTime? DirChange { get; set; }

    public virtual Department DepartmentNavigation { get; set; } = null!;
}
