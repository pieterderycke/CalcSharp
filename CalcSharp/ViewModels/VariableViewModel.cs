using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace CalcSharp.ViewModels
{
    public class VariableViewModel : ViewModelBase
    {
        private const int MaximumNumberOfVariables = 6;

        private MainViewModel mainViewModel;
        private string name;
        private string value;

        public VariableViewModel(MainViewModel mainViewModel)
        {
            this.mainViewModel = mainViewModel;
        }

        public VariableViewModel(MainViewModel mainViewModel, string name, string value)
        {
            this.mainViewModel = mainViewModel;
            this.name = name;
            this.value = value;
        }

        public string Name
        {
            get { return this.name; }
            set
            {
                if (value != "" && this.name == null && mainViewModel.Variables.Count < MaximumNumberOfVariables)
                    mainViewModel.Variables.Add(new VariableViewModel(mainViewModel));

                if (this.value == "" && value == "" && !mainViewModel.IsLastVariable(this))
                    mainViewModel.Variables.Remove(this);

                this.name = value;
            }
        }
        
        public string Value
        {
            get { return this.value; }
            set
            {
                if (this.name == "" && value == "" && !mainViewModel.IsLastVariable(this))
                    mainViewModel.Variables.Remove(this);

                this.value = value;
            }
        }
    }
}
