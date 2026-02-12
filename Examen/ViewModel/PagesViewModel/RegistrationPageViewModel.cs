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
        public RegistrationPageViewModel()
        {
            SendFormRegistration = new AsyncRelayCommand(OnSendForm);
        }

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

        public RegistrationPageViewModel(INavigateService navigateService) : this()
        {
            _navigateService = navigateService;
            BackToLoginPage = new RelayCommand(_ => _navigateService.NavigateTo<LoginPageViewModel>());

            this.PropertyChanged += (s, e) => {
                UpdateSendButtonState();
            };

            UpdateSendButtonState();
        }

        public string Login
        {
            get => _login;
            set
            {
                _login = value;
                OnPropertyChanged(nameof(Login));
                UpdateSendButtonState();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
                System.Diagnostics.Debug.WriteLine($"[REG PASSWORD] = '{value}'");
                UpdateSendButtonState();
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
                UpdateSendButtonState();
            }
        }

        public string Surname
        {
            get => _surname;
            set
            {
                _surname = value;
                OnPropertyChanged(nameof(Surname));
                UpdateSendButtonState();
            }
        }

        public string Patronymic
        {
            get => _patronymic;
            set
            {
                _patronymic = value;
                OnPropertyChanged(nameof(Patronymic));
            }
        }

        public string Mail
        {
            get => _mail;
            set
            {
                _mail = value;
                OnPropertyChanged(nameof(Mail));
                UpdateSendButtonState();
            }
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                _phoneNumber = value;
                OnPropertyChanged(nameof(PhoneNumber));
            }
        }

        private bool _isEnabledSendFormRegistration;
        public bool IsEnabledSendFormRegistration
        {
            get => _isEnabledSendFormRegistration;
            set
            {
                if (_isEnabledSendFormRegistration != value)
                {
                    _isEnabledSendFormRegistration = value;
                    OnPropertyChanged(nameof(IsEnabledSendFormRegistration));
                }
            }
        }

        private void UpdateSendButtonState()
        {
            bool isValid = !string.IsNullOrWhiteSpace(Login)
                && !string.IsNullOrWhiteSpace(Password)
                && !string.IsNullOrWhiteSpace(Name)
                && !string.IsNullOrWhiteSpace(Surname)
                && !string.IsNullOrWhiteSpace(Mail);

            System.Diagnostics.Debug.WriteLine($"[REG] Login='{Login}', Pwd='{Password}', Name='{Name}', Surname='{Surname}', Mail='{Mail}' -> isValid={isValid}");

            IsEnabledSendFormRegistration = isValid;
        }

        private async Task OnSendForm()
        {
            if (!IsEnabledSendFormRegistration)
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
                HumanType = "user"
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
    }
}