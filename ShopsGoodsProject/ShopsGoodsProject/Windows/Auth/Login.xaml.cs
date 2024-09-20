using System.Text;
using System.Windows;
using System.Windows.Input;
using ShopsGoodsLibrary.Functional;
using ShopsGoodsLibrary.Models;
using ShopsGoodsProject.Windows.Administrator;

namespace ShopsGoodsProject.Windows.Auth;

public partial class Login : Window
{
    public Login()
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

        if (errors.Length > 0)
        {
            MessageBox.Show(errors.ToString(),
                            "Внимание!",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
            return;
        }

        var existingUser = ShopsGoodsContext.GetContext().Users.FirstOrDefault(x => x.UserEmail == UserEmail.Text &&
                                                                                    x.UserPassword == Validation.GetHashString(UserPassword.Password));
        if (existingUser != null)
        {
            MessageBox.Show($"Добро пожаловать, {existingUser.UserEmail}!",
                            "Внимание!",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
            switch (existingUser.UserRole)
            {
                case "Администратор":
                    new Admin().Show();
                    break;
                case "Менеджер":
                    new Manager.Manager().Show();
                    break;
                default:
                    new Client.Client().Show();
                    break;
            }

            ShopsGoodsLibrary.Functional.Manager.CurrentUser = existingUser;
            Hide();
        }
        else
        {
            MessageBox.Show("Неправильно введены почта или пароль!",
                            "Внимание!",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
        }
    }

    private void RegWindowButton_Click(object sender, RoutedEventArgs e)
    {
        new Registration().Show();
        Hide();
    }
}
