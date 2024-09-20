using Microsoft.EntityFrameworkCore;
using ShopsGoodsLibrary.Models;
using ShopsGoodsProject.Windows.Client.Windows;
using System.Windows;
using System.Windows.Controls;

namespace ShopsGoodsProject.Windows.Client.Pages
{
    /// <summary>
    /// Логика взаимодействия для Orders.xaml
    /// </summary>
    public partial class Orders : Page
    {
        public Orders()
        {
            InitializeComponent();
            OrdersGridModel.ItemsSource = ShopsGoodsContext.GetContext()
                                                           .Orders
                                                           .Where(x => x.UserId == ShopsGoodsLibrary.Functional.Manager.CurrentUser!.Id)
                                                           .Include(x => x.OrderStatus)
                                                           .OrderBy(x => x.Id)
                                                           .ToList();
        }

        private void SeeGoods_Click(object sender, RoutedEventArgs e)
        {
            new SeeGoodsInOrder(Convert.ToInt32((sender as Button)?.Tag)).Show();
        }
    }
}
