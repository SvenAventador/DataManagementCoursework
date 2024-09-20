using Microsoft.EntityFrameworkCore;
using ShopsGoodsLibrary.Models;
using ShopsGoodsProject.Windows.Administrator.Pages;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ShopsGoodsProject.Windows.Administrator.Windows.Goods
{
    /// <summary>
    /// Логика взаимодействия для SeeGoods.xaml
    /// </summary>
    public partial class SeeGoods : Window
    {
        public ObservableCollection<Good> GoodsInCurrentShop { get; set; } = [];
        private int _shopId;
        public SeeGoods(int shopId)
        {
            InitializeComponent();
            _shopId = shopId;
            var currentGoods = ShopsGoodsContext.GetContext()
                                                .ShopGoods
                                                .Include(x => x.Good.Type)  
                                                .Include(x => x.Good.Brand) 
                                                .Where(x => x.ShopId == shopId)
                                                .Where(x => x.Good.GoodCount > 0)
                                                .Select(x => x.Good)     
                                                .ToList();

            GoodsInCurrentShop = new ObservableCollection<Good>(currentGoods);
            GoodsGridModel.ItemsSource = GoodsInCurrentShop;
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

        private void CreateObject_Click(object sender, RoutedEventArgs e)
        {
            new AddAndEditGoods(null!, _shopId).Show();
        }

        private void RefreshObject_Click(object sender, RoutedEventArgs e)
        {
            var currentGoods = ShopsGoodsContext.GetContext()
                                                .ShopGoods
                                                .Where(x => x.ShopId == _shopId)
                                                .Select(x => x.Good)
                                                .ToList();
            GoodsInCurrentShop = new ObservableCollection<Good>(currentGoods);
            GoodsGridModel.ItemsSource = GoodsInCurrentShop;
        }

        private void EditObject_Click(object sender, RoutedEventArgs e)
        {
            new AddAndEditGoods((sender as Button)?.DataContext as Good, int.MinValue).Show();
        }

        private void DeleteObject_Click(object sender, RoutedEventArgs e)
        {
            var deleteElements = GoodsGridModel.SelectedItems.Cast<Good>().ToList();
            if (MessageBox.Show($"Вы уверены, что хотите удалить {deleteElements.Count} элемента(-ов)?",
                                 "Внимание!",
                                 MessageBoxButton.YesNo,
                                 MessageBoxImage.Question) != MessageBoxResult.Yes)
                return;

            ShopsGoodsContext.GetContext()
                             .Goods
                             .RemoveRange(deleteElements);
            ShopsGoodsContext.GetContext().SaveChanges();
            MessageBox.Show("Информация успешно сохранена.",
                            "Внимание!",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
            var currentGoods = ShopsGoodsContext.GetContext()
                                                .ShopGoods
                                                .Where(x => x.ShopId == _shopId)
                                                .Select(x => x.Good)
                                                .ToList();
            GoodsInCurrentShop = new ObservableCollection<Good>(currentGoods);
            GoodsGridModel.ItemsSource = GoodsInCurrentShop;
        }
    }
}
