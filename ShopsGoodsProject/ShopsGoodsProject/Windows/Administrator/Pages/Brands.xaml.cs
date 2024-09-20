using System.Windows;
using System.Windows.Controls;
using ShopsGoodsLibrary.Models;
using ShopsGoodsProject.Windows.Administrator.Windows;

namespace ShopsGoodsProject.Windows.Administrator.Pages;

public partial class Brands : Page
{
    public Brands()
    {
        InitializeComponent();
        BrandGridModel.ItemsSource = ShopsGoodsContext.GetContext()
                                                      .Brands
                                                      .OrderBy(x => x.Id)
                                                      .ToList();
    }

    private void Brands_OnLoaded(object sender, RoutedEventArgs e)
    {
        BrandGridModel.ItemsSource = ShopsGoodsContext.GetContext()
                                                      .Brands
                                                      .OrderBy(x => x.Id)
                                                      .ToList();
    }

    private void CreateObject_OnClick(object sender, RoutedEventArgs e)
    {
        new AddAndEditBrands(null!).Show();
    }

    private void RefreshObject_Click(object sender, RoutedEventArgs e)
    {
        BrandGridModel.ItemsSource = ShopsGoodsContext.GetContext()
                                                      .Brands
                                                      .OrderBy(x => x.Id)
                                                      .ToList();
    }

    private void EditObject_OnClick(object sender, RoutedEventArgs e)
    {
        new AddAndEditBrands((sender as Button)?.DataContext as Brand).Show();
    }
    
    private void DeleteObject_OnClick(object sender, RoutedEventArgs e)
    {
        var deleteElements = BrandGridModel.SelectedItems.Cast<Brand>().ToList();
        if (MessageBox.Show($"Вы уверены, что хотите удалить {deleteElements.Count} элемента(-ов)?",
                             "Внимание!",
                             MessageBoxButton.YesNo,
                             MessageBoxImage.Question) != MessageBoxResult.Yes) 
            return;
        
        ShopsGoodsContext.GetContext().Brands.RemoveRange(deleteElements);
        ShopsGoodsContext.GetContext().SaveChanges();
        MessageBox.Show("Информация успешно сохранена.",
                        "Внимание!",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
        BrandGridModel.ItemsSource = ShopsGoodsContext.GetContext()
                                                      .Brands
                                                      .OrderBy(x => x.Id)
                                                      .ToList();
    }
}