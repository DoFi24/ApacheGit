using System.Windows.Controls;

namespace Apache.Views.AdminPages
{
    /// <summary>
    /// Логика взаимодействия для ProductPage.xaml
    /// </summary>
    public partial class ProductPage : Page
    {
        public ProductPage()
        {
            InitializeComponent();
            DataContext = new ProductPageViewModel();
        }
    }
}
