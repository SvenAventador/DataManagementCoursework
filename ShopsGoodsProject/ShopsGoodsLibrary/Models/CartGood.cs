namespace ShopsGoodsLibrary.Models;

public partial class CartGood
{
    public int Id { get; set; }

    public int CartId { get; set; }

    public int GoodId { get; set; }

    public int? GoodAmount { get; set; }

    public virtual Cart Cart { get; set; } = null!;

    public virtual Good Good { get; set; } = null!;
}
