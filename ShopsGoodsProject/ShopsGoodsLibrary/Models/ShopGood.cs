namespace ShopsGoodsLibrary.Models;

public partial class ShopGood
{
    public int Id { get; set; }

    public int ShopId { get; set; }

    public int GoodId { get; set; }

    public virtual Good Good { get; set; } = null!;

    public virtual Shop Shop { get; set; } = null!;
}
