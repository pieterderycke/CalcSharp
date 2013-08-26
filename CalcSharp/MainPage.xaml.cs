using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;
using CalcSharp.ViewModels;
using WinRTXamlToolkit.Controls;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CalcSharp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Dispatcher.AcceleratorKeyActivated += OnAcceleratorKeyActivated;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            Dispatcher.AcceleratorKeyActivated -= OnAcceleratorKeyActivated;
        }

        private void OnAcceleratorKeyActivated(CoreDispatcher sender, AcceleratorKeyEventArgs args)
        {
            // Ensures the ENTER key always runs the same code as your default button.
            if (args.EventType == CoreAcceleratorKeyEventType.KeyDown && args.VirtualKey == VirtualKey.Enter)
            {
                ICommand calculateCommand = ((MainViewModel) DataContext).Calculate;

                if(calculateCommand.CanExecute(null))
                    calculateCommand.Execute(null);
            }
        }

        private void ListViewBase_OnItemClick(object sender, ItemClickEventArgs e)
        {
            // TODO: remove quick hack
            ((HistoryItemViewModel)e.ClickedItem).Display.Execute(null);
        }

        private void FormulaTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            // No UpdateSourceTrigger in Windows 8 currently ...
            string newFormula = ((TextBox) sender).Text;
            ((MainViewModel) DataContext).Formula = newFormula;
        }

        private void VariableNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // No UpdateSourceTrigger in Windows 8 currently ...
            WatermarkTextBox nameTextBox = (WatermarkTextBox)sender;
            string newName = nameTextBox.Text;
            ((VariableViewModel) nameTextBox.DataContext).Name = newName;
        }

        private void VariableValueTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // No UpdateSourceTrigger in Windows 8 currently ...
            WatermarkTextBox valueTextBox = (WatermarkTextBox)sender;
            string newValue = valueTextBox.Text;
            ((VariableViewModel)valueTextBox.DataContext).Value = newValue;
        }
    }
}
