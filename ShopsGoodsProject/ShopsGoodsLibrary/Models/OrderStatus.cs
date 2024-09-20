using System.Windows.Media;

namespace ShopsGoodsLibrary.Models;

public partial class OrderStatus
{
    public int Id { get; set; }

    public string StatusName { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public SolidColorBrush StatusColorBrush
    {
        get
        {
            return Id switch
            {
                1 => Brushes.Red,
                2 => Brushes.Blue,
                3 => Brushes.Orange,
                4 => Brushes.DarkMagenta,
                5 => Brushes.Green,
                6 => Brushes.LightGreen,
                _ => Brushes.Transparent 
            };
        }
    }
}
