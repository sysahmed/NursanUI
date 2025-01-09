namespace Nursan.Domain.Personal;

public partial class Hat
{
    public int Id { get; set; }

    public string? KonveyorNo { get; set; }

    public string? Hat1 { get; set; }

    public virtual ICollection<Konveyor> Konveyors { get; set; } = new List<Konveyor>();
}
