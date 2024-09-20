using ShopsGoodsLibrary.Functional;
using ShopsGoodsLibrary.Models;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ShopsGoodsProject.Windows.Administrator.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddAndEditShops.xaml
    /// </summary>
    public partial class AddAndEditShops : Window
    {
        private readonly Shop? _shop = new();
        public AddAndEditShops(Shop? shop)
        {
            InitializeComponent();

            if (shop != null) _shop = shop;
            
            DataContext = _shop;
            Action.Text = _shop == null ? "Добавить магазин" : "Изменить данные магазина";
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

        private void CreateAction_Click(object sender, RoutedEventArgs e)
        {
            var errors = new StringBuilder();

            if (string.IsNullOrEmpty(ShopName.Text))
                errors.AppendLine("Пожалуйста, введите название магазина!");
            if (string.IsNullOrEmpty(ShopAddress.Text))
                errors.AppendLine("Пожалуйста, введите адрес магазина магазина!");
            if (string.IsNullOrEmpty(ShopPhone.Text))
                errors.AppendLine("Пожалуйста, введите телефон магазина!");
            else if (!ShopsGoodsLibrary.Functional.Validation.ValidPhone(ShopPhone.Text))
                errors.AppendLine("Пожалуйста, введите корректный номер телефона!");

            var existingName = ShopsGoodsContext.GetContext()
                                                .Shops
                                                .FirstOrDefault(x => x.ShopName == ShopName.Text);
            var existingAddress = ShopsGoodsContext.GetContext()
                                                   .Shops
                                                   .FirstOrDefault(x => x.ShopAddress == ShopAddress.Text);
            var existingPhone = ShopsGoodsContext.GetContext()
                                                 .Shops
                                                 .FirstOrDefault(x => x.ShopPhone == ShopPhone.Text);

            if (_shop?.Id == 0)
            {
                if (existingName != null)
                    errors.AppendLine("Данное наименование уже есть в системе. Пожалуйста, введите другое наименование магазина!");
                if (existingAddress != null)
                    errors.AppendLine("Данный адрес уже есть в системе. Пожалуйста, введите другой адрес магазина!");
                if (existingPhone != null)
                    errors.AppendLine("Данный номер телефона уже есть в системе. Пожалуйста, введите другой номер телефона!");
            }
            else
            {
                if (existingName != null && existingName.Id != _shop?.Id)
                    errors.AppendLine("Данное наименование уже есть в системе. Пожалуйста, введите другое наименование магазина!");
                if (existingAddress != null && existingAddress.Id != _shop?.Id)
                    errors.AppendLine("Данный адрес уже есть в системе. Пожалуйста, введите другой адрес магазина!");
                if (existingPhone != null && existingPhone.Id != _shop?.Id)
                    errors.AppendLine("Данный номер телефона уже есть в системе. Пожалуйста, введите другой номер телефона!");
            }

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(),
                                "Внимание!",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return;
            }

            if (_shop?.Id == 0)
                ShopsGoodsContext.GetContext().Shops.Add(_shop);

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
}
