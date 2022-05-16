using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PokiApi.ViewModels
{
    internal class RelayCommand : ICommand
    {
        private Action<object>? _action;
        public RelayCommand(Action<object>? action)
        {
            _action = action;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            //throw new NotImplementedException();
            //zal niet altijd true zijn, kan met if-else opgevangen worden
            //adhv de parameter parameter die meegegeven wordt
            return true;
        }

        public void Execute(object? parameter)
        {
            //throw new NotImplementedException();
            if (parameter != null)
            {
                //actie uitgevoerd worden met de variabele 'parameter'
                _action(parameter);
            }
            else
            {
                //actie met foutmelding
                _action("foutmelding");
            }
        }
    }
}
