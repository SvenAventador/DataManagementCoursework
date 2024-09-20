namespace ShopsGoodsLibrary.Models;

public partial class Cart
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public virtual ICollection<CartGood> CartGoods { get; set; } = new List<CartGood>();

    public virtual User User { get; set; } = null!;
}
