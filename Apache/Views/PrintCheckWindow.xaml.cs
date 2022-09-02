using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Apache.Views
{
    /// <summary>
    /// Логика взаимодействия для PrintCheckWindow.xaml
    /// </summary>
    public partial class PrintCheckWindow : Window
    {
        SalePageViewModel _model;
        public PrintCheckWindow(SalePageViewModel model)
        {
            InitializeComponent();
            _model = model;
            DataContext = new PrintCheckWindowViewModel(model);
            DateTimeStr.Text = DateTime.UtcNow.ToString("HH:mm dd.MMMM.yyyy");
        }
        private void PrintMethod()
        {
            try
            {
                new PrintDialog().PrintVisual(printGrid, "Печать чека");
            }
            catch
            {

            }
        }
        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PrintMethod();
            _model!._viewModel!.ExecuteShowTablePageCommand("");
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            _model!._viewModel!.ExecuteShowTablePageCommand("");
            Close();
        }
    }
}
