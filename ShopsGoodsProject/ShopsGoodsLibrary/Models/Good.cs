using System.Windows.Media;

namespace ShopsGoodsLibrary.Models;

public partial class Good
{
    public int Id { get; set; }

    public string NameGood { get; set; } = null!;

    public string DescriptionGood { get; set; } = null!;

    public int GoodPrice { get; set; }

    public string GoodImage { get; set; } = null!;

    public int GoodCount { get; set; }

    public int TypeId { get; set; }

    public int BrandId { get; set; }

    public virtual Brand Brand { get; set; } = null!;

    public virtual ICollection<CartGood> CartGoods { get; set; } = new List<CartGood>();

    public virtual ICollection<OrderGood> OrderGoods { get; set; } = new List<OrderGood>();

    public virtual ICollection<ShopGood> ShopGoods { get; set; } = new List<ShopGood>();

    public virtual Type Type { get; set; } = null!;

    public string GetPrice
    {
        get => $"{GoodPrice}₽";
    }

    public string GetCount
    {
        get => $"В количестве {GoodCount} шт.";
    }

    public SolidColorBrush CountColorBrush
    {
        get
        {
            if (GoodCount > 20)
                return Brushes.Green;
            else if (GoodCount >= 10 && GoodCount <= 20)
                return Brushes.Orange;
            else
                return Brushes.Red;
        }
    }

    public string GetImage
    {
        get => GoodImage;
    }
}
