namespace Nursan.Domain.Personal;

public partial class OperatorVardiya
{
    public int Id { get; set; }

    public string CalismaHat { get; set; } = null!;

    public string Vardiya { get; set; } = null!;

    public int Sicil { get; set; }

    public string UnikVardiya { get; set; } = null!;

    public DateTime Tarih { get; set; }
}
