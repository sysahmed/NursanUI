namespace Nursan.Domain.AmbarModels;

public partial class MakineGruplariDahil
{
    public int Id { get; set; }

    public int? GroupName { get; set; }

    public decimal? MakineNumara { get; set; }

    public virtual MakineGruplari? GroupNameNavigation { get; set; }
}
