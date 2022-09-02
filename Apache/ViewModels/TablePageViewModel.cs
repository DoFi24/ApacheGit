using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apache.ViewModels
{
    public class TablePageViewModel : BaseViewModel
    {
        private MainWindowViewModel? _viewModel;
        public TablePageViewModel(MainWindowViewModel viewModel)
        {
            _viewModel = viewModel;
            GetAllPlaces();
        }
        public TablePageViewModel()
        {

        }

        private Places? _selectedPlace = new();
        public Places? SelectedPlace
        {
            get { return _selectedPlace; }
            set 
            { 
                Set(ref _selectedPlace, value);
                if (value != null && value != new Places())
                    _viewModel!.ShowSalePage(value!);
            }
        }

        private ObservableCollection<Places>? _allPlacesCollection;
        public ObservableCollection<Places>? AllPlacesCollection
        {
            get { return _allPlacesCollection; }
            set { Set(ref _allPlacesCollection, value); }
        }

        private void GetAllPlaces()
        {
            using ApplicationContext db = new ApplicationContext();

            AllPlacesCollection = new ObservableCollection<Places>(db.Places!);
        }
    }
}
