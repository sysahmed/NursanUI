namespace Nursan.Domain.Personal;

public partial class Controller
{
    public long ControllerId { get; set; }

    public string? Ip { get; set; }

    public string? Description { get; set; }

    public bool? Rfid1Worktime { get; set; }

    public bool? Rfid2Worktime { get; set; }
}
