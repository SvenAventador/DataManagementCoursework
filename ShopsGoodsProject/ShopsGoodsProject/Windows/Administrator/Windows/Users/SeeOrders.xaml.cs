using Microsoft.EntityFrameworkCore;
using ShopsGoodsLibrary.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ShopsGoodsProject.Windows.Administrator.Windows.Users
{
    /// <summary>
    /// Логика взаимодействия для SeeOrders.xaml
    /// </summary>
    public partial class SeeOrders : Window
    {
        public List<OrderStatus> OrderStatuses { get; set; }
        public SeeOrders(int userId)
        {
            InitializeComponent();

            OrdersGridModel.ItemsSource = ShopsGoodsContext.GetContext()
                                                           .Orders
                                                           .Where(x => x.UserId == userId)
                                                           .Include(x => x.OrderStatus)
                                                           .OrderBy(x => x.Id)
                                                           .ToList();
            OrderStatuses = [.. ShopsGoodsContext.GetContext().OrderStatuses.OrderBy(x => x.Id)];

            DataContext = this;
        }

        private void ComboBox_DropDownClosed(object sender, EventArgs e)
        {
            var comboBox = sender as ComboBox;

            if (comboBox!.SelectedValue != null)
            {
                var selectedOrderStatusId = (int)comboBox.SelectedValue;
                var order = (Order)((FrameworkElement)sender).DataContext;

                order.OrderStatusId = selectedOrderStatusId;

                var context = ShopsGoodsContext.GetContext();
                context.SaveChanges();

                MessageBox.Show($"Статус заказа изменен на {order.OrderStatus.StatusName}", 
                                "Внимание!",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
            }
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
