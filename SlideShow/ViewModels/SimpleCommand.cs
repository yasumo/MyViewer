using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SlideShow.ViewModels
{
    class SimpleCommand : ICommand
    {
        Action execute;
        public SimpleCommand(Action execute)
        {
            this.execute = execute;
        }
        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter)
            => true;
        public void Execute(object parameter)
            => execute();
    }
}
