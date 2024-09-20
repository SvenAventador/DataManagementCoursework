namespace ShopsGoodsLibrary.Models;

public partial class Shop
{
    public int Id { get; set; }

    public string ShopName { get; set; } = null!;

    public string ShopAddress { get; set; } = null!;

    public string ShopPhone { get; set; } = null!;

    public virtual ICollection<ShopGood> ShopGoods { get; set; } = new List<ShopGood>();
}
