using ShopsGoodsLibrary.Models;
using System.Windows;
using Microsoft.Win32;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Text;
using System.IO;

namespace ShopsGoodsProject.Windows.Administrator.Windows.Goods
{
    /// <summary>
    /// Логика взаимодействия для AddAndEditGoods.xaml
    /// </summary>
    public partial class AddAndEditGoods : Window
    {
        private Good? _good;
        private int _shopId;
        private string? _selectedImage;
        public AddAndEditGoods(Good? good, int shopId)
        {
            InitializeComponent();
            TypeComboBox.ItemsSource = ShopsGoodsContext.GetContext()
                                                        .Types
                                                        .OrderBy(x => x.Id)
                                                        .ToList();
            BrandComboBox.ItemsSource = ShopsGoodsContext.GetContext()
                                                         .Brands
                                                         .OrderBy(x => x.Id)
                                                         .ToList();
            
            if (good != null) _good = good;

            DataContext = _good;
            _shopId = shopId;
            _selectedImage = _good?.GoodImage;
            Action.Text = _good == null ? "Добавление товара" : "Изменение товара";
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void BrandComboBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is System.Windows.Controls.ComboBox comboBox)
            {
                if (!comboBox.IsDropDownOpen)
                {
                    comboBox.IsDropDownOpen = true;
                }
            }
        }

        private void TypeComboBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is System.Windows.Controls.ComboBox comboBox)
            {
                if (!comboBox.IsDropDownOpen)
                {
                    comboBox.IsDropDownOpen = true;
                }
            }
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ChooseImage_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png|All files (*.*)|*.*",
                Title = "Выберите изображение"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                _selectedImage = openFileDialog.FileName;
                SelectedImage.Source = new BitmapImage(new Uri(_selectedImage));
            }
        }

        private void CreateAction_Click(object sender, RoutedEventArgs e)
        {
            var errors = new StringBuilder();

            if (string.IsNullOrEmpty(NameGood.Text))
                errors.AppendLine("Пожалуйста, введите название товара!");
            if (string.IsNullOrEmpty(DescriptionGood.Text))
                errors.AppendLine("Пожалуйста, введите описание товара!");

            if (string.IsNullOrEmpty(GoodPrice.Text))
                errors.AppendLine("Пожалуйста, введите цену товара!");
            else if (!int.TryParse(GoodPrice.Text, out int price) || price < 100 || price > 100000)
                errors.AppendLine("Цена товара должна быть между 100₽ и 100000₽!");

            if (string.IsNullOrEmpty(GoodCount.Text))
                errors.AppendLine("Пожалуйста, введите количество товара!");
            else if (!int.TryParse(GoodCount.Text, out int count) || count <= 0)
                errors.AppendLine("Количество товара должно быть больше 0!");

            if (TypeComboBox.SelectedItem == null)
                errors.AppendLine("Пожалуйста, выберите тип товара!");
            if (BrandComboBox.SelectedItem == null)
                errors.AppendLine("Пожалуйста, выберите бренд товара!");

            if (string.IsNullOrEmpty(_selectedImage) || !File.Exists(_selectedImage))
                errors.AppendLine("Пожалуйста, выберите изображение товара!");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Внимание!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _good ??= new Good();
            _good.NameGood = NameGood.Text;
            _good.DescriptionGood = DescriptionGood.Text;
            _good.GoodPrice = Convert.ToInt32(GoodPrice.Text);
            _good.GoodCount = Convert.ToInt32(GoodCount.Text);
            _good.TypeId = (TypeComboBox.SelectedItem as ShopsGoodsLibrary.Models.Type)?.Id ?? 0;
            _good.BrandId = (BrandComboBox.SelectedItem as Brand)?.Id ?? 0;
            _good.GoodImage = _selectedImage != null ? Path.GetFullPath(_selectedImage) : SelectedImage?.Source?.ToString() ?? string.Empty;

            var existingGood = ShopsGoodsContext.GetContext()
                                                .ShopGoods
                                                .Where(x => x.ShopId == _shopId)
                                                .Select(x => x.Good)
                                                .FirstOrDefault(x => x.NameGood == _good.NameGood && x.Id != _good.Id);

            if (existingGood != null)
            {
                MessageBox.Show("Товар с таким именем уже существует!", 
                                "Внимание!", 
                                MessageBoxButton.OK, 
                                MessageBoxImage.Error);
                return;
            }
          
            try
            { 
                if (_good.Id == 0)
                {
                    ShopsGoodsContext.GetContext().Goods.Add(_good);
                    ShopsGoodsContext.GetContext().SaveChanges();

                    var shopGood = new ShopGood
                    {
                        GoodId = _good.Id,
                        ShopId = _shopId
                    };
                    ShopsGoodsContext.GetContext().ShopGoods.Add(shopGood);
                    ShopsGoodsContext.GetContext().SaveChanges();
                }

                MessageBox.Show("Данные успешно сохранены!", 
                                "Внимание!", 
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения данных: {ex.Message}", 
                                "Ошибка",
                                MessageBoxButton.OK, 
                                MessageBoxImage.Error);
            }
        }
    }
}
