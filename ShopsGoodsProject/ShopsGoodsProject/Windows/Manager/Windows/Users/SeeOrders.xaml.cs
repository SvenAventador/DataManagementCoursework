using ShopsGoodsLibrary.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ShopsGoodsProject.Windows.Manager.Windows.Users
{
    /// <summary>
    /// Логика взаимодействия для SeeOrders.xaml
    /// </summary>
    public partial class SeeOrders : Window
    {
        public SeeOrders(int userId)
        {
            InitializeComponent();
            OrdersGridModel.ItemsSource = ShopsGoodsContext.GetContext()
                                                           .Orders
                                                           .Where(x => x.UserId == userId)
                                                           .OrderBy(x => x.Id)
                                                           .ToList();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SeeGoods_Click(object sender, RoutedEventArgs e)
        {
            var orderId = (sender as Button)?.Tag;
            new SeeGoods(Convert.ToInt32(orderId)).Show();
        }
    }
}
