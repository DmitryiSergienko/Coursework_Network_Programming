using System.Windows;
using System.Windows.Input;
using ViewModel.Core;
using ViewModel.Services.Classes;
using ViewModel.Services.Interfaces;

namespace ViewModel.PagesViewModel
{
    public class LoginPageViewModel : BasePageViewModel
    {
        private readonly INavigateService _navigateService;
        private readonly ApiAuthService _authService = new();

        public ICommand NavigateToSignIn { get; }
        public ICommand NavigateToRegistration { get; }

        public LoginPageViewModel(INavigateService navigateService)
        {
            _navigateService = navigateService;

            System.Diagnostics.Debug.WriteLine("!!! КОНСТРУКТОР LoginPageViewModel ВЫЗВАН !!!");

            NavigateToSignIn = new RelayCommand(async _ => {
                System.Diagnostics.Debug.WriteLine("!!! КОМАНДА NavigateToSignIn ВЫПОЛНЕНА !!!");
                await OnLoginAsync();
            });

            NavigateToRegistration = new RelayCommand(_ =>
                _navigateService.NavigateTo<RegistrationPageViewModel>());
        }

        private string _login = "";
        public string Login
        {
            get => _login;
            set
            {
                _login = value;
                OnPropertyChanged(nameof(Login));
                System.Diagnostics.Debug.WriteLine($"[LOGIN] = '{value}'");
                UpdateSignInButtonState();
            }
        }

        private string _password = "";
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
                System.Diagnostics.Debug.WriteLine($"[PASSWORD] = '{value}'");
                UpdateSignInButtonState();
            }
        }

        private bool _isEnabledNavigateToSignIn;
        public bool IsEnabledNavigateToSignIn
        {
            get => _isEnabledNavigateToSignIn;
            set
            {
                if (_isEnabledNavigateToSignIn != value)
                {
                    _isEnabledNavigateToSignIn = value;
                    OnPropertyChanged(nameof(IsEnabledNavigateToSignIn));
                    System.Diagnostics.Debug.WriteLine($"[BUTTON] Enabled = {value}");
                }
            }
        }

        private void UpdateSignInButtonState()
        {
            IsEnabledNavigateToSignIn = !string.IsNullOrWhiteSpace(Login) &&
                                       !string.IsNullOrWhiteSpace(Password);
        }

        private async Task OnLoginAsync()
        {
            if (string.IsNullOrWhiteSpace(Login) || string.IsNullOrWhiteSpace(Password))
            {
                MessageBox.Show("Заполните поля.");
                return;
            }

            try
            {
                var user = await _authService.LoginAsync(Login, Password);

                if (user != null)
                {
                    System.Diagnostics.Debug.WriteLine($"[CLIENT] Тип от сервера: '{user.Type}'");
                    System.Diagnostics.Debug.WriteLine($"[CLIENT] Длина строки: {user.Type?.Length}");
                    System.Diagnostics.Debug.WriteLine($"[CLIENT] Это админ? {user.Type == "admin"}");

                    CurrentUser.Instance.SetUser(user);

                    if (user.Type == "admin")
                    {
                        System.Diagnostics.Debug.WriteLine("[CLIENT] ПЕРЕХОД НА АДМИНКУ!");
                        _navigateService.NavigateTo<AdminPageViewModel>();
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("[CLIENT] ПЕРЕХОД НА ЮЗЕРА!");
                        _navigateService.NavigateTo<UserPageViewModel>();
                    }
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
    }
}