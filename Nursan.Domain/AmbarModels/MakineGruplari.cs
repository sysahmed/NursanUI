namespace Nursan.Domain.AmbarModels;

public partial class MakineGruplari
{
    public int Id { get; set; }

    public string? GroupName { get; set; }

    public virtual ICollection<MakineGruplariDahil> MakineGruplariDahils { get; set; } = new List<MakineGruplariDahil>();
}
