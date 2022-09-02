using Apache.Views.AdminPages;
using System.Windows;
using System.Windows.Controls;

namespace Apache.Views
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
            ChangeWindow(new AccountPage());
        }

        private void ChangePageClick(object sender, RoutedEventArgs e)
        {
            var id = (sender as RadioButton)!.Uid;
            switch (id)
            {
                case "0":
                    ChangeWindow(new AccountPage());
                    break;
                case "1":
                    ChangeWindow(new ProductPage());
                    break;
                case "2":
                    ChangeWindow(new ProductBuyPage());
                    break;
                case "3":
                    ChangeWindow(new OstatokPage());
                    break;
                case "4":
                    ChangeWindow(new CheckPage());
                    break;
                case "5":
                    ChangeWindow(new SettingsPage());
                    break;
            }
        }
        private void ChangeWindow(Page page) 
        {
            MainFrame.Navigate(page);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new AuthorizationWindow().Show();
            Close();
        }
    }
}
