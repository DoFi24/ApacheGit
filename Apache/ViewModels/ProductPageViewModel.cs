using Apache.Infrastructure.Commands;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Apache.ViewModels
{
    public class ProductPageViewModel : BaseViewModel
    {
        public ProductPageViewModel()
        {
            AddProductCommand = new RelayCommand(AddProductCommandExecute);
            UpdateProductCommand = new RelayCommand(UpdateProductCommandExecute);
            DeleteProductCommand = new RelayCommand(DeleteProductCommandExecute);
            PreviousPageProductCommand = new RelayCommand(PreviousPageProductCommandExecute);
            NextPageProductCommand = new RelayCommand(NextPageProductCommandExecute);

            AddCategoryCommand = new RelayCommand(AddCategoryCommandExecute);
            UpdateCategoryCommand = new RelayCommand(UpdateCategoryCommandExecute);
            DeleteCategoryCommand = new RelayCommand(DeleteCategoryCommandExecute);
            PreviousPageCategoryCommand = new RelayCommand(PreviousPageCategoryCommandExecute);
            NextPageCategoryCommand = new RelayCommand(NextPageCategoryCommandExecute);

            LoadCategories();
            LoadAddCategories();
            LoadProducts();
        }

        #region Params

        private int _currentPageIndexProduct = 1;
        private int _pageCountProduct = 1;
        private string _productName = "";

        private int _currentPageIndexCategory = 1;
        private int _pageCountCategory = 1;
        private string _categoryName = "";

        private Products _selectedProduct = new();
        private Categories _selectedCategory = new();

        private Categories _selectedAddCategory = new();

        private ObservableCollection<Products> _productsCollection = new ObservableCollection<Products>();
        private ObservableCollection<Categories> _categoriesCollection = new ObservableCollection<Categories>();

        private ObservableCollection<Categories> _categoriesAddCollection = new ObservableCollection<Categories>();

        public int CurrentPageIndexProduct
        {
            get => _currentPageIndexProduct;
            set
            {
                Set(ref _currentPageIndexProduct, value);
            }
        }
        public int PageCountProduct
        {
            get => _pageCountProduct;
            set
            {
                Set(ref _pageCountProduct, value);
            }
        }
        public string ProductName
        {
            get => _productName;
            set
            {
                Set(ref _productName, value);
            }
        }
        public Products SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                Set(ref _selectedProduct, value);
                if (value is null)
                    return;
                using (ApplicationContext db = new ApplicationContext())
                {
                    ProductName = value.Name!;
                    SelectedCategory = db.Categories!.First(s=>s.Id == value.CategoryId);
                }
            }
        }
        public Categories? SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                Set(ref _selectedCategory, value);
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
        public ObservableCollection<Categories> CategoryCollection
        {
            get => _categoriesCollection;
            set
            {
                Set(ref _categoriesCollection, value);
            }
        }


        public int CurrentPageIndexCategory
        {
            get => _currentPageIndexCategory;
            set
            {
                Set(ref _currentPageIndexCategory, value);
            }
        }
        public int PageCountCategory
        {
            get => _pageCountCategory;
            set
            {
                Set(ref _pageCountCategory, value);
            }
        }
        public string CategoryName
        {
            get => _categoryName;
            set
            {
                Set(ref _categoryName, value);
            }
        }
        public Categories SelectedAddCategory
        {
            get => _selectedAddCategory;
            set
            {
                Set(ref _selectedAddCategory, value);
            }
        }
        public ObservableCollection<Categories> CategoryAddCollection
        {
            get => _categoriesAddCollection;
            set
            {
                Set(ref _categoriesAddCollection, value);
            }
        }

        #endregion

        #region Commands

        //Команды
        public ICommand AddProductCommand { get; set; }
        public ICommand UpdateProductCommand { get; set; }
        public ICommand DeleteProductCommand { get; set; }
        public ICommand AddCategoryCommand { get; set; }
        public ICommand UpdateCategoryCommand { get; set; }
        public ICommand DeleteCategoryCommand { get; set; }

        //пагинация
        public ICommand PreviousPageCategoryCommand { get; set; }
        public ICommand NextPageCategoryCommand { get; set; }
        public ICommand PreviousPageProductCommand { get; set; }
        public ICommand NextPageProductCommand { get; set; }

        #endregion

        #region CommandsRealization
        private async void AddProductCommandExecute(object o)
        {
            if (SelectedCategory is null || SelectedCategory == new Categories())
            {
                MessageBox.Show("Выберите категорию!");
                return;
            }
            try
            {
                await using (ApplicationContext db = new ApplicationContext())
                {
                    db.Products!.Add(new Products { Name = ProductName, CategoryId = SelectedCategory.Id });
                    db.SaveChanges();
                    MessageBox.Show("Успешно");
                    LoadProducts();
                    ProductName = String.Empty;
                    SelectedCategory = null;
                }
            }
            catch
            {
                MessageBox.Show("Ошибка");
            }
        }
        private async void UpdateProductCommandExecute(object o)
        {
            try
            {
                await using (ApplicationContext db = new())
                {
                    var product = db.Products!.First(s => s.Id == SelectedProduct.Id);
                    if (ProductName != product.Name && ProductName!= String.Empty)
                        product.Name = ProductName;
                    if (SelectedCategory!= null && SelectedCategory != new Categories())
                        product.CategoryId = SelectedCategory!.Id;
                    db.SaveChanges();
                    MessageBox.Show("Успешно");
                    LoadProducts();
                    ProductName = String.Empty;
                    SelectedCategory = null;
                }
            }
            catch
            {
                MessageBox.Show("Ошибка");
            }
        }

        private async void DeleteProductCommandExecute(object o)
        {
            try
            {
                await using (ApplicationContext db = new ApplicationContext())
                {
                    db.Products!.Remove(SelectedProduct);
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
        private async void PreviousPageProductCommandExecute(object o)
        {
            await using (ApplicationContext db = new ApplicationContext())
            {
                if (CurrentPageIndexProduct == 1)
                    return;
                CurrentPageIndexProduct--;
                ProductCollection = new ObservableCollection<Products>(db.Products!.Skip((CurrentPageIndexProduct - 1) * 12).Take(12));
            }
        }
        private async void NextPageProductCommandExecute(object o)
        {
            await using (ApplicationContext db = new ApplicationContext())
            {
                if (CurrentPageIndexProduct == PageCountProduct)
                {
                    return;
                }
                else if (CurrentPageIndexProduct > PageCountProduct)
                {
                    CurrentPageIndexProduct = 1;
                    LoadProducts();
                    return;
                }
                CurrentPageIndexProduct++;
                ProductCollection = new ObservableCollection<Products>(db.Products!.Skip((CurrentPageIndexProduct - 1) * 12).Take(12));
            }
        }

        #endregion
        private async void AddCategoryCommandExecute(object o)
        {
            try
            {
                await using (ApplicationContext db = new ApplicationContext())
                {
                    db.Categories!.Add(new Categories { Name = CategoryName });
                    db.SaveChanges();
                    MessageBox.Show("Успешно");
                    LoadAddCategories();
                    CategoryName = string.Empty;
                }
            }
            catch
            {
                MessageBox.Show("Ошибка");
            }
        }
        private async void UpdateCategoryCommandExecute(object o)
        {
            try
            {
                await using (ApplicationContext db = new())
                {
                    var product = db.Categories!.First(s => s.Id == SelectedAddCategory.Id);
                    if (CategoryName != product.Name && CategoryName!= String.Empty)
                        product.Name = CategoryName;
                    db.SaveChanges();
                    MessageBox.Show("Успешно");
                    LoadAddCategories(); 
                    CategoryName = string.Empty;
                }
            }
            catch
            {
                MessageBox.Show("Ошибка");
            }
        }
        private async void DeleteCategoryCommandExecute(object o)
        {
            try
            {
                await using (ApplicationContext db = new ApplicationContext())
                {
                    db.Categories!.Remove(SelectedAddCategory);
                    db.SaveChanges();
                    MessageBox.Show("Успешно");
                    LoadAddCategories();
                }
            }
            catch
            {
                MessageBox.Show("Ошибка");
            }
        }
        private async void PreviousPageCategoryCommandExecute(object o)
        {
            await using (ApplicationContext db = new ApplicationContext())
            {
                if (CurrentPageIndexCategory == 1)
                    return;
                CurrentPageIndexCategory--;
                CategoryAddCollection = new ObservableCollection<Categories>(db.Categories!.Skip((CurrentPageIndexCategory - 1) * 12).Take(12));
            }
        }
        private async void NextPageCategoryCommandExecute(object o)
        {
            await using (ApplicationContext db = new ApplicationContext())
            {
                if (CurrentPageIndexCategory == PageCountCategory)
                {
                    return;
                }
                else if (CurrentPageIndexCategory > PageCountCategory)
                {
                    CurrentPageIndexCategory = 1;
                    LoadAddCategories();
                    return;
                }
                CurrentPageIndexCategory++;
                CategoryAddCollection = new ObservableCollection<Categories>(db.Categories!.Skip((CurrentPageIndexCategory - 1) * 12).Take(12));
            }
        }
        private async void LoadProducts()
        {
            await using (ApplicationContext db = new())
            {
                var products = db.Products;
                ProductCollection = new ObservableCollection<Products>(products!.Take(12));
                PageCountProduct = Convert.ToInt32(Math.Ceiling(products!.Count() / 12m));
                CurrentPageIndexProduct = 1;
            }
        }
        private async void LoadCategories()
        {
            await using (ApplicationContext db = new())
            {
                CategoryCollection = new ObservableCollection<Categories>(db.Categories!);
            }
        }
        private async void LoadAddCategories()
        {
            await using (ApplicationContext db = new())
            {
                var categories = db.Categories;
                CategoryAddCollection = new ObservableCollection<Categories>(categories!.Take(12));
                PageCountCategory = Convert.ToInt32(Math.Ceiling(categories!.Count() / 12m));
                CurrentPageIndexCategory = 1;
            }
        }
    }
}
