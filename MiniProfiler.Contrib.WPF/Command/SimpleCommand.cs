using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace MiniProfiler.Contrib.WPF.Command
{
    public class SimpleCommand : ICommand
    {
        private readonly Action<object> action;

        public SimpleCommand(Action<object> action)
        {
            this.action = action;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public virtual void Execute(object parameter)
        {
            action(parameter);
        }
    }

    public class SimpleCommand<T> : SimpleCommand
    {
        public SimpleCommand(Action<T> action) : base(obj => action((T)obj))
        {
        }
    }
}
