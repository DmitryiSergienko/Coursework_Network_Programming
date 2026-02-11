using System.Windows;
using System.Windows.Input;
using ViewModel.Core;
using ViewModel.Services.Classes;
using ViewModel.Services.Interfaces;

namespace ViewModel.PagesViewModel;
public class LoginPageViewModel : BasePageViewModel
{
    public LoginPageViewModel() { } // Для дизайнера

    private readonly INavigateService _navigateService;
    private readonly ApiAuthService _authService = new(); // ← HTTP-клиент

    public ICommand NavigateToSignIn { get; }
    public ICommand NavigateToRegistration { get; }

    public LoginPageViewModel(INavigateService navigateService)
    {
        _navigateService = navigateService;
        NavigateToSignIn = new RelayCommand(async _ => await OnLoginAsync());
        NavigateToRegistration = new RelayCommand(_ => navigateService.NavigateTo<RegistrationPageViewModel>());
    }

    private string _login = "";
    private string _password = "";

    public string Login
    {
        get => _login;
        set { Set(ref _login, value); UpdateSignInButtonState(); }
    }

    public string Password
    {
        get => _password;
        set { Set(ref _password, value); UpdateSignInButtonState(); }
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
                OnPropertyChanged("IsEnabledNavigateToSignIn");
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
            MessageBox.Show("Заполните обязательные поля.");
            return;
        }

        try
        {
            var user = await _authService.LoginAsync(Login, Password);

            if (user != null)
            {
                // Сохрани данные пользователя (например, в статическом классе или сервисе)
                CurrentUser.Instance.SetUser(user);

                if (user.Type == "admin")
                    _navigateService.NavigateTo<AdminPageViewModel>();
                else
                    _navigateService.NavigateTo<UserPageViewModel>();
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