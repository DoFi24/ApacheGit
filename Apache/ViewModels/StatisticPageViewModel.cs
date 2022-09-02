using Apache.Infrastructure.Commands;
using Apache.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Apache.ViewModels
{
    public class StatisticPageViewModel : BaseViewModel
    {
        public StatisticPageViewModel()
        {
            SearchCommand = new RelayCommand(SearchCommandExecute);
        }
        private DateTime? _selectedOneDate;
        public DateTime? SelectedOneDate
        {
            get { return _selectedOneDate; }
            set { Set(ref _selectedOneDate, value); }
        }

        private DateTime? _selectedDateStart;
        public DateTime? SelectedDateStart
        {
            get { return _selectedDateStart; }
            set { Set(ref _selectedDateStart, value); }
        }
        private DateTime? _selectedDateEnd;
        public DateTime? SelectedDateEnd
        {
            get { return _selectedDateEnd; }
            set { Set(ref _selectedDateEnd, value); }
        }

        private ObservableCollection<string>? _dayWeekMonthSelectionCollection = new();
        public ObservableCollection<string>? DayWeekMonthSelectionCollection
        {
            get { return _dayWeekMonthSelectionCollection; }
            set { Set(ref _dayWeekMonthSelectionCollection, value); }
        }
        private string _selectedDWM;
        public string SelectedDWM
        {
            get { return _selectedDWM; }
            set { Set(ref _selectedDWM, value); }
        }
        public ICommand SearchCommand { get; set; }
        public async void SearchCommandExecute(object o)
        {
            
        }
    }
}
