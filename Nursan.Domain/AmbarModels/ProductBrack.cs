namespace Nursan.Domain.AmbarModels;

public partial class ProductBrack
{
    public long BrackId { get; set; }

    public int? Adet { get; set; }

    public long? BrackProduct { get; set; }

    public decimal BrackKullanici { get; set; }

    public int? BrackOnay { get; set; }

    public bool? BrackEvent { get; set; }

    public DateTime? BrackTarih { get; set; }

    public virtual Product? BrackProductNavigation { get; set; }
}
