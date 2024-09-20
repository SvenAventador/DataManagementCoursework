using ShopsGoodsLibrary.Models;
using System.Windows.Controls;

namespace ShopsGoodsProject.Windows.Manager.Pages
{
    /// <summary>
    /// Логика взаимодействия для Shops.xaml
    /// </summary>
    public partial class Shops : Page
    {
        public Shops()
        {
            InitializeComponent();
            ShopGridModel.ItemsSource = ShopsGoodsContext.GetContext()
                                                         .Shops
                                                         .OrderBy(x => x.Id)
                                                         .ToList();
        }
    }
}
