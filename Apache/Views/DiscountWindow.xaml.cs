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
    /// Логика взаимодействия для DiscountWindow.xaml
    /// </summary>
    public partial class DiscountWindow : Window
    {
        private int Count = 1;
        public DiscountWindow()
        {
            InitializeComponent();
        }

        private void Click1(object sender, RoutedEventArgs e)
        {
            if (Count < 100)
                Count++;
            CountTextBox.Text = Count.ToString();
        }

        private void Click3(object sender, RoutedEventArgs e)
        {
            if (Count < 97)
                Count += 3;
            CountTextBox.Text = Count.ToString();
        }

        private void Click5(object sender, RoutedEventArgs e)
        {
            if (Count < 95)
                Count += 5;
            CountTextBox.Text = Count.ToString();
        }

        private void ClickM1(object sender, RoutedEventArgs e)
        {
            if (Count > 0)
                Count--;
            CountTextBox.Text = Count.ToString();
        }

        private void ClickM3(object sender, RoutedEventArgs e)
        {
            if (Count > 0)
            {
                if (Count < 3)
                    Count = 0;
                else
                    Count -= 3;
            }
            CountTextBox.Text = Count.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
