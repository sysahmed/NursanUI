namespace Nursan.Domain.AmbarModels;

public partial class Product
{
    public long ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public string? ProductBarcode { get; set; }

    public int? ProductGroup { get; set; }

    public int? Adet { get; set; }

    public DateTime? ProductTarih { get; set; }

    public int? ProductStore { get; set; }

    public decimal ProductKullanan { get; set; }

    public int? ProductAdress { get; set; }

    public decimal? Price { get; set; }

    public int? KritikAdet { get; set; }

    public bool? KritikParca { get; set; }

    public virtual ICollection<Aktarim> Aktarims { get; set; } = new List<Aktarim>();

    public virtual ICollection<Faktura> Fakturas { get; set; } = new List<Faktura>();

    public virtual ICollection<Onay> Onays { get; set; } = new List<Onay>();

    public virtual ICollection<ProductBrack> ProductBracks { get; set; } = new List<ProductBrack>();

    public virtual ProductGroup? ProductGroupNavigation { get; set; }

    public virtual ICollection<ProductImport> ProductImports { get; set; } = new List<ProductImport>();

    public virtual ICollection<ProductToplam> ProductToplams { get; set; } = new List<ProductToplam>();
}
