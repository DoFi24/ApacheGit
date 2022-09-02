using System.Windows.Controls;

namespace Apache.Views.AdminPages
{
    /// <summary>
    /// Логика взаимодействия для ProductBuyPage.xaml
    /// </summary>
    public partial class ProductBuyPage : Page
    {
        public ProductBuyPage()
        {
            InitializeComponent();
            DataContext = new ProductBuyPageViewModel();
        }
    }
}
