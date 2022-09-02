using System.Windows.Controls;

namespace Apache.Views.AdminPages
{
    /// <summary>
    /// Логика взаимодействия для CheckPage.xaml
    /// </summary>
    public partial class CheckPage : Page
    {
        public CheckPage()
        {
            InitializeComponent();
            DataContext = new CheckPageViewModel();
        }
    }
}
