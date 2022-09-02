using Apache.Infrastructure.Commands;
using Apache.Views.AdminPages;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Apache.ViewModels
{
    public class AccountPageViewModel : BaseViewModel
    {
        public AccountPageViewModel()
        {
            AddUsersCommand = new RelayCommand(AddUsersCommandExecute);
            UpdateUsersCommand = new RelayCommand(UpdateUsersCommandExecute);
            DeleteUsersCommand = new RelayCommand(DeleteUsersCommandExecute);
            PreviousPageUsersCommand = new RelayCommand(PreviousPageClientsCommandExecute);
            NextPageUsersCommand = new RelayCommand(NextPageClientsCommandExecute);

            RolesCollection.Add("1 Админ");
            RolesCollection.Add("2 Кассир");
            LoadUsers();
        }

        #region Params

        private int _currentPageIndexUser = 1;
        private int _pageCountUser = 0;
        private string _password = "";
        private string _userName = "";

        private Staffs _selectedUser = new Staffs();
        private string _selectedRole = string.Empty;
        private ObservableCollection<Staffs> _usersCollection = new ObservableCollection<Staffs>();
        private ObservableCollection<string> _rolesCollection = new ObservableCollection<string>();

        public int CurrentPageIndexUser
        {
            get => _currentPageIndexUser;
            set
            {
                Set(ref _currentPageIndexUser, value);
            }
        }
        public int PageCountUser
        {
            get => _pageCountUser;
            set
            {
                Set(ref _pageCountUser, value);
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                Set(ref _password, value);
            }
        }
        public string UserName
        {
            get => _userName;
            set
            {
                Set(ref _userName, value);
            }
        }
        public string SelectedRole
        {
            get => _selectedRole;
            set
            {
                Set(ref _selectedRole, value);
            }
        }
        public Staffs SelectedUser
        {
            get => _selectedUser;
            set
            {
                Set(ref _selectedUser, value);
                if (value is null || value == new Staffs())
                    return;
                UserName = value!.Name!;
                Password = value!.PinCode!;
                SelectedRole = RolesCollection[value.Role - 1];
            }
        }
        public ObservableCollection<Staffs> UsersCollection
        {
            get => _usersCollection;
            set
            {
                Set(ref _usersCollection, value);

            }
        }
        public ObservableCollection<string> RolesCollection
        {
            get => _rolesCollection;
            set
            {
                Set(ref _rolesCollection, value);
            }
        }

        #endregion

        #region Commands

        //Команды
        public ICommand AddUsersCommand { get; set; }
        public ICommand UpdateUsersCommand { get; set; }
        public ICommand DeleteUsersCommand { get; set; }

        //пагинация
        public ICommand PreviousPageUsersCommand { get; set; }
        public ICommand NextPageUsersCommand { get; set; }

        #endregion

        #region CommandsRealization
        private async void AddUsersCommandExecute(object o)
        {
            if (string.IsNullOrWhiteSpace(SelectedRole))
            {
                MessageBox.Show("Выберите роль!");
                return;
            }
            try
            {
                await using (ApplicationContext db = new ApplicationContext())
                {
                    db.Staffs!.Add(new Staffs { Name = UserName, PinCode = Password, Role = int.Parse(SelectedRole.Split()[0]) });
                    db.SaveChanges();
                    MessageBox.Show("Успешно");
                    LoadUsers();
                }
            }
            catch
            {
                MessageBox.Show("Ошибка");
            }
        }
        private async void UpdateUsersCommandExecute(object o)
        {
            try
            {
                await using (ApplicationContext db = new())
                {
                    var product = db.Staffs!.First(s => s.Id == SelectedUser.Id);
                    if (UserName != product.Name)
                        product.Name = UserName;
                    if (Password != product.PinCode)
                        product.PinCode = Password;
                    if (SelectedRole != product.Role.ToString())
                        product.Role = int.Parse(SelectedRole.Split()[0]);
                    db.SaveChanges();
                    MessageBox.Show("Успешно");
                    LoadUsers();
                }
            }
            catch
            {
                MessageBox.Show("Ошибка");
            }
        }

        private async void DeleteUsersCommandExecute(object o)
        {
            try
            {
                await using (ApplicationContext db = new ApplicationContext())
                {
                    db.Staffs!.Remove(SelectedUser);
                    db.SaveChanges();
                    MessageBox.Show("Успешно");
                    LoadUsers();
                }
            }
            catch
            {
                MessageBox.Show("Ошибка");
            }
        }
        private async void PreviousPageClientsCommandExecute(object o)
        {
            await using (ApplicationContext db = new ApplicationContext())
            {
                if (CurrentPageIndexUser == 1)
                    return;
                CurrentPageIndexUser--;
                UsersCollection = new ObservableCollection<Staffs>(db.Staffs!.Skip((CurrentPageIndexUser-1) * 12).Take(12));
            }
        }
        private async void NextPageClientsCommandExecute(object o)
        {
            await using (ApplicationContext db = new ApplicationContext())
            {
                if (CurrentPageIndexUser == PageCountUser)
                {
                    return;
                }
                else if (CurrentPageIndexUser > PageCountUser)
                {
                    CurrentPageIndexUser = 1;
                    LoadUsers();
                    return;
                }
                CurrentPageIndexUser++;
                UsersCollection = new ObservableCollection<Staffs>(db.Staffs!.Skip((CurrentPageIndexUser-1) * 12).Take(12));
            }
        }

        #endregion

        #region Методы

        private async void LoadUsers()
        {
            await using (ApplicationContext db = new())
            {
                UsersCollection = new ObservableCollection<Staffs>(db.Staffs!.Take(12));
                PageCountUser = Convert.ToInt32(Math.Ceiling(db.Staffs!.Count()/12m));
            }
        }
        #endregion
    }
}
