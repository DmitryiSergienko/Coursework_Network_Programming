// ViewModel/Core/AsyncRelayCommand.cs
using System.Windows;
using System.Windows.Input;

namespace ViewModel.Core
{
    public class AsyncRelayCommand : ICommand
    {
        private readonly Func<Task> _execute;
        private readonly Func<bool> _canExecute;

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public AsyncRelayCommand(Func<Task> execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute?.Invoke() ?? true;

        public async void Execute(object parameter)
        {
            if (CanExecute(parameter))
            {
                try
                {
                    await _execute();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка команды: {ex.Message}");
                }
            }
        }
    }
}