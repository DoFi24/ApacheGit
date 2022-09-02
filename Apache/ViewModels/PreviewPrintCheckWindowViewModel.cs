using System.Collections.ObjectModel;

namespace Apache.ViewModels
{
    public class PreviewPrintCheckWindowViewModel : BaseViewModel
    {
        public PreviewPrintCheckWindowViewModel(SalePageViewModel vmodel)
        {
            StaffName = StaticFields.currrentStaff!.Name!;
            CheckNumber = vmodel._currentCheck!.Id.ToString();
            ProductList = new ObservableCollection<SaleProduct>(vmodel.InSaleProductsCollection!);
        }
        public PreviewPrintCheckWindowViewModel()
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
