using ShopsGoodsLibrary.Models;
using ShopsGoodsProject.Windows.Administrator.Windows.Goods;
using System.Windows;
using System.Windows.Controls;

namespace ShopsGoodsProject.Windows.Administrator.Pages
{
    /// <summary>
    /// Логика взаимодействия для Goods.xaml
    /// </summary>
    public partial class Goods : Page
    {
        public Goods()
        {
            InitializeComponent();
            ShopGridModel.ItemsSource = ShopsGoodsContext.GetContext()
                                                         .Shops
                                                         .OrderBy(x => x.Id)
                                                         .ToList();
        }

        private void SeeGoods_Click(object sender, RoutedEventArgs e)
        {
            var shopId = (sender as Button)?.Tag;
            new SeeGoods(Convert.ToInt32(shopId)).Show();
        }
    }
}
