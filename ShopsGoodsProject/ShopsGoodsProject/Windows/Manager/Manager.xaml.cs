using ShopsGoodsProject.Windows.Auth;
using System.Windows;
using System.Windows.Input;

namespace ShopsGoodsProject.Windows.Manager;

public partial class Manager : Window
{
    public Manager()
    {
        InitializeComponent();
        MainFrame.Navigate(new Pages.Shops());
        ShopsGoodsLibrary.Functional.Manager.MainFrame = MainFrame;
    }

    private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
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

    private void ShopButton_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new Pages.Shops());
    }

    private void GoodsShop_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new Pages.Goods());
    }

    private void UserShop_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new Pages.Users());
    }

    private void ExitAccount_Click(object sender, RoutedEventArgs e)
    {
        new Login().Show();
        Hide();
    }
}