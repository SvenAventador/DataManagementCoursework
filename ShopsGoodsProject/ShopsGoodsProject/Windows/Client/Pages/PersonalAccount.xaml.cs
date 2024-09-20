using ShopsGoodsLibrary.Models;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace ShopsGoodsProject.Windows.Client.Pages
{
    /// <summary>
    /// Логика взаимодействия для PersonalAccount.xaml
    /// </summary>
    public partial class PersonalAccount : Page
    {
        public PersonalAccount()
        {
            InitializeComponent();
            DataContext = ShopsGoodsLibrary.Functional.Manager.CurrentUser;
        }

        private void SaveData_Click(object sender, RoutedEventArgs e)
        {
            var errors = new StringBuilder();
            var user = ShopsGoodsLibrary.Functional.Manager.CurrentUser;

            if (!string.IsNullOrEmpty(UserEmail.Text) && UserEmail.Text != user!.UserEmail)
            {
                if (!ShopsGoodsLibrary.Functional.Validation.ValidEmail(UserEmail.Text))
                    errors.Append("Пожалуйста, введите корректную почту!\n");
                else
                {
                    var existingUser = ShopsGoodsContext.GetContext()
                                                        .Users
                                                        .Where(x => x.Id != user.Id)
                                                        .FirstOrDefault(x => x.UserEmail == UserEmail.Text);
                    if (existingUser != null)
                        errors.Append("Пользователь с такой почтой уже существует!\n");
                    else
                        user.UserEmail = UserEmail.Text;
                }
            }

            if (!string.IsNullOrEmpty(UserPassword.Password) && ShopsGoodsLibrary.Functional.Validation.GetHashString(UserPassword.Password) != user!.UserPassword)
            {
                if (UserPassword.Password.Length < 8)
                    errors.Append("Длина пароля должна быть минимум 8 символов!\n");
                else if (UserPassword.Password != RepeatPassword.Password)
                    errors.Append("Пожалуйста, введите одинаковые пароли!\n");
                else
                    user.UserPassword = ShopsGoodsLibrary.Functional.Validation.GetHashString(UserPassword.Password);
            }

            // Проверка на изменение ФИО
            if (!string.IsNullOrEmpty(UserFio.Text) && UserFio.Text != user!.UserFio)
            {
                if (UserFio.Text.Split(' ').Length < 2)
                    errors.Append("Пожалуйста, введите корректное ФИО!\n");
                else
                    user.UserFio = UserFio.Text;
            }

            if (!string.IsNullOrEmpty(UserBirthday.Text) && DateOnly.TryParse(UserBirthday.Text, out DateOnly birthday) && birthday != user!.UserBirthday)
            {
                user.UserBirthday = birthday;
            }

            if (!string.IsNullOrEmpty(UserPhone.Text) && UserPhone.Text != user!.UserPhone)
            {
                if (!ShopsGoodsLibrary.Functional.Validation.ValidPhone(UserPhone.Text))
                    errors.Append("Пожалуйста, введите корректный номер телефона!\n");
                else
                {
                    var existingPhone = ShopsGoodsContext.GetContext()
                                                         .Users
                                                         .Where(x => x.Id != user.Id)
                                                         .FirstOrDefault(x => x.UserPhone == UserPhone.Text);
                    if (existingPhone != null)
                        errors.Append("Пользователь с таким номером телефона уже существует!\n");
                    else
                        user.UserPhone = UserPhone.Text;
                }
            }

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(),
                                "Внимание!",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return;
            }

            try
            {
                ShopsGoodsContext.GetContext().SaveChanges();
                MessageBox.Show("Данные успешно сохранены",
                                "Внимание!",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
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
