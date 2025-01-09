namespace Nursan.Domain.Entity;

public partial class UrFabrika
{
    public short Id { get; set; }

    public string? Fabrika { get; set; }

    public string? Locasion { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }
}
