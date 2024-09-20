using System.Text;
using System.Windows;
using System.Windows.Input;
using ShopsGoodsLibrary.Models;

namespace ShopsGoodsProject.Windows.Administrator.Windows;

public partial class AddAndEditBrands : Window
{
    private readonly Brand? _brand = new();
    public AddAndEditBrands(Brand? brand)
    {
        InitializeComponent();

        if (brand != null) _brand = brand;

        DataContext = _brand;
        Action.Text = _brand == null ? "Добавить бренд" : "Изменить данные бренда";
    }

    private void AddAndEditBrands_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
            DragMove();
    }

    private void MinimizeButton_OnClick(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }

    private void CloseButton_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void CreateAction_OnClick(object sender, RoutedEventArgs e)
    {
        var errors = new StringBuilder();

        if (string.IsNullOrEmpty(BrandName.Text))
            errors.AppendLine("Пожалуйста, введите наименование бренда товара!");

        var existingBrand = ShopsGoodsContext.GetContext().Brands.FirstOrDefault(x => x.BrandName == BrandName.Text);

        if (_brand?.Id == 0)
        {
            if (existingBrand != null)
                errors.AppendLine("Данное наименование уже есть в системе. Пожалуйста, введите другое наименование бренда!");
        }
        else
        {
            if (existingBrand != null && existingBrand.Id != _brand?.Id)
                errors.AppendLine("Данное наименование уже есть в системе. Пожалуйста, введите другое наименование бренда!");
        }

        if (errors.Length > 0)
        {
            MessageBox.Show(errors.ToString(),
                            "Внимание!",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
            return;
        }

        if (_brand?.Id == 0)
            ShopsGoodsContext.GetContext().Brands.Add(_brand);

        try
        {
            ShopsGoodsContext.GetContext().SaveChanges();
            MessageBox.Show("Данные успешно сохранены!",
                            "Внимание!",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.ToString(),
                            "Внимание!",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
        }
    }
}