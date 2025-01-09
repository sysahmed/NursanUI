namespace Nursan.Domain.AmbarModels;

public partial class ProductGroup
{
    public int GroupId { get; set; }

    public string? GroupName { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
