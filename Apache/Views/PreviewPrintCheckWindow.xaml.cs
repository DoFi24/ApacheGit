using System;
using System.Windows;
using System.Windows.Controls;

namespace Apache.Views
{
    /// <summary>
    /// Логика взаимодействия для PreviewPrintCheckWindow.xaml
    /// </summary>
    public partial class PreviewPrintCheckWindow : Window
    {
        public PreviewPrintCheckWindow(SalePageViewModel model)
        {
            InitializeComponent();
            DataContext = new PreviewPrintCheckWindowViewModel(model);
            DateTimeStr.Text = DateTime.UtcNow.ToString("HH:mm dd.MMMM.yyyy");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                new PrintDialog().PrintVisual(printGrid, "Печать чека");
                Close();
            }
            catch
            {

            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
