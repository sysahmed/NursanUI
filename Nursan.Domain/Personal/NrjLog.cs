namespace Nursan.Domain.Personal;

public partial class NrjLog
{
    public long Id { get; set; }

    public long? UserId { get; set; }

    public string? UserCode { get; set; }

    public long? CardId { get; set; }

    public bool? InOut { get; set; }

    public DateTime? Timestamp { get; set; }

    public long ControllerId { get; set; }

    public long XrefId { get; set; }
}
