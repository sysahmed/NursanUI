namespace Nursan.Domain.AmbarModels;

public partial class Store
{
    public int StoreId { get; set; }

    public string StoreName { get; set; } = null!;

    public int StoreAmbar { get; set; }

    public int? Adet { get; set; }

    public virtual ICollection<Aktarim> Aktarims { get; set; } = new List<Aktarim>();

    public virtual ICollection<Onay> Onays { get; set; } = new List<Onay>();

    public virtual ICollection<ProductImport> ProductImports { get; set; } = new List<ProductImport>();
}
