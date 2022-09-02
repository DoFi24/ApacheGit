using System.Linq;
using System.Windows;

namespace Apache.Views
{
    /// <summary>
    /// Логика взаимодействия для ChangeTableWindow.xaml
    /// </summary>
    public partial class ChangeTableWindow : Window
    {
        private SalePageViewModel _vm;
        public ChangeTableWindow(SalePageViewModel vm)
        {
            InitializeComponent();
            _vm = vm;
            GetAllTable();
        }
        private async void GetAllTable()
        {
            await using (ApplicationContext db = new ApplicationContext())
            {
                var result = db.Places!.OrderBy(s => s.Name);
                foreach (var item in result)
                    TableCollection.Items.Add(item);
            }
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            _vm._currentCheck!.PlacesId = (TableCollection.SelectedItem as Places)!.Id;
            _vm._viewModel!.ExecuteShowTablePageCommand("");
            Close();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
