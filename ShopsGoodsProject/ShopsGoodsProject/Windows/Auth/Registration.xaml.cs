using System.Text;
using System.Windows;
using System.Windows.Input;
using ShopsGoodsLibrary.Functional;
using ShopsGoodsLibrary.Models;

namespace ShopsGoodsProject.Windows.Auth;

public partial class Registration : Window
{
    public Registration()
    {
        InitializeComponent();
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
        Application.Current.Shutdown();
    }

    private void AuthButton_Click(object sender, RoutedEventArgs e)
    {
        var errors = new StringBuilder();

        if (string.IsNullOrEmpty(UserEmail.Text))
            errors.Append("Пожалуйста, введите почту!\n");
        else if (!Validation.ValidEmail(UserEmail.Text))
            errors.Append("Пожалуйста, введите корректную почту!\n");

        if (string.IsNullOrEmpty(UserPassword.Password))
            errors.Append("Пожалуйста, введите пароль!\n");
        else if (UserPassword.Password.Length < 8)
            errors.Append("Длина пароля должна быть минимум 8 символов!\n");
        else if (UserPassword.Password != RepeatPassword.Password)
            errors.Append("Пожалуйста, введите одинаковые пароли!\n");

        if (string.IsNullOrEmpty(UserFio.Text))
            errors.Append("Пожалуйста, введите Ваше ФИО!\n");
        else if (UserFio.Text.Split(' ').Length < 2)
            errors.Append("Пожалуйста, введите корректное ФИО!\n");

        if (string.IsNullOrEmpty(UserBirthday.Text))
            errors.Append("Пожалуйста, введите Вашу дату рождения!\n");

        if (string.IsNullOrEmpty(UserPhone.Text))
            errors.Append("Пожалуйста, Введите Ваш номер телефона!\n");
        else if (!Validation.ValidPhone(UserPhone.Text))
            errors.Append("Пожалуйста, введите корректный номер телефона!\n");

        var existingUser = ShopsGoodsContext.GetContext()
                                            .Users
                                            .FirstOrDefault(x => x.UserEmail == UserEmail.Text);
        if (existingUser != null)
            errors.Append("Пользователь с такой почтой уже существует!\n");

        var existingPhone = ShopsGoodsContext.GetContext()
                                             .Users
                                             .FirstOrDefault(x => x.UserPhone == UserPhone.Text);
        if (existingPhone != null)
            errors.Append("Пользователь с таким номером телефона уже существует!\n");

        if (errors.Length > 0)
        {
            MessageBox.Show(errors.ToString(),
                            "Внимание!",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
            return;
        }

        var user = new User
        {
            UserFio = UserFio.Text,
            UserBirthday = DateOnly.Parse(UserBirthday.Text),
            UserEmail = UserEmail.Text,
            UserPassword = Validation.GetHashString(UserPassword.Password),
            UserPhone = UserPhone.Text,
            UserRole = "Пользователь"
        };

        try
        {
            ShopsGoodsContext.GetContext().Users.Add(user);
            ShopsGoodsContext.GetContext().SaveChanges();
            var cart = new Cart
            {
                UserId = user.Id,
            };
            ShopsGoodsContext.GetContext().Carts.Add(cart);
            ShopsGoodsContext.GetContext().SaveChanges();
            MessageBox.Show("Регистрация прошла успешно!",
                            "Внимание!",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
            new Login().Show();
            Hide();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Ошибка при сохранении данных: " + ex.Message,
                            "Внимание",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
        }
    }

    private void AuthWindowButton_Click(object sender, RoutedEventArgs e)
    {
        new Login().Show();
        Hide();
    }

}
