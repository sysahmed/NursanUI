namespace Nursan.Domain.AmbarModels;

public partial class ProductToplam
{
    public int Id { get; set; }

    public long ProductId { get; set; }

    public int? IslemId { get; set; }

    public int? Adet { get; set; }

    public virtual Islemler? Islem { get; set; }

    public virtual Product Product { get; set; } = null!;
}
