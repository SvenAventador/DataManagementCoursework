using Microsoft.EntityFrameworkCore;
using ShopsGoodsLibrary.Models;
using System.Windows;
using System.Windows.Input;

namespace ShopsGoodsProject.Windows.Client.Windows
{
    /// <summary>
    /// Логика взаимодействия для SeeGoodsInOrder.xaml
    /// </summary>
    public partial class SeeGoodsInOrder : Window
    {
        public SeeGoodsInOrder(int orderId)
        {
            InitializeComponent();

            GoodsGridModel.ItemsSource = ShopsGoodsContext.GetContext()
                                                         .OrderGoods
                                                         .Where(x => x.OrderId == orderId)
                                                         .Include(x => x.Good)
                                                         .ThenInclude(g => g.Type)
                                                         .Include(x => x.Good)
                                                         .ThenInclude(g => g.Brand)
                                                         .Select(x => x.Good)
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
    }
}
