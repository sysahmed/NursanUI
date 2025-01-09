namespace Nursan.Domain.AmbarModels;

public partial class Onaylayici
{
    public int OnaylayiciId { get; set; }

    public string? OnaylayiciIsim { get; set; }

    public string? UserId { get; set; }

    public int? NormalId { get; set; }

    public virtual ICollection<Onay> Onays { get; set; } = new List<Onay>();

    public virtual AspNetUser? User { get; set; }
}
