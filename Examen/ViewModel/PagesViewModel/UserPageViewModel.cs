#nullable enable
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using ViewModel.Core;
using ViewModel.Services;
using ViewModel.Services.Interfaces;

namespace ViewModel.PagesViewModel
{
    public class UserPageViewModel : BasePageViewModel
    {
        public UserPageViewModel() { // Для дизайнера XAML
            LogOut = new RelayCommand(_ => { });
            ShowTop3Products = new AsyncRelayCommand(async () => { });
            SearchProductByName = new RelayCommand(_ => { });
            SearchProductByPrice = new RelayCommand(_ => { });
            ShowProductInCategory = new RelayCommand(_ => { });
            ShowProductInPortions = new RelayCommand(_ => { });
            SearchOrdersByDate = new RelayCommand(_ => { });
            ShowUserOrderHistory = new AsyncRelayCommand(async () => { });
            CreateOrder = new RelayCommand(_ => { });
            ShowAllProducts = new RelayCommand(_ => { });
            EnterCountPortions = new AsyncRelayCommand(async () => { });
            LeftArrowCountPortions = new AsyncRelayCommand(async () => { });
            RightArrowCountPortions = new AsyncRelayCommand(async () => { });
        } 

        private readonly INavigateService? _navigateService;
        private readonly ApiUserService _apiService = new();

        // Данные пользователя
        public string? LoginUser { get; set; }
        public string? NameUser { get; set; }
        public string? SurnameUser { get; set; }
        public string? EmailUser { get; set; }
        public string? PhoneUser { get; set; }

        // Команды
        public ICommand LogOut { get; private set; }
        public ICommand ShowTop3Products { get; private set; }
        public ICommand SearchProductByName { get; private set; }
        public ICommand SearchProductByPrice { get; private set; }
        public ICommand ShowProductInCategory { get; private set; }
        public ICommand ShowProductInPortions { get; private set; }
        public ICommand SearchOrdersByDate { get; private set; }
        public ICommand ShowUserOrderHistory { get; private set; }
        public ICommand CreateOrder { get; private set; }
        public ICommand ShowAllProducts { get; private set; }
        public ICommand EnterCountPortions { get; private set; }
        public ICommand LeftArrowCountPortions { get; private set; }
        public ICommand RightArrowCountPortions { get; private set; }

        // Пагинация
        private int _skipRows = 0;
        private int _countPortions;
        public int CountPortions
        {
            get => _countPortions;
            set { Set(ref _countPortions, value); UpdateCountPortionsButtonState(); }
        }

        // Данные для отображения
        private DataTable _data = new();
        public DataTable Data
        {
            get => _data;
            set { Set(ref _data, value); }
        }

        // Видимость формы пагинации
        private Visibility _isPortionsFormVisible = Visibility.Collapsed;
        public Visibility IsPortionsFormVisible
        {
            get => _isPortionsFormVisible;
            set { Set(ref _isPortionsFormVisible, value); }
        }

        // Состояние кнопки
        private bool _enterCountPortionsIsEnabled;
        public bool EnterCountPortionsIsEnabled
        {
            get => _enterCountPortionsIsEnabled;
            set
            {
                if (_enterCountPortionsIsEnabled != value)
                {
                    _enterCountPortionsIsEnabled = value;
                    OnPropertyChanged("EnterCountPortionsIsEnabled");
                }
            }
        }

        // Основной конструктор
        public UserPageViewModel(INavigateService navigateService) : this()
        {
            _navigateService = navigateService;

            // Перенастройка команд с реальной логикой
            LogOut = new RelayCommand(_ => OnLogOut());
            ShowTop3Products = new AsyncRelayCommand(async () => await OnShowTop3Products());
            SearchProductByName = new RelayCommand(_ => { });
            SearchProductByPrice = new RelayCommand(_ => { });
            ShowProductInCategory = new RelayCommand(_ => { });
            ShowProductInPortions = new RelayCommand(_ => OnShowProductInPortions());
            SearchOrdersByDate = new RelayCommand(_ => { });
            ShowUserOrderHistory = new AsyncRelayCommand(async () => await OnShowUserOrderHistory());
            CreateOrder = new RelayCommand(_ => { });
            ShowAllProducts = new RelayCommand(_ => { });
            EnterCountPortions = new AsyncRelayCommand(async () => await OnEnterCountPortions());
            LeftArrowCountPortions = new AsyncRelayCommand(async () => await OnLeftArrowCountPortions());
            RightArrowCountPortions = new AsyncRelayCommand(async () => await OnRightArrowCountPortions());
        }

        private void OnLogOut()
        {
            IsPortionsFormVisible = Visibility.Collapsed;
            if (_navigateService != null)
                _navigateService.NavigateTo<LoginPageViewModel>();
        }

        private async Task OnShowTop3Products()
        {
            try
            {
                IsPortionsFormVisible = Visibility.Collapsed;
                var result = await _apiService.GetTop3ProductsAsync();
                Data = result;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void OnShowProductInPortions()
        {
            try
            {
                IsPortionsFormVisible = Visibility.Visible;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private async Task OnEnterCountPortions()
        {
            try
            {
                var result = await _apiService.GetShowProductsInPortionsAsync(_skipRows, CountPortions);
                Data = result;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private async Task OnLeftArrowCountPortions()
        {
            try
            {
                if (_skipRows > 0)
                {
                    _skipRows -= CountPortions;
                    if (_skipRows < 0) _skipRows = 0;
                }
                var result = await _apiService.GetShowProductsInPortionsAsync(_skipRows, CountPortions);
                Data = result;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private async Task OnRightArrowCountPortions()
        {
            try
            {
                var totalCount = await _apiService.GetTotalProductsCountAsync();
                if (_skipRows + CountPortions < totalCount)
                {
                    _skipRows += CountPortions;
                    var result = await _apiService.GetShowProductsInPortionsAsync(_skipRows, CountPortions);
                    Data = result;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private async Task OnShowUserOrderHistory()
        {
            try
            {
                IsPortionsFormVisible = Visibility.Collapsed;
                var result = await _apiService.GetUserOrderHistoryAsync();
                Data = result;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void UpdateCountPortionsButtonState()
        {
            EnterCountPortionsIsEnabled = CountPortions > 0;
        }

        // Изображения
        private byte[]? _imageData;
        public byte[]? ImageData
        {
            get => _imageData;
            set { Set(ref _imageData, value); }
        }

        public BitmapImage? Image
        {
            get
            {
                if (_imageData == null || _imageData.Length == 0)
                    return null;

                try
                {
                    var image = new BitmapImage();
                    using (var stream = new MemoryStream(_imageData))
                    {
                        image.BeginInit();
                        image.StreamSource = stream;
                        image.CacheOption = BitmapCacheOption.OnLoad;
                        image.EndInit();
                        image.Freeze();
                    }
                    return image;
                }
                catch
                {
                    return null;
                }
            }
        }
    }
}