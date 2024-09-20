using Microsoft.EntityFrameworkCore;
using ShopsGoodsLibrary.Models;
using System.Windows;
using System.Windows.Controls;

namespace ShopsGoodsProject.Windows.Client.Pages
{
    /// <summary>
    /// Логика взаимодействия для Cart.xaml
    /// </summary>
    public partial class Cart : Page
    {
        public Cart()
        {
            InitializeComponent();
            var cart = ShopsGoodsContext.GetContext()
                                        .Carts
                                        .FirstOrDefault(x => x.UserId == ShopsGoodsLibrary.Functional.Manager.CurrentUser!.Id);

            GoodsGridModel.ItemsSource = ShopsGoodsContext.GetContext()
                                                          .CartGoods
                                                          .Include(x => x.Good.Type)
                                                          .Include(x => x.Good.Brand)
                                                          .Where(x => x.CartId == cart!.Id)
                                                          .Select(x => new
                                                          {
                                                              x.Good.Id,
                                                              x.Good.NameGood,
                                                              x.Good.GetPrice,
                                                              x.GoodAmount,
                                                              x.Good.GetImage,
                                                              x.Good.Type.TypeName,
                                                              x.Good.Brand.BrandName
                                                          })
                                                          .ToList();
            UpdateTotalPrice();
        }

        private void DeleteObject_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show($"Вы уверены, что хотите удалить данный товар из корзины?",
                                "Внимание!",
                                MessageBoxButton.YesNo,
                                MessageBoxImage.Question) != MessageBoxResult.Yes)
                return;

            var goodId = Convert.ToInt32((sender as Button)?.Tag);
            var cart = ShopsGoodsContext.GetContext()
                                        .Carts
                                        .FirstOrDefault(x => x.UserId == ShopsGoodsLibrary.Functional.Manager.CurrentUser!.Id);
            var cartGood = ShopsGoodsContext.GetContext()
                                            .CartGoods
                                            .FirstOrDefault(x => (x.CartId == cart!.Id) &&
                                                                 (x.GoodId == goodId));
            ShopsGoodsContext.GetContext().CartGoods.Remove(cartGood!);
            ShopsGoodsContext.GetContext().SaveChanges();
            GoodsGridModel.ItemsSource = ShopsGoodsContext.GetContext()
                                                          .CartGoods
                                                          .Include(x => x.Good.Type)
                                                          .Include(x => x.Good.Brand)
                                                          .Where(x => x.CartId == cart!.Id)
                                                          .Select(x => new
                                                          {
                                                              x.Good.Id,
                                                              x.Good.NameGood,
                                                              x.Good.GetPrice,
                                                              x.GoodAmount,
                                                              x.Good.GetImage,
                                                              x.Good.Type.TypeName,
                                                              x.Good.Brand.BrandName
                                                          })
                                                          .ToList();
            UpdateTotalPrice();
        }

        private void CreateOrder_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show($"Вы действительно хотите оформить заказ?",
                                "Внимание!",
                                MessageBoxButton.YesNo,
                                MessageBoxImage.Question) != MessageBoxResult.Yes)
                return;

            var cart = ShopsGoodsContext.GetContext()
                                        .Carts
                                        .FirstOrDefault(x => x.UserId == ShopsGoodsLibrary.Functional.Manager.CurrentUser!.Id);
            var totalPrice = ShopsGoodsContext.GetContext()
                                              .CartGoods
                                              .Where(x => x.CartId == cart!.Id)
                                              .Sum(x => x.Good.GoodPrice * x.GoodAmount);
            var order = new Order
            {
                OrderDate = DateTime.Now,
                OrderPrice = (int)totalPrice!,
                UserId = ShopsGoodsLibrary.Functional.Manager.CurrentUser!.Id,
                OrderStatusId = 1
            };
            ShopsGoodsContext.GetContext().Orders.Add(order);
            ShopsGoodsContext.GetContext().SaveChanges();

            var cartGoods = ShopsGoodsContext.GetContext()
                                             .CartGoods
                                             .Where(x => x.CartId == cart!.Id)
                                             .ToList();
            foreach (var cartGood in cartGoods)
            {
                var orderGood = new OrderGood
                {
                    OrderId = order.Id,
                    GoodId = cartGood.GoodId
                };
                ShopsGoodsContext.GetContext().OrderGoods.Add(orderGood);
            }
            ShopsGoodsContext.GetContext().SaveChanges();

            foreach (var cartGood in cartGoods)
            {
                var good = ShopsGoodsContext.GetContext()
                                            .Goods
                                            .FirstOrDefault(x => x.Id == cartGood.GoodId);

                if (good != null)
                {
                    good.GoodCount -= (int)cartGood.GoodAmount!; 
                }
            }
            ShopsGoodsContext.GetContext().CartGoods.RemoveRange(cartGoods);
            ShopsGoodsContext.GetContext().SaveChanges();

            MessageBox.Show("Поздравляем, заказ успешно сформирован!",
                            "Внимание!",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
            GoodsGridModel.ItemsSource = ShopsGoodsContext.GetContext()
                                                          .CartGoods
                                                          .Include(x => x.Good.Type)
                                                          .Include(x => x.Good.Brand)
                                                          .Where(x => x.CartId == cart!.Id)
                                                          .Select(x => new
                                                          {
                                                              x.Good.Id,
                                                              x.Good.NameGood,
                                                              x.Good.GetPrice,
                                                              x.GoodAmount,
                                                              x.Good.GetImage,
                                                              x.Good.Type.TypeName,
                                                              x.Good.Brand.BrandName
                                                          })
                                                          .ToList();
            UpdateTotalPrice();
        }

        private void UpdateTotalPrice()
        {
            var cart = ShopsGoodsContext.GetContext()
                                        .Carts
                                        .FirstOrDefault(x => x.UserId == ShopsGoodsLibrary.Functional.Manager.CurrentUser!.Id);
            var totalPrice = ShopsGoodsContext.GetContext()
                                              .CartGoods
                                              .Where(x => x.CartId == cart!.Id)
                                              .Sum(x => x.Good.GoodPrice * x.GoodAmount);

            FullPrice.Text = $"Общая сумма заказа: {Convert.ToString($"{totalPrice}₽")}"; 
        }
    }
}
