namespace Nursan.Domain.AmbarModels;

public partial class ProductImport
{
    public int Id { get; set; }

    public long ProductId { get; set; }

    public int Adet { get; set; }

    public double Price { get; set; }

    public double TotalPrice { get; set; }

    public DateTime Tarih { get; set; }

    public int Store { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual Store StoreNavigation { get; set; } = null!;
}
