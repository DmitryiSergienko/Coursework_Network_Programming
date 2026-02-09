#nullable enable
using System.Data;
using System.Windows.Input;
using ViewModel.Core;
using ViewModel.ModalWindowsViewModel;
using ViewModel.Services;
using ViewModel.Services.Interfaces;

namespace ViewModel.PagesViewModel
{
    public class AdminPageViewModel : BasePageViewModel
    {
        public AdminPageViewModel() { } // Для дизайнера XAML

        private readonly ApiAdminService _apiService = new();
        private readonly INavigateService? _navigateService;

        // Данные администратора
        public string? LoginAdmin { get; set; }
        public string? NameAdmin { get; set; }
        public string? SurnameAdmin { get; set; }
        public string? EmailAdmin { get; set; }
        public string? PhoneAdmin { get; set; }

        // Команды (обязательно инициализируем!)
        public ICommand LogOut { get; private set; } = null!;
        public ICommand AddUser { get; private set; } = null!;
        public ICommand UpdateUser { get; private set; } = null!;
        public ICommand DeleteUser { get; private set; } = null!;
        public ICommand AddAdmin { get; private set; } = null!;
        public ICommand UpdateAdmin { get; private set; } = null!;
        public ICommand DeleteAdmin { get; private set; } = null!;
        public ICommand AddProduct { get; private set; } = null!;
        public ICommand UpdateProduct { get; private set; } = null!;
        public ICommand DeleteProduct { get; private set; } = null!;
        public ICommand AddCategory { get; private set; } = null!;
        public ICommand UpdateCategory { get; private set; } = null!;
        public ICommand DeleteCategory { get; private set; } = null!;
        public ICommand AddQuantityProduct { get; private set; } = null!;
        public ICommand ExecuteQueryCommand { get; private set; } = null!;

        // SQL-запрос
        private string _inputArea = "";
        public string InputArea
        {
            get => _inputArea;
            set { Set(ref _inputArea, value); }
        }

        // Результат запроса
        private DataTable _dataTableAdmin = new();
        public DataTable DataTableAdmin
        {
            get => _dataTableAdmin;
            set { Set(ref _dataTableAdmin, value); }
        }

        // Конструктор для DI
        public AdminPageViewModel(INavigateService navigateService)
        {
            _navigateService = navigateService;

            // Обязательная инициализация всех команд
            LogOut = new RelayCommand(_ => OnLogOut());
            AddUser = new RelayCommand(_ => OnAddUser());
            UpdateUser = new RelayCommand(_ => { });
            DeleteUser = new RelayCommand(_ => OnDeleteUser());
            AddAdmin = new RelayCommand(_ => OnAddAdmin());
            UpdateAdmin = new RelayCommand(_ => { });
            DeleteAdmin = new RelayCommand(_ => OnDeleteAdmin());
            AddProduct = new RelayCommand(_ => { });
            UpdateProduct = new RelayCommand(_ => { });
            DeleteProduct = new RelayCommand(_ => { });
            AddCategory = new RelayCommand(_ => { });
            UpdateCategory = new RelayCommand(_ => { });
            DeleteCategory = new RelayCommand(_ => { });
            AddQuantityProduct = new RelayCommand(_ => { });
            ExecuteQueryCommand = new RelayCommand(_ => ExecuteQuery());
        }

        private void OnLogOut()
        {
            _navigateService?.NavigateTo<LoginPageViewModel>();
        }

        private void OnAddUser()
        {
            var vm = new RegistrationWindowViewModel();
            vm.SetHumanType("user");
        }

        private void OnDeleteUser()
        {
            var vm = new DeleteHumanWindowViewModel();
            vm.HumanType = "user";
        }

        private void OnAddAdmin()
        {
            var vm = new RegistrationWindowViewModel();
            vm.SetHumanType("admin");
        }

        private void OnDeleteAdmin()
        {
            var vm = new DeleteHumanWindowViewModel();
            vm.HumanType = "admin";
        }

        private async void ExecuteQuery()
        {
            try
            {
                var result = await _apiService.ExecuteSqlQueryAsync(InputArea);
                DataTableAdmin = result;
            }
            catch (Exception ex)
            {
                var errorTable = new DataTable();
                errorTable.Columns.Add("Ошибка", typeof(string));
                errorTable.Rows.Add(ex.Message);
                DataTableAdmin = errorTable;
            }
        }
    }
}