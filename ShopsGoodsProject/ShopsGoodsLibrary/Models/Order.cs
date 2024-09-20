namespace ShopsGoodsLibrary.Models;

public partial class Order
{
    public int Id { get; set; }

    public DateTime? OrderDate { get; set; }

    public int OrderPrice { get; set; }

    public int UserId { get; set; }

    public int OrderStatusId { get; set; }

    public virtual ICollection<OrderGood> OrderGoods { get; set; } = new List<OrderGood>();

    public virtual OrderStatus OrderStatus { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
