using System.Windows;
using System.Windows.Input;

namespace ShopsGoodsProject.Windows.Client;

public partial class Client : Window
{
    public Client()
    {
        InitializeComponent();
        MainFrame.Navigate(new Pages.Goods());
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

    private void Goods_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new Pages.Goods());
    }

    private void PersonalAccount_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new Pages.PersonalAccount());
    }

    private void Cart_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new Pages.Cart());
    }

    private void Orders_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new Pages.Orders());
    }
}