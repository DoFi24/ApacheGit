using System.Windows.Controls;

namespace Apache.Views
{
    /// <summary>
    /// Логика взаимодействия для SalePage.xaml
    /// </summary>
    public partial class SalePage : Page
    {
        public SalePage(MainWindowViewModel window,Places places,Checks? checks = null)
        {
            InitializeComponent();
            DataContext = new SalePageViewModel(window,places,checks);
        }
    }
}
