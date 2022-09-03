using Apache.Infrastructure.Commands;
using Apache.Views;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Apache.ViewModels
{
    public class SalePageViewModel : BaseViewModel
    {
        public MainWindowViewModel? _viewModel;
        private Places? _currentPlace;
        public Checks? _currentCheck;
        public SalePageViewModel(MainWindowViewModel viewModel, Places places, Checks? currentCheck = null)
        {
            _viewModel = viewModel;
            _currentPlace = places;

            DeleteCheckCommand = new RelayCommand(DeleteCheckCommandExecute);
            SaveInBaseCommand = new RelayCommand(SaveInBaseCommandExecute);
            PrintPreviewCheckCommand = new RelayCommand(PrintPreviewCheckCommandExecute);
            ChangeTableCommand = new RelayCommand(ChangeTableCommandExecute);
            EndPaymandCommand = new RelayCommand(EndPaymendCommandExecute);
            ShowCategoryCommand = new RelayCommand(ShowCategoryCommandExecute);

            GetAllCategorises();

            if (currentCheck is not null)
            {
                this._currentCheck = currentCheck;
                GetFromCheck();
            }
        }
        public SalePageViewModel()
        {

        }

        #region Commands
        public ICommand DeleteCheckCommand { get; set; }
        public ICommand SaveInBaseCommand { get; set; }
        public ICommand PrintPreviewCheckCommand { get; set; }
        public ICommand ChangeTableCommand { get; set; }
        public ICommand EndPaymandCommand { get; set; }
        public ICommand ShowCategoryCommand { get; set; }



        #endregion

        #region Props

        private decimal _endPrice = 0;
        public decimal EndPrice
        {
            get { return _endPrice; }
            set { Set(ref _endPrice, value); }
        }

        private decimal _servicePrice = 0;
        public decimal ServicePrice
        {
            get { return _servicePrice; }
            set { Set(ref _servicePrice, value); }
        }

        private decimal _itogPrice = 0;
        public decimal ItogPrice
        {
            get { return _itogPrice; }
            set { Set(ref _itogPrice, value); }
        }

        private Visibility? _categoryVisibile = Visibility.Visible;
        public Visibility? CategoryVisibile
        {
            get { return _categoryVisibile; }
            set
            {
                Set(ref _categoryVisibile, value);
            }
        }

        private Visibility? _productVisibile = Visibility.Hidden;
        public Visibility? ProductVisibile
        {
            get { return _productVisibile; }
            set
            {
                Set(ref _productVisibile, value);
            }
        }

        private Categories? _selectedCategory = new();
        public Categories? SelectedCategory
        {
            get { return _selectedCategory; }
            set
            {
                Set(ref _selectedCategory, value);

                if (value != null && value != new Categories())
                    ShowProductsInCategory(value);
            }
        }

        private ObservableCollection<Categories>? _allCategoriesCollection = new();
        public ObservableCollection<Categories>? AllCategoriesCollection
        {
            get { return _allCategoriesCollection; }
            set { Set(ref _allCategoriesCollection, value); }
        }

        private ProductsPrihod? _selectedProduct = new();
        public ProductsPrihod? SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                Set(ref _selectedProduct, value);
                if (value is null || value == new ProductsPrihod())
                    return;

                if (!InSaleProductsCollection!.Any(s => s.Id == value.Id))
                    InSaleProductsCollection!.Add(new SaleProduct
                    {
                        Id = value.Id,
                        Discount = 0,
                        Price = value.Price,
                        Products = value,
                        Quantity = 1
                    });
                else
                {
                    var product = InSaleProductsCollection!.First(s => s.Id == value.Id);
                    if (string.IsNullOrWhiteSpace(product.Products!.ProductsId.ToString())) return;
                    if (product.Quantity < value.Quantity)
                    {
                        product.Quantity++;
                        InSaleProductsCollection!.Remove(product);
                        InSaleProductsCollection.Add(product);
                    }
                }
                CalcPriceMeth();
            }
        }

        private ObservableCollection<ProductsPrihod>? _allProductsCollection = new();
        public ObservableCollection<ProductsPrihod>? AllProductsCollection
        {
            get { return _allProductsCollection; }
            set { Set(ref _allProductsCollection, value); }
        }

        private ObservableCollection<SaleProduct>? _inSaleProductsCollection = new();
        public ObservableCollection<SaleProduct>? InSaleProductsCollection
        {
            get { return _inSaleProductsCollection; }
            set
            {
                Set(ref _inSaleProductsCollection, value);
            }
        }

        private SaleProduct? _inSaleProduct = new();
        public SaleProduct? InSaleProduct
        {
            get { return _inSaleProduct; }
            set
            {
                Set(ref _inSaleProduct, value);
                if (value is null || value == new SaleProduct())
                    return;
                new QuantityWindow(this).ShowDialog();
            }
        }

        #endregion

        #region MethCommand
        public async void DeleteCheckCommandExecute(object o)
        {
            await using (ApplicationContext db = new())
            {
                if (_currentCheck is not null)
                {
                    var check = db.Checks!.First(s => s.Id == _currentCheck!.Id);
                    check.IsActive = 2;
                    db.SaveChanges();
                }
                _viewModel!.ExecuteShowTablePageCommand("");
            }
        }
        public void SaveInBaseCommandExecute(object o)
        {
            if (InSaleProductsCollection!.Any())
            {
                SaveProducsInBase(0);
                _viewModel!.ExecuteShowTablePageCommand("");
            }
            else
                MessageBox.Show("Выберите товары для сохранения!");
        }
        public void PrintPreviewCheckCommandExecute(object o)
        {
            if (InSaleProductsCollection!.Any())
                SaveProducsInBase(0);
            else
                MessageBox.Show("Выберите товары для сохранения!");
            new PreviewPrintCheckWindow(this).ShowDialog();
        }
        public void ChangeTableCommandExecute(object o)
        {
            if (_currentPlace is not null)
                new ChangeTableWindow(this).ShowDialog();
            else
                MessageBox.Show("Сохраните чек перед сменой стола!");
        }
        public void ShowCategoryCommandExecute(object o)
        {
            ChangeVisibile(false);
        }
        public void EndPaymendCommandExecute(object o)
        {
            if (InSaleProductsCollection!.Any())
            {
                SaveProducsInBase(1);
                DeleteProduct();
            }
            else
                MessageBox.Show("Выберите товары для сохранения!");
            new PrintCheckWindow(this).ShowDialog();
        }

        private async void SaveProducsInBase(int index)
        {
            await using (ApplicationContext db = new ApplicationContext())
            {
                if (_currentCheck is null)
                {
                    db.Checks!.Add(
                        new Checks
                        {
                            IsActive = index,
                            StaffsId = StaticFields.currrentStaff.Id,
                            TotalSum = ItogPrice,
                            PlacesId = _currentPlace!.Id,
                            PrintDate = System.DateTime.UtcNow,
                        });
                    db.SaveChanges();

                    _currentCheck = db.Checks.OrderBy(a => a.Id).Last();

                    db.CheckDetails!.AddRange(InSaleProductsCollection!
                            .Select(s => new CheckDetails
                            {
                                ChecksId = _currentCheck.Id,
                                Discount = s.Discount,
                                Price = s.Price,
                                ProductsPrihodId = s.Products!.Id,
                                Quantity = s.Quantity,
                            }));
                }
                else
                {
                    var check = db.Checks!.First(s => s.Id == _currentCheck.Id);
                    check.StaffsId = StaticFields.currrentStaff.Id;
                    check.IsActive = index;
                    check.TotalSum = ItogPrice;
                    check.PlacesId = _currentPlace!.Id;
                    check.PrintDate = System.DateTime.UtcNow;

                    db.CheckDetails!.RemoveRange(db.CheckDetails.Where(cd => cd.ChecksId == check.Id));
                    db.SaveChanges();
                    db.CheckDetails!.AddRange(InSaleProductsCollection!
                            .Select(s => new CheckDetails
                            {
                                ChecksId = _currentCheck.Id,
                                Discount = s.Discount,
                                Price = s.Price,
                                ProductsPrihodId = s.Products!.Id,
                                Quantity = s.Quantity,
                            }));
                }
                db.SaveChanges();
            }
        }
        private async void DeleteProduct()
        {
            await using (ApplicationContext db = new())
            {
                foreach (var item in InSaleProductsCollection!)
                {
                    var product = db.ProductsPrihod!.First(s => s.Id == item.Id);
                    product.ComeQuantity -= item.Quantity;
                }
                db.SaveChanges();
            }
        }

        private void GetFromCheck()
        {
            ApplicationContext db = new ApplicationContext();
            var products = db.CheckDetails!.Where(s => s.ChecksId == _currentCheck!.Id).ToList();
            foreach (var item in products)
            {
                InSaleProductsCollection!.Add(new SaleProduct
                {
                    Id = item.ProductsPrihodId,
                    Discount = item.Discount,
                    Price = item.Price,
                    Products = db.ProductsPrihod!.First(p => p.Id == item.ProductsPrihodId),
                    Quantity = item.Quantity
                });
            }
            CalcPriceMeth();
        }
        public void CalcPriceMeth()
        {
            if (InSaleProductsCollection!.Any())
            {
                EndPrice = InSaleProductsCollection!.Sum(s => s.Itog);
                ServicePrice = EndPrice / 100 * StaticFields.currentServicePrice;
                ItogPrice = EndPrice + ServicePrice;
            }
            else
            {
                EndPrice = 0;
                ServicePrice = 0;
                ItogPrice = 0;
            }
        }
        private void GetAllCategorises()
        {
            using ApplicationContext db = new();
            AllCategoriesCollection = new ObservableCollection<Categories>(db.Categories!.OrderBy(s => s.Name));
        }
        private void ShowProductsInCategory(Categories category)
        {
            using (ApplicationContext db = new())
            {
                AllProductsCollection = new ObservableCollection<ProductsPrihod>(db.ProductsPrihod!
                    .Where(s => db.Products!.First(p => p.Id == s.ProductsId).CategoryId == category.Id));
            }
            ChangeVisibile(true);
        }

        /// <summary>
        /// True если надо показать товар и скрыть категории
        /// </summary>
        /// <param name="i"></param>
        private void ChangeVisibile(bool i)
        {
            if (i)
            {
                CategoryVisibile = Visibility.Hidden;
                ProductVisibile = Visibility.Visible;
            }
            else
            {
                CategoryVisibile = Visibility.Visible;
                ProductVisibile = Visibility.Hidden;
            }
        }

        #endregion
    }
}
