using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PhoneShop.ViewModel
{
    public class RelayCommand : ICommand
    {
        
        private readonly Action<object?>? _execute;
        private readonly Func<object?, bool> _canExecute;
        public event EventHandler? CanExecuteChanged;

        public RelayCommand(Action<object?>? execute, Func<object?, bool> canExcute)
        {
            _execute = execute;
            _canExecute = canExcute;
        }

        public bool CanExecute(object? Parameter) => _canExecute?.Invoke(Parameter) ?? true;

        public void Execute(object? Parameter) => _execute?.Invoke(Parameter);
        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
