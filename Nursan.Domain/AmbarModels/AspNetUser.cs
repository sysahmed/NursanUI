namespace Nursan.Domain.AmbarModels;

public partial class AspNetUser
{
    public string Id { get; set; } = null!;

    public string? Name { get; set; }

    public string? SurName { get; set; }

    public string? UserName { get; set; }

    public string? NormalizedUserName { get; set; }

    public string? Email { get; set; }

    public string? NormalizedEmail { get; set; }

    public bool EmailConfirmed { get; set; }

    public string? PasswordHash { get; set; }

    public string? SecurityStamp { get; set; }

    public string? ConcurrencyStamp { get; set; }

    public string? PhoneNumber { get; set; }

    public bool PhoneNumberConfirmed { get; set; }

    public bool TwoFactorEnabled { get; set; }

    public DateTimeOffset? LockoutEnd { get; set; }

    public bool LockoutEnabled { get; set; }

    public int AccessFailedCount { get; set; }

    public int? Sicil { get; set; }

    public int? Role { get; set; }

    public bool? MailAt { get; set; }

    public int? IsGonder { get; set; }

    public string? Discriminator { get; set; }

    public virtual ICollection<AspNetUserClaim> AspNetUserClaims { get; set; } = new List<AspNetUserClaim>();

    public virtual ICollection<AspNetUserLogin> AspNetUserLogins { get; set; } = new List<AspNetUserLogin>();

    public virtual ICollection<AspNetUserToken> AspNetUserTokens { get; set; } = new List<AspNetUserToken>();

    public virtual ICollection<Onaylayici> Onaylayicis { get; set; } = new List<Onaylayici>();

    public virtual ICollection<AspNetRole> Roles { get; set; } = new List<AspNetRole>();
}
