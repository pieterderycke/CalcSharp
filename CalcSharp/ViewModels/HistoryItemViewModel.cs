using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace CalcSharp.ViewModels
{
    public class HistoryItemViewModel : ViewModelBase
    {
        private readonly MainViewModel mainViewModel;

        public HistoryItemViewModel(MainViewModel mainViewModel, string formula, double result,
            IEnumerable<Tuple<string, string>> variables)
        {
            this.mainViewModel = mainViewModel;
            this.Formula = formula;
            this.Result = result;
            this.Variables = variables;
            this.Display = new RelayCommand(DisplayHistoryItem);
        }

        public string Formula { get; private set; }
        public double Result { get; private set; }
        public IEnumerable<Tuple<string, string>> Variables { get; private set; }
        public ICommand Display { get; private set; }

        private void DisplayHistoryItem()
        {
            mainViewModel.Formula = Formula;
            mainViewModel.Result = "";
            mainViewModel.Variables.Clear();
            foreach (Tuple<string, string> variable in Variables)
            {
                mainViewModel.Variables.Add(new VariableViewModel(mainViewModel, variable.Item1, variable.Item2));
            }
        }
    }
}
