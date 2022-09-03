using Apache.Infrastructure.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Apache.ViewModels
{
    public class StatisticPageViewModel : BaseViewModel
    {
        public ICommand PreviousPageProductCommand { get; set; }
        public ICommand NextPageProductCommand { get; set; }


        private int _currentPageIndex = 1;
        private int _pageCount = 1;

        public int CurrentPageIndex
        {
            get => _currentPageIndex;
            set
            {
                Set(ref _currentPageIndex, value);
            }
        }
        public int PageCount
        {
            get => _pageCount;
            set
            {
                Set(ref _pageCount, value);
            }
        }

        public StatisticPageViewModel()
        {

            SearchCommand = new RelayCommand(SearchCommandExecute);
            PreviousPageProductCommand = new RelayCommand(PreviousPageProductCommandExecute);
            NextPageProductCommand = new RelayCommand(NextPageProductCommandExecute);


            DefaultValues();
            
            AllLoad();

        }

        private void NextPageProductCommandExecute(object obj)
        {
            if (CurrentPageIndex == PageCount)
            {
                return;
            }
            else if (CurrentPageIndex > PageCount)
            {
                CurrentPageIndex = 1;
                VievStatisticCollection = new List<ProductsStatistic>(StatisticCollection!.Skip(0).Take(12));

                return;
            }
            CurrentPageIndex++;
            VievStatisticCollection = new List<ProductsStatistic>(StatisticCollection!.Skip((CurrentPageIndex - 1) * 12).Take(12));
        }

        private void PreviousPageProductCommandExecute(object obj)
        {
            if (CurrentPageIndex == 1)
                return;
            CurrentPageIndex--;
            VievStatisticCollection = new List<ProductsStatistic>(StatisticCollection!.Skip((CurrentPageIndex - 1) * 12).Take(12));

        }

        public void DefaultValues() 
        {
            SelectedOneDate = DateTime.Now;

            SetComboboxItems();
        }

        public void SetComboboxItems() 
        {
            DaysSettings.Add("1 день назад");
            DaysSettings.Add("7 дней назад");
            DaysSettings.Add("30 дней назад");

            SelectedDWM = DaysSettings[0];
        }

        public async void AllLoad() 
        {
            List<Products> products = new List<Products>();

            await Task.Run(() =>
            {
                using(var db = new ApplicationContext()) 
                {
                    products = db.Products.ToList();
                }
            });
            StatisticCollection.AddRange(products.Select(o => new ProductsStatistic { ProductId = o.Id, ProductName = o.Name }));

        }

        private bool _dateInterval = false;
        public bool DateInterval 
        {
            get => _dateInterval;
            set
            {
                Set(ref _dateInterval, value);
            }
        }


        private DateTime _selectedOneDate;
        public DateTime SelectedOneDate
        {
            get { return _selectedOneDate; }
            set { Set(ref _selectedOneDate, value); }
        }

        private DateTime _selectedDateStart = DateTime.Now.AddDays(-30);
        public DateTime SelectedDateStart
        {
            get { return _selectedDateStart; }
            set { Set(ref _selectedDateStart, value); }
        }
        private DateTime _selectedDateEnd = DateTime.Now.AddDays(1);
        public DateTime SelectedDateEnd
        {
            get { return _selectedDateEnd; }
            set { Set(ref _selectedDateEnd, value); }
        }

        private List<string> _daysSettings = new();
        public List<string> DaysSettings 
        {
            get => _daysSettings;
            set => Set(ref _daysSettings, value);
        }

        private List<ProductsStatistic> _vievStatisticCollection = new();
        public List<ProductsStatistic> VievStatisticCollection
        {
            get
            {
                return _vievStatisticCollection;
            }
            set
            {
                Set(ref _vievStatisticCollection, value);
            }
        }

        private List<ProductsStatistic> _statisticCollection = new();
        public List<ProductsStatistic> StatisticCollection
        {
            get 
            { 
                return _statisticCollection; 
            }
            set 
            { 
                Set(ref _statisticCollection, value); 
            }
        }
        private string _selectedDWM;
        public string SelectedDWM
        {
            get => _selectedDWM;
            set
            { 
                Set(ref _selectedDWM, value);
            }
        }
        public ICommand SearchCommand { get; set; }
        public async void SearchCommandExecute(object o)
        {

            if (DateInterval) 
                GetDateInterval();
            else
                GetSelectedDWM();

            PageCount = Convert.ToInt32( StatisticCollection.Count / 12);

        }

        public void GetDateInterval() 
        {
            GetValues(SelectedDateStart, SelectedDateEnd);
        }

        public void GetSelectedDWM()
        {

            int DWMvalue = int.Parse(SelectedDWM[0].ToString());
            DateTime DWMstartDay = SelectedOneDate.AddDays(DWMvalue * -1);

            GetValues(DWMstartDay, SelectedOneDate);
        }

        public async void GetValues(DateTime start, DateTime end) 
        {
            foreach (var i in StatisticCollection)
            {
                i.ProductsSold = 0;
                i.ProductsSoldPrice = 0;
            }

            List<long> checks = new List<long>();

            using (var db = new ApplicationContext())
            {

                checks = db.Checks.Where(t => t.IsActive == 1 && t.PrintDate < end && t.PrintDate > start).Select(t => t.Id).ToList();

                var checkDetails = db.CheckDetails
                    .Where(t => checks.Contains(t.ChecksId))
                    .Select(o => new CheckDetailsWithProductId
                    {
                        Quantity = o.Quantity,
                        Price = o.Price,
                        ProductsPrihodId = o.ProductsPrihodId
                    })
                    .ToList();

                var prihodsIdArray = checkDetails.Select(o => o.ProductsPrihodId).ToArray();

                var prihods = db.ProductsPrihod.Where(t => prihodsIdArray.Contains(t.Id));

                foreach (var i in checkDetails)
                {

                    i.ProductId = prihods?.FirstOrDefault(t => t.Id == i.ProductsPrihodId)?.ProductsId ?? 0L;

                    var product = StatisticCollection.FirstOrDefault(t => t.ProductId == i.ProductId);

                    if (product != null)
                    {
                        product.ProductsSold += i.Quantity;
                        product.ProductsSoldPrice += i.Price;
                    }

                }
                StatisticCollection = new List<ProductsStatistic>(StatisticCollection.OrderBy(t=>t.ProductsSold));
            }

        }
    }
}
