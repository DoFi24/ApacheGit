using Apache.Models;
using Apache.Views.AdminPages;
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
    /// Логика взаимодействия для QuantityWindow.xaml
    /// </summary>
    public partial class QuantityWindow : Window
    {
        private int Count = 1;
        private decimal DisCount = 0;
        private SalePageViewModel _model;
        public QuantityWindow(SalePageViewModel model)
        {
            InitializeComponent();
            _model = model;
            ProductName.Text = _model.InSaleProduct!.Products!.ProductsName;
            Count = _model.InSaleProduct!.Quantity;
            CountTextBox.Text = Count.ToString();
            DisCount = _model.InSaleProduct.Discount;
            DiscountTextBox.Text = DisCount.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void DisClick(object sender, RoutedEventArgs e)
        {
            var coiun = (sender as Button)!.Content;
            switch (coiun)
            {
                case "+1":
                    DisCount += 1;
                    break;
                case "+3":
                    DisCount += 3;
                    break;
                case "+5":
                    DisCount += 5;
                    break;
                case "-1":
                    if (DisCount > 0)
                        DisCount--;
                    break;
                case "-3":
                    if (DisCount < 3)
                        DisCount = 0;
                    else
                        DisCount -= 3;
                    break;
            }
            DiscountTextBox.Text = DisCount.ToString();
        }

        private void CountClick(object sender, RoutedEventArgs e)
        {
            var coiun = (sender as Button)!.Content;
            switch (coiun)
            {
                case "+1":
                    Count += 1;
                    break;
                case "+3":

                    Count += 3;
                    break;
                case "+5":
                    Count += 5;
                    break;
                case "-1":
                    if (Count > 0)
                        Count--;
                    break;
                case "-3":
                    if (Count < 3)
                        Count = 0;
                    else
                        Count -= 3;
                    break;
            }
            CountTextBox.Text = Count.ToString();
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var product = _model!.InSaleProduct!;

            product.Discount = DisCount;
            product.Quantity = Count;

            _model.InSaleProductsCollection!.Remove(_model!.InSaleProduct!);
            if (Count > 0)
            {
                _model.InSaleProductsCollection!.Add(product);
            }
            _model.CalcPriceMeth();
            Close();
        }

        private void DeleteClick(object sender, RoutedEventArgs e)
        {
            _model.InSaleProductsCollection!.Remove(_model!.InSaleProduct!);
            _model.CalcPriceMeth();
            Close();
        }
    }
}
