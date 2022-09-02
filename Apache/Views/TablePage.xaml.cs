using System.Windows.Controls;

namespace Apache.Views
{
    /// <summary>
    /// Логика взаимодействия для TablePage.xaml
    /// </summary>
    public partial class TablePage : Page
    {
        public TablePage(MainWindowViewModel window)
        {
            InitializeComponent();
            DataContext = new TablePageViewModel(window);
        }
    }
}
