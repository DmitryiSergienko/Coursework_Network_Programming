using System.Windows;
using System.Windows.Input;
using ViewModel.Core;
using ViewModel.Models;
using ViewModel.Services;
using ViewModel.Services.Interfaces;

namespace ViewModel.PagesViewModel
{
    public class RegistrationPageViewModel : BasePageViewModel
    {
        public RegistrationPageViewModel() { } // Для дизайнера XAML

        private readonly INavigateService _navigateService;
        private readonly ApiRegistrationService _registrationService = new();

        public ICommand BackToLoginPage { get; }
        public ICommand SendFormRegistration { get; }

        private string _login = "";
        private string _password = "";
        private string _name = "";
        private string _surname = "";
        private string _patronymic = "";
        private string _mail = "";
        private string _phoneNumber = "";

        public RegistrationPageViewModel(INavigateService navigateService)
        {
            _navigateService = navigateService;
            BackToLoginPage = new RelayCommand(_ => _navigateService.NavigateTo<LoginPageViewModel>());
            SendFormRegistration = new RelayCommand(_ => OnSendForm());
        }

        private async void OnSendForm()
        {
            if (string.IsNullOrWhiteSpace(Login) ||
                string.IsNullOrWhiteSpace(Password) ||
                string.IsNullOrWhiteSpace(Name) ||
                string.IsNullOrWhiteSpace(Surname) ||
                string.IsNullOrWhiteSpace(Mail))
            {
                MessageBox.Show("Заполните обязательные поля.");
                return;
            }

            var request = new RegisterRequest
            {
                Login = Login,
                Password = Password,
                Name = Name,
                Surname = Surname,
                Patronymic = Patronymic,
                Mail = Mail,
                PhoneNumber = PhoneNumber,
                HumanType = "user" // или "admin" если нужно
            };

            try
            {
                int newId = await _registrationService.RegisterAsync(request);

                if (newId > 0)
                {
                    MessageBox.Show($"Регистрация успешна! ID: {newId}");
                    _navigateService.NavigateTo<LoginPageViewModel>();
                }
                else
                {
                    MessageBox.Show("Ошибка при регистрации.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        public string Login
        {
            get => _login;
            set { Set(ref _login, value); UpdateSendButtonState(); }
        }

        public string Password
        {
            get => _password;
            set { Set(ref _password, value); UpdateSendButtonState(); }
        }

        public string Name
        {
            get => _name;
            set { Set(ref _name, value); UpdateSendButtonState(); }
        }

        public string Surname
        {
            get => _surname;
            set { Set(ref _surname, value); UpdateSendButtonState(); }
        }

        public string Patronymic
        {
            get => _patronymic;
            set { Set(ref _patronymic, value); UpdateSendButtonState(); }
        }

        public string Mail
        {
            get => _mail;
            set { Set(ref _mail, value); UpdateSendButtonState(); }
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set { Set(ref _phoneNumber, value); UpdateSendButtonState(); }
        }

        private void UpdateSendButtonState()
        {
            IsEnabledSendFormRegistration = !string.IsNullOrWhiteSpace(Login)
                                        && !string.IsNullOrWhiteSpace(Password)
                                        && !string.IsNullOrWhiteSpace(Name)
                                        && !string.IsNullOrWhiteSpace(Surname)
                                        && !string.IsNullOrWhiteSpace(Mail);
        }

        private bool _isEnabledSendFormRegistration;
        public bool IsEnabledSendFormRegistration
        {
            get => _isEnabledSendFormRegistration;
            set { Set(ref _isEnabledSendFormRegistration, value); }
        }
    }
}