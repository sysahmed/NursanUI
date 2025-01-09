namespace Nursan.Domain.AmbarModels;

public partial class TalepFormu
{
    public int Id { get; set; }

    public int? TalepGroupId { get; set; }

    public string? Talep { get; set; }

    public string? TalepGenel { get; set; }

    public int? UserId { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public bool? IsActiv { get; set; }
}
