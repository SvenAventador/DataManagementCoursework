using ShopsGoodsLibrary.Models;
using ShopsGoodsProject.Windows.Administrator.Windows;
using System.Windows;
using System.Windows.Controls;

namespace ShopsGoodsProject.Windows.Administrator.Pages
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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ShopGridModel.ItemsSource = ShopsGoodsContext.GetContext()
                                                         .Shops
                                                         .OrderBy(x => x.Id)
                                                         .ToList();
        }

        private void CreateObject_Click(object sender, RoutedEventArgs e)
        {
            new AddAndEditShops(null!).Show();
        }

        private void RefreshObject_Click(object sender, RoutedEventArgs e)
        {
            ShopGridModel.ItemsSource = ShopsGoodsContext.GetContext()
                                                         .Shops
                                                         .OrderBy(x => x.Id)
                                                         .ToList();
        }

        private void EditObject_Click(object sender, RoutedEventArgs e)
        {
            new AddAndEditShops((sender as Button)?.DataContext as Shop).Show();
        }

        private void DeleteObject_Click(object sender, RoutedEventArgs e)
        {
            var deleteElements = ShopGridModel.SelectedItems.Cast<Shop>().ToList();
            if (MessageBox.Show($"Вы уверены, что хотите удалить {deleteElements.Count} элемента(-ов)?",
                                "Внимание!",
                                MessageBoxButton.YesNo,
                                MessageBoxImage.Question) != MessageBoxResult.Yes) 
                return;
            
            ShopsGoodsContext.GetContext()
                             .Shops
                             .RemoveRange(deleteElements);
            ShopsGoodsContext.GetContext().SaveChanges();
            MessageBox.Show("Информация успешно сохранена.",
                            "Внимание!",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
            ShopGridModel.ItemsSource = ShopsGoodsContext.GetContext()
                                                         .Shops
                                                         .OrderBy(x => x.Id)
                                                         .ToList();
        }
    }
}
