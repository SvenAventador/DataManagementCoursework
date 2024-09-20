namespace ShopsGoodsLibrary.Models;

public partial class Brand
{
    public int Id { get; set; }

    public string BrandName { get; set; } = null!;

    public virtual ICollection<Good> Goods { get; set; } = new List<Good>();
}
