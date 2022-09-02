using System.Windows.Controls;

namespace Apache.Views.AdminPages
{
    /// <summary>
    /// Логика взаимодействия для OstatokPage.xaml
    /// </summary>
    public partial class OstatokPage : Page
    {
        public OstatokPage()
        {
            InitializeComponent();
            DataContext = new OstatokPageViewModel();
        }
    }
}
