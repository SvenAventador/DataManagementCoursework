using System.Windows;
using System.Windows.Controls;
using ShopsGoodsLibrary.Models;
using ShopsGoodsProject.Windows.Administrator.Windows;

namespace ShopsGoodsProject.Windows.Administrator.Pages
{
    /// <summary>
    /// Логика взаимодействия для Types.xaml
    /// </summary>
    public partial class Types : Page
    {
        public Types()
        {
            InitializeComponent();
            TypesGridModel.ItemsSource = ShopsGoodsContext.GetContext()
                                                          .Types
                                                          .OrderBy(x => x.Id)
                                                          .ToList();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            TypesGridModel.ItemsSource = ShopsGoodsContext.GetContext()
                                                          .Types
                                                          .OrderBy(x => x.Id)
                                                          .ToList();
        }

        private void CreateObject_Click(object sender, RoutedEventArgs e)
        {
            new AddEndEditTypes(null!).Show();
        }

        private void RefreshObject_Click(object sender, RoutedEventArgs e)
        {
            TypesGridModel.ItemsSource = ShopsGoodsContext.GetContext()
                                                          .Types
                                                          .OrderBy(x => x.Id)
                                                          .ToList();
        }

        private void DeleteObject_Click(object sender, RoutedEventArgs e)
        {
            var deleteElements = TypesGridModel.SelectedItems.Cast<ShopsGoodsLibrary.Models.Type>().ToList();
            if (MessageBox.Show($"Вы уверены, что хотите удалить {deleteElements.Count} элемента(-ов)?",
                                 "Внимание!",
                                 MessageBoxButton.YesNo,
                                 MessageBoxImage.Question) != MessageBoxResult.Yes)
                return;

            ShopsGoodsContext.GetContext()
                             .Types
                             .RemoveRange(deleteElements);
            ShopsGoodsContext.GetContext().SaveChanges();
            MessageBox.Show("Информация успешно сохранена.",
                            "Внимание!",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
            TypesGridModel.ItemsSource = ShopsGoodsContext.GetContext()
                                                          .Types
                                                          .OrderBy(x => x.Id)
                                                          .ToList();
        }

        private void EditObject_Click(object sender, RoutedEventArgs e)
        {
            new AddEndEditTypes((sender as Button)?.DataContext as ShopsGoodsLibrary.Models.Type).Show();
            
        }
    }
}
