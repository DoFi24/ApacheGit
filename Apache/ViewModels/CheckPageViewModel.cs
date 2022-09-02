using Apache.Infrastructure.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Apache.ViewModels
{
    public class CheckPageViewModel : BaseViewModel
    {
        public CheckPageViewModel()
        {
            PreviousPageChecksCommand = new RelayCommand(PreviousPageChecksCommandExecute);
            NextPageChecksCommand = new RelayCommand(NextPageChecksCommandExecute);

            LoadChecks();
        }

        public ICommand PreviousPageChecksCommand { get; set; }
        public ICommand NextPageChecksCommand { get; set; }

        private int _currentPageIndexCheck = 1;
        public int CurrentPageIndexCheck
        {
            get => _currentPageIndexCheck;
            set
            {
                Set(ref _currentPageIndexCheck, value);
            }
        }
        private int _pageCountCheck = 1;
        public int PageCountCheck
        {
            get => _pageCountCheck;
            set
            {
                Set(ref _pageCountCheck, value);
            }
        }

        private ObservableCollection<CheckPageModel>? _allChecksCollection = new();
        public ObservableCollection<CheckPageModel>? AllChecksCollection
        {
            get { return _allChecksCollection; }
            set { Set(ref _allChecksCollection, value); }
        }

        private CheckPageModel? _selectedCheck = new();
        public CheckPageModel? SelectedCheck
        {
            get { return _selectedCheck; }
            set 
            { 
                Set(ref _selectedCheck, value); 
            }
        }


        private void PreviousPageChecksCommandExecute(object o)
        {
            if (CurrentPageIndexCheck == 1)
                return;
            CurrentPageIndexCheck--;
            AllChecksCollection = new ObservableCollection<CheckPageModel>(ReturnChecks());
        }
        private void NextPageChecksCommandExecute(object o)
        {
            if (CurrentPageIndexCheck == PageCountCheck)
            {
                return;
            }
            else if (CurrentPageIndexCheck > PageCountCheck)
            {
                CurrentPageIndexCheck = 1;
                LoadChecks();
                return;
            }
            CurrentPageIndexCheck++;
            AllChecksCollection = new ObservableCollection<CheckPageModel>(ReturnChecks());
        }

        private void LoadChecks()
        {
            CurrentPageIndexCheck = 1;
            AllChecksCollection = new ObservableCollection<CheckPageModel>(ReturnChecks());
        }
        private List<CheckPageModel> ReturnChecks()
        {
            using ApplicationContext db = new ApplicationContext();

            var ostatok = db.Checks!.Select(s => new CheckPageModel { Id = s.Id, TotalSum = s.TotalSum, PrintDate = s.PrintDate, IsActive = s.IsActive == 2 ? "LightPink" : "White" }).ToList();

            var result = ostatok.Skip((CurrentPageIndexCheck - 1) * 12).Take(12).ToList();

            for (int i = 0; i < ostatok.Count; i++)
            {
                result[i].ChecksList = new List<CheckModel>(
                    db.CheckDetails!
                    .Where(s => s.ChecksId == result[i].Id)
                    .Select(s => new CheckModel(
                        db.ProductsPrihod!.First(p => p.Id == s.ProductsPrihodId).ProductsName,
                        s.Quantity,
                        s.Discount,
                        s.Price)));
            }

            PageCountCheck = Convert.ToInt32(Math.Ceiling(ostatok.Count() / 12m));

            return result;
        }
    }
}
