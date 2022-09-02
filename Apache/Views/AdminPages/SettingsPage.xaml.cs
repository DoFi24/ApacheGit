using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Apache.Views.AdminPages
{
    /// <summary>
    /// Логика взаимодействия для SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();
           
        }
        private async void GetSettings()
        {
            await using (ApplicationContext db = new ApplicationContext())
            {
                ServiceDiscountTextBox.Text = db.Settings!.First(s => s.Key == "ServiceDiscount").Value;
            }
        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await using (ApplicationContext db = new ApplicationContext()) 
            {
                var service = db.Settings!.First(s=>s.Key == "ServiceDiscount");
                service.Key = ServiceDiscountTextBox.Text;
                db.SaveChanges();
            }
        }
    }
}
