namespace Nursan.Domain.Entity;
[Nursan.Domain.Table.Table(TableName = "UrPersonalTakib", IdentityColumn = "Id", PrimaryColumn = "DayOfYear")]
public partial class UrPersonalTakib
{
    public int Id { get; set; }

    public string? Sicil { get; set; }

    public string? FullName { get; set; }
    public string? DayOfYear { get; set; }

    public long? Department { get; set; }

    public int? UrIstasyonId { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }
}
