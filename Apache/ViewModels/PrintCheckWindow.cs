using System.Collections.ObjectModel;
using System.Linq;

namespace Apache.ViewModels
{
    public class PrintCheckWindowViewModel : BaseViewModel
    {
        private SalePageViewModel _model;
        public PrintCheckWindowViewModel(SalePageViewModel model)
        {
            _model = model;
            StaffName = StaticFields.currrentStaff!.Name!;
            CheckNumber = model._currentCheck!.Id.ToString();
            ProductList = new ObservableCollection<SaleProduct>(model.InSaleProductsCollection!);
            DiscountSum = model.InSaleProductsCollection!.Sum(a => (a.Discount * a.Price * a.Quantity / 100));
            WithoutDiscountSum = model.ServicePrice;
            ItogSum = model.InSaleProductsCollection!.Sum(a => a.Itog) + model.ServicePrice;
        }
        public PrintCheckWindowViewModel()
        {

        }

        private string _staffName = string.Empty;
        public string StaffName
        {
            get { return _staffName; }
            set { Set(ref _staffName, value); }
        }

        private string _checkNumber = string.Empty;
        public string CheckNumber
        {
            get { return _checkNumber; }
            set { Set(ref _checkNumber, value); }
        }

        private decimal _discountSum = 0;
        public decimal DiscountSum
        {
            get { return _discountSum; }
            set { Set(ref _discountSum, value); }
        }

        private decimal _withoutDiscountSum = 0;
        public decimal WithoutDiscountSum
        {
            get { return _withoutDiscountSum; }
            set { Set(ref _withoutDiscountSum, value); }
        }

        private decimal _itogSum = 0;
        public decimal ItogSum
        {
            get { return _itogSum; }
            set { Set(ref _itogSum, value); }
        }

        private ObservableCollection<SaleProduct>? _productList;
        public ObservableCollection<SaleProduct>? ProductList
        {
            get { return _productList; }
            set
            {
                Set(ref _productList, value);
            }
        }
    }
}
