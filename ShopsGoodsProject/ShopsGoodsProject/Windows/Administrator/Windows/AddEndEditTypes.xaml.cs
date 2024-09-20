using System.Text;
using System.Windows;
using System.Windows.Input;
using ShopsGoodsLibrary.Models;

namespace ShopsGoodsProject.Windows.Administrator.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddEndEditTypes.xaml
    /// </summary>
    public partial class AddEndEditTypes : Window
    {
        private readonly ShopsGoodsLibrary.Models.Type? _type = new();
        public AddEndEditTypes(ShopsGoodsLibrary.Models.Type? type)
        {
            InitializeComponent();

            if (type != null) _type = type;

            DataContext = _type;
            Action.Text = _type == null ? "Добавить тип" : "Изменить данные типа";
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

            if (string.IsNullOrEmpty(TypeName.Text))
                errors.AppendLine("Пожалуйста, введите наименование типа товара!");

            var existingType = ShopsGoodsContext.GetContext().Types.FirstOrDefault(x => x.TypeName == TypeName.Text);

            if (_type?.Id == 0)
            {
                if (existingType != null)
                    errors.AppendLine("Данное наименование уже есть в системе. Пожалуйста, введите другое наименование типа!");
            }
            else
            {
                if (existingType != null && existingType.Id != _type?.Id)
                    errors.AppendLine("Данное наименование уже есть в системе. Пожалуйста, введите другое наименование типа!");
            }

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(),
                                "Внимание!",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return;
            }

            if (_type?.Id == 0)
                ShopsGoodsContext.GetContext().Types.Add(_type);

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
