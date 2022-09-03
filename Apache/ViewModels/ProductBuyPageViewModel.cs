using Apache.Infrastructure.Commands;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Apache.ViewModels
{
    public class ProductBuyPageViewModel : BaseViewModel
    {
        public ProductBuyPageViewModel()
        {
            AddProductPrihodCommand = new RelayCommand(AddProductPrihodCommandExecute);
            DeleteProductPrihodCommand = new RelayCommand(DeleteProductPrihodCommandExecute);
            PreviousPageProductPrihodCommand = new RelayCommand(PreviousPageProductPrihodCommandExecute);
            NextPageProductPrihodCommand = new RelayCommand(NextPageProductPrihodCommandExecute);

            LoadProductsBuy();
            LoadProducts();
        }

        #region Params

        private int _currentPageIndexProductBuy = 1;
        private int _pageCountProductBuy = 1;
        private int _productBuyCount = 1;
        private int _productBuyPrice = 0;

        private Products _selectedProduct = new();
        private ProductsPrihod _selectedProductPrihod = new();

        private ObservableCollection<ProductsPrihod> _productsPrihodCollection = new ObservableCollection<ProductsPrihod>();
        private ObservableCollection<Products> _productsCollection = new ObservableCollection<Products>();

        public int CurrentPageIndexProductBuy
        {
            get => _currentPageIndexProductBuy;
            set
            {
                Set(ref _currentPageIndexProductBuy, value);
            }
        }
        public int PageCountProductBuy
        {
            get => _pageCountProductBuy;
            set
            {
                Set(ref _pageCountProductBuy, value);
            }
        }
        public int ProductBuyCount
        {
            get => _productBuyCount;
            set
            {
                Set(ref _productBuyCount, value);
            }
        }
        public int ProductBuyPrice
        {
            get => _productBuyPrice;
            set
            {
                Set(ref _productBuyPrice, value);
            }
        }
        public Products SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                Set(ref _selectedProduct, value);

            }
        }
        public ProductsPrihod SelectedProductPrihod
        {
            get => _selectedProductPrihod;
            set
            {
                Set(ref _selectedProductPrihod, value);
            }
        }
        public ObservableCollection<Products> ProductCollection
        {
            get => _productsCollection;
            set
            {
                Set(ref _productsCollection, value);
            }
        }
        public ObservableCollection<ProductsPrihod> ProductsPrihodCollection
        {
            get => _productsPrihodCollection;
            set
            {
                Set(ref _productsPrihodCollection, value);
            }
        }

        #endregion

        #region Commands

        //Команды
        public ICommand AddProductPrihodCommand { get; set; }
        public ICommand DeleteProductPrihodCommand { get; set; }
        //пагинация
        public ICommand PreviousPageProductPrihodCommand { get; set; }
        public ICommand NextPageProductPrihodCommand { get; set; }

        #endregion

        #region CommandsRealization

        private async void AddProductPrihodCommandExecute(object o)
        {
            if (SelectedProduct is null || SelectedProduct == new Products())
            {
                MessageBox.Show("Выберите продукт!");
                return;
            }
            try
            {
                await using (ApplicationContext db = new ApplicationContext())
                {
                    db.ProductsPrihod!.Add(
                        new ProductsPrihod
                        {
                            PrihodDate = System.DateTime.Now,
                            Price = ProductBuyPrice,
                            ProductsId = SelectedProduct.Id,
                            Quantity = ProductBuyCount,
                            ComeQuantity = ProductBuyCount,
                            ProductsName = SelectedProduct!.Name!
                        });
                    db.SaveChanges();
                    MessageBox.Show("Успешно");
                    LoadProducts();
                    ProductBuyCount = 1;
                    ProductBuyPrice = 0;
                }
            }
            catch
            {
                MessageBox.Show("Ошибка");
            }
        }

        private async void DeleteProductPrihodCommandExecute(object o)
        {
            try
            {
                await using (ApplicationContext db = new ApplicationContext())
                {
                    db.ProductsPrihod!.Remove(SelectedProductPrihod);
                    db.SaveChanges();
                    MessageBox.Show("Успешно");
                    LoadProducts();
                }
            }
            catch
            {
                MessageBox.Show("Ошибка");
            }
        }
        private async void PreviousPageProductPrihodCommandExecute(object o)
        {
            await using (ApplicationContext db = new ApplicationContext())
            {
                if (CurrentPageIndexProductBuy == 1)
                    return;
                CurrentPageIndexProductBuy--;
                ProductsPrihodCollection = new ObservableCollection<ProductsPrihod>(db.ProductsPrihod!.Skip((CurrentPageIndexProductBuy - 1) * 12).Take(12));
            }
        }
        private async void NextPageProductPrihodCommandExecute(object o)
        {
            await using (ApplicationContext db = new ApplicationContext())
            {
                if (CurrentPageIndexProductBuy == PageCountProductBuy)
                {
                    return;
                }
                else if (CurrentPageIndexProductBuy > PageCountProductBuy)
                {
                    CurrentPageIndexProductBuy = 1;
                    LoadProducts();
                    return;
                }
                CurrentPageIndexProductBuy++;
                ProductsPrihodCollection = new ObservableCollection<ProductsPrihod>(db.ProductsPrihod!.Skip((CurrentPageIndexProductBuy - 1) * 12).Take(12));
            }
        }

        #endregion
        private async void LoadProducts()
        {
            await using (ApplicationContext db = new())
            {
                ProductCollection = new ObservableCollection<Products>(db.Products!);
            }
        }

        private async void LoadProductsBuy()
        {
            await using (ApplicationContext db = new())
            {
                ProductsPrihodCollection = new ObservableCollection<ProductsPrihod>(db.ProductsPrihod!.Take(12));
                PageCountProductBuy = Convert.ToInt32(Math.Ceiling(db.ProductsPrihod!.Count() / 12m));
            }
        }

        private void ReturnPrihods() 
        {
        
        }
    }
}
