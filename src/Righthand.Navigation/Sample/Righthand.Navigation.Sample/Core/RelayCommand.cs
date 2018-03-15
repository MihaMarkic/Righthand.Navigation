using System;
using System.Windows.Input;

namespace Righthand.Navigation.Sample.Core
{
    public class RelayCommand : ICommand
    {
        readonly Func<bool> canExecute;
        readonly Action execute;

        public event EventHandler CanExecuteChanged;

        public RelayCommand(Action execute) : this(execute, null)
        {
        }
        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            this.execute = execute ?? throw new ArgumentNullException("execute");
            if (canExecute != null)
            {
                this.canExecute = new Func<bool>(canExecute);
            }
        }
        public bool CanExecute(object parameter)
        {
            if (canExecute == null)
            {
                return true;
            }
            return canExecute();
        }
        public virtual void Execute(object parameter)
        {
            execute();
        }
        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
