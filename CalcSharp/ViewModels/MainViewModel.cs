using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Jace;

namespace CalcSharp.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly CalculationEngine calculationEngine;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            this.calculationEngine = new CalculationEngine(CultureInfo.CurrentCulture);

            this.Variables = new ObservableCollection<VariableViewModel>();
            this.Variables.Add(new VariableViewModel(this));

            this.History = new ObservableCollection<HistoryItemViewModel>();

            this.Calculate = new RelayCommand(CalculateResult, () => !string.IsNullOrWhiteSpace(Formula));

            if (IsInDesignMode)
            {
                // Code runs in Blend --> create design time data.
                this.History.Add(new HistoryItemViewModel(this, "salary*index", 42, new Tuple<string, string>[0]));
            }
        }

        public ObservableCollection<VariableViewModel> Variables { get; private set; }
        public ObservableCollection<HistoryItemViewModel> History { get; private set; }

        private string formula;
        public string Formula
        {
            get { return formula; } 
            set { formula = value; RaisePropertyChanged("Formula"); Calculate.RaiseCanExecuteChanged(); }
        }

        public string Result { get; set; }
        public RelayCommand Calculate { get; private set; }

        private string error;
        public string Error
        {
            get { return error; }
            set { error = value; RaisePropertyChanged("Error"); }
        }

        public bool IsLastVariable(VariableViewModel variable)
        {
            return Variables[Variables.Count - 1] == variable;
        }

        private void CalculateResult()
        {
            Error = "";

            Dictionary<string, double> variables = new Dictionary<string, double>();
            foreach (VariableViewModel variable in Variables)
            {
                if (!string.IsNullOrWhiteSpace(variable.Name) && !string.IsNullOrWhiteSpace(variable.Value))
                {
                    try
                    {
                        variables.Add(variable.Name, double.Parse(variable.Value));
                    }
                    catch (FormatException)
                    {
                        if (Error != "")
                            Error += "\n";
                        Error += string.Format("Please provide a numeric value for variable \"{0}\".", variable.Name);
                    }
                }
            }

            if (!string.IsNullOrEmpty(Error))
                return; // There are errors, so we stop

            try
            {
                double result = calculationEngine.Calculate(Formula, variables);

                Result = "" + result;
                RaisePropertyChanged("Result");

                History.Insert(0, new HistoryItemViewModel(this, Formula, result, CopyVariables()));
            }
            catch (Exception ex)
            {
                Error = ex.Message;
            }
        }

        private List<Tuple<string, string>> CopyVariables()
        {
            List<Tuple<string, string>> variables = new List<Tuple<string, string>>();
            foreach (VariableViewModel variable in Variables)
            {
                variables.Add(new Tuple<string, string>(variable.Name, variable.Value));
            }

            return variables;
        }
    }
}