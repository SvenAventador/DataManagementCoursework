using ShopsGoodsLibrary.Models;
using ShopsGoodsProject.Windows.Administrator.Windows.Users;
using System.Windows;
using System.Windows.Controls;

namespace ShopsGoodsProject.Windows.Administrator.Pages
{
    /// <summary>
    /// Логика взаимодействия для Users.xaml
    /// </summary>
    public partial class Users : Page
    {
        public Users()
        {
            InitializeComponent();
            UsersGridModel.ItemsSource = ShopsGoodsContext.GetContext()
                                                          .Users
                                                          .Where(x => x.UserRole == "Пользователь")
                                                          .ToList();
        }

        private void SeeOrder_Click(object sender, RoutedEventArgs e)
        {
            var userId = (sender as Button)?.Tag;
            new SeeOrders(Convert.ToInt32(userId)).Show();
        }
    }
}
