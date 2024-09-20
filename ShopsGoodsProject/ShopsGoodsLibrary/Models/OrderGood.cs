namespace ShopsGoodsLibrary.Models;

public partial class OrderGood
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public int GoodId { get; set; }

    public virtual Good Good { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
