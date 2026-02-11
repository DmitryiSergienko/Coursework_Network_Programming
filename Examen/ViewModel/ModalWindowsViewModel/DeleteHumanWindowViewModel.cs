#nullable enable
using System.Windows;
using System.Windows.Input;
using ViewModel.Core;
using ViewModel.Models;
using ViewModel.Services.Classes;
using ViewModel.Services.Interfaces;

namespace ViewModel.ModalWindowsViewModel
{
    public class DeleteHumanWindowViewModel : BasePageViewModel, IClosable
    {
        private readonly ApiHumanService _apiService = new();
        public event Action? RequestClose;

        public ICommand SearchHuman { get; }
        public ICommand DeleteHuman { get; }

        private int _id;
        public int Id
        {
            get => _id;
            set
            {
                Set(ref _id, value);
                UpdateSearchButtonState();
            }
        }

        private HumanDto? _foundHuman;
        public string? Login => _foundHuman?.Login;
        public string? Name => _foundHuman?.Name;
        public string? Surname => _foundHuman?.Surname;
        public string? Patronymic => _foundHuman?.Patronymic;
        public string? Email => _foundHuman?.Mail;
        public string? Phone => _foundHuman?.PhoneNumber;

        private string _humanType = "user";
        public string HumanType
        {
            get => _humanType;
            set => _humanType = value;
        }

        public DeleteHumanWindowViewModel()
        {
            SearchHuman = new AsyncRelayCommand(OnSearch);
            DeleteHuman = new AsyncRelayCommand(OnDelete);
        }

        public void ClearData()
        {
            Id = 0;
            _foundHuman = null;
            RefreshHumanProperties();
        }

        private async Task OnSearch()
        {
            if (Id <= 0) return;

            try
            {
                var human = await _apiService.GetHumanByIdAsync(Id, HumanType);
                if (human != null)
                {
                    _foundHuman = human;
                    RefreshHumanProperties();
                    UpdateDeleteButtonState();
                }
                else
                {
                    MessageBox.Show("Пользователь не найден");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка поиска: {ex.Message}");
            }
        }

        private async Task OnDelete()
        {
            if (_foundHuman == null) return;

            try
            {
                var success = await _apiService.DeleteHumanAsync(_foundHuman.Id, HumanType);
                if (success)
                {
                    MessageBox.Show("Пользователь удалён");
                    _foundHuman = null;
                    RefreshHumanProperties();
                    RequestClose?.Invoke();
                }
                else
                {
                    MessageBox.Show("Не удалось удалить пользователя");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления: {ex.Message}");
            }
        }

        private void UpdateSearchButtonState()
        {
            IsEnabledIdInput = Id > 0;
        }

        private void UpdateDeleteButtonState()
        {
            IsEnabledFormDelete = _foundHuman != null;
        }

        private void RefreshHumanProperties()
        {
            OnPropertyChanged(nameof(Login));
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(Surname));
            OnPropertyChanged(nameof(Patronymic));
            OnPropertyChanged(nameof(Email));
            OnPropertyChanged(nameof(Phone));
        }

        private bool _isEnabledIdInput;
        public bool IsEnabledIdInput
        {
            get => _isEnabledIdInput;
            set
            {
                if (_isEnabledIdInput != value)
                {
                    _isEnabledIdInput = value;
                    OnPropertyChanged("IsEnabledIdInput");
                }
            }
        }

        private bool _isEnabledFormDelete;
        public bool IsEnabledFormDelete
        {
            get => _isEnabledFormDelete;
            set
            {
                if (_isEnabledFormDelete != value)
                {
                    _isEnabledFormDelete = value;
                    OnPropertyChanged("IsEnabledFormDelete");
                }
            }
        }
    }
}