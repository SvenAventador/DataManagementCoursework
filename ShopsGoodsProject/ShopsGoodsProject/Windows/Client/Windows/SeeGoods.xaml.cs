using Microsoft.EntityFrameworkCore;
using ShopsGoodsLibrary.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ShopsGoodsProject.Windows.Client.Windows
{
    /// <summary>
    /// Логика взаимодействия для SeeGoods.xaml
    /// </summary>
    public partial class SeeGoods : Window
    {
        private static string? _findOther;
        private static int _shopId;
        private List<Good> _goods = [];
        private List<Good> _allGoods = [];
        public SeeGoods(int shopId)
        {
            InitializeComponent();

            var customType = new ShopsGoodsLibrary.Models.Type
            {
                Id = -1,
                TypeName = "Все типы"
            };
            var customBrand = new Brand
            {
                Id = -1,
                BrandName = "Все бренды"
            };
            var types = ShopsGoodsContext.GetContext()
                                         .Types
                                         .OrderBy(x => x.Id)
                                         .ToList();
            types.Insert(0, customType);
            var brands = ShopsGoodsContext.GetContext()
                                          .Brands
                                          .OrderBy(x => x.Id)
                                          .ToList();
            brands.Insert(0, customBrand);
            TypeComboBox.ItemsSource = types;
            BrandComboBox.ItemsSource = brands;
            _shopId = shopId;

            _allGoods = ShopsGoodsContext.GetContext()
                                        .ShopGoods
                                        .Include(x => x.Good.Type)
                                        .Include(x => x.Good.Brand)
                                        .Where(x => x.ShopId == _shopId)
                                        .Where(x => x.Good.GoodCount > 0)
                                        .Select(x => x.Good)
                                        .ToList();

            GoodsListView.ItemsSource = _allGoods;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GoodsListView.ItemsSource = _allGoods;
            NameGood.Text = "Пожалуйста, введите название товара...";
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

        private void BrandComboBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
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

        private void NameGood_GotFocus(object sender, RoutedEventArgs e)
        {
            NameGood.Text = "";
        }

        private void NameGood_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(NameGood.Text))
                NameGood.Text = "Пожалуйста, введите название товара...";
        }

        private void NameGood_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            _findOther = NameGood.Text.ToLower();
            FilteredGoods();
        }

        private void TypeComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            FilteredGoods();
        }

        private void BrandComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            FilteredGoods();
        }

        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            var goodId = Convert.ToInt32(((Button)sender).Tag);
            var cart = ShopsGoodsContext.GetContext()
                                        .Carts
                                        .FirstOrDefault(x => x.UserId == ShopsGoodsLibrary.Functional.Manager.CurrentUser!.Id);

            var cartGood = ShopsGoodsContext.GetContext()
                                            .CartGoods
                                            .FirstOrDefault(x => (x.CartId == cart!.Id) &&
                                                                 (x.GoodId == goodId));
            
            if (cartGood == null)
            {
                var cart_good = new CartGood
                {
                    CartId = cart!.Id,
                    GoodId = goodId,
                    GoodAmount = 1
                };

                ShopsGoodsContext.GetContext().CartGoods.Add(cart_good);
                ShopsGoodsContext.GetContext().SaveChanges();
            }
            else
            {
                var good = ShopsGoodsContext.GetContext()
                                            .Goods
                                            .FirstOrDefault(x => x.Id == cartGood.GoodId);
                if (good!.GoodCount >= cartGood.GoodAmount)
                {
                    cartGood.GoodAmount++;
                }
                else
                {
                    MessageBox.Show("Товара больше нет на складе!",
                                    "Вниммание!",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                    return;
                }

                ShopsGoodsContext.GetContext().SaveChanges();
            }
            MessageBox.Show("Товар успешно добавлен в корзину!",
                            "Вниммание!",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
        }

        private void FilteredGoods()
        {
            var selectedType = (ShopsGoodsLibrary.Models.Type)TypeComboBox.SelectedItem;
            var selectedBrand = (Brand)BrandComboBox.SelectedItem;

            var filteredGoods = _allGoods.AsQueryable();

            if (!string.IsNullOrEmpty(_findOther) && NameGood.Text != "Пожалуйста, введите название товара...")
            {
                filteredGoods = filteredGoods.Where(x => x.NameGood.Contains(_findOther, StringComparison.CurrentCultureIgnoreCase));
            }

            if (selectedType != null && selectedType.Id != -1)
            {
                filteredGoods = filteredGoods.Where(x => x.TypeId == selectedType.Id);
            }

            if (selectedBrand != null && selectedBrand.Id != -1)
            {
                filteredGoods = filteredGoods.Where(x => x.BrandId == selectedBrand.Id);
            }

            GoodsListView.ItemsSource = filteredGoods.ToList();
        }
    }
}
