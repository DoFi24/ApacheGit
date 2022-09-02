using Apache.Infrastructure.Commands;
using Apache.Views;
using System.Windows.Controls;
using System.Linq;

namespace Apache.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        public MainWindow? _window;
        public MainWindowViewModel(MainWindow window)
        {
            _window = window;
            _window.NavigationFrame.Navigate(new TablePage(this));
            StaffName = StaticFields.currrentStaff!.Name!;
        }
        public MainWindowViewModel()
        {

        }

        #region Props

        private string _staffName = string.Empty;
        public string StaffName
        {
            get { return _staffName; }
            set { Set(ref _staffName, value); }
        }

        private string _tableName = "Столы";
        public string TableName
        {
            get { return _tableName; }
            set { Set(ref _tableName, value); }
        }

        #endregion

        #region Commands

        private RelayCommand? _LockWindowCommand;
        public RelayCommand? LockWindowCommand =>
            _LockWindowCommand ??= new RelayCommand(ExecuteLockWindowCommand, (object obj) => { return true; });
        private void ExecuteLockWindowCommand(object obj)
        {
            new AuthorizationWindow().Show();
            _window!.Close();
        }

        private RelayCommand? _ShowTablePageCommand;
        public RelayCommand? ShowTablePageCommand =>
            _ShowTablePageCommand ??= new RelayCommand(ExecuteShowTablePageCommand, (object obj) => { return true; });
        public void ExecuteShowTablePageCommand(object obj)
        {
            if ((_window!.NavigationFrame.Content as Page)!.Title == nameof(TablePage))
                return;

            _window.NavigationFrame.Navigate(new TablePage(this));
            TableName = "Столы";
        }

        public void ShowSalePage(Places table)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var check = db.Checks!.Where(a => a.PlacesId == table.Id && a.IsActive == 0);
                if (check.Any())
                {
                    _window!.NavigationFrame.Navigate(new SalePage(this, table, check.First()));
                }
                else
                {
                    _window!.NavigationFrame.Navigate(new SalePage(this, table));
                }
                TableName = table.Name!;
            }
        }
        #endregion
    }
}
