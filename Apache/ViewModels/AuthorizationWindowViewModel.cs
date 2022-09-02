using Apache.Infrastructure.Commands;
using Apache.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apache.ViewModels
{
    public class AuthorizationWindowViewModel : BaseViewModel
    {
        private AuthorizationWindow _window;
        public AuthorizationWindowViewModel(AuthorizationWindow window)
        {
            _window = window;
        }

        private string? _pinCode;
        public string? PinCode
        {
            get { return _pinCode; }
            set { Set(ref _pinCode, value); }
        }

        private string? _errorMessage;
        public string? ErrorMessage
        {
            get { return _errorMessage; }
            set { Set(ref _errorMessage, value); }
        }

        private RelayCommand? _PutNumberCommand;
        public RelayCommand? PutNumberCommand =>
            _PutNumberCommand ??= new RelayCommand(ExecuteUDSSettingsCommand, (object obj) => { return true; });
        private void ExecuteUDSSettingsCommand(object obj)
        {
            ErrorMessage = string.Empty;

            switch (obj) 
            {
                case "-":
                    if (PinCode!.Length > 0)
                        PinCode = PinCode.Remove(PinCode.Length - 1, 1);
                    break;
                case "1":
                    PinCode += obj;
                    break;
                case "2":
                    PinCode += obj;
                    break;
                case "3":
                    PinCode += obj;
                    break;
                case "4":
                    PinCode += obj;
                    break;
                case "5":
                    PinCode += obj;
                    break;
                case "6":
                    PinCode += obj;
                    break;
                case "7":
                    PinCode += obj;
                    break;
                case "8":
                    PinCode += obj;
                    break;
                case "9":
                    PinCode += obj;
                    break;
                case "0":
                    PinCode += obj;
                    break;
                default:
                Authorization();
                    break;

            }
        }
        private void Authorization()
        {
            using ApplicationContext context = new ApplicationContext();
            var staff = context.Staffs!.FirstOrDefault(s => s.PinCode == PinCode);
            if (staff is null || staff == new Models.Staffs())
            {
                ErrorMessage = "Неправильный пароль!";
                return;
            }
            StaticFields.currrentStaff = staff;
            StaticFields.currentServicePrice = Convert.ToDecimal(context.Settings!.First(s=>s.Key == "ServiceDiscount").Value);
            switch (staff.Role)
            {
                case 1: 
                    new AdminWindow().Show();
                    break;
                case 2:
                    new MainWindow().Show();
                    break;
            }
            _window.Close();
        } 
    }
}
