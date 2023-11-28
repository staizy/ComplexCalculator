using System.Windows.Input;
using ComplexCalculator.Model;

namespace ComplexCalculator
{
    public class MenuViewModel : PropChange
    {
        private CalculatorViewModel calculatorViewModel = new CalculatorViewModel();
        private MatrixViewModel matrixViewModel = new MatrixViewModel();
        private object selectedViewModel;

        public object SelectedViewModel
        {
            get { return selectedViewModel; }
            set
            {
                selectedViewModel = value;
                OnPropertyChanged(nameof(SelectedViewModel));
            }
        }

        public MenuViewModel()
        {
            SwitchViewCommand = new RelayCommand(SwitchView);
            SelectedViewModel = matrixViewModel;
        }

        public ICommand SwitchViewCommand { get; set; }

        private void SwitchView(object parameter)
        {
            if (parameter.ToString() == "Calculator") SelectedViewModel = calculatorViewModel;
            else if (parameter.ToString() == "Matrix") SelectedViewModel = matrixViewModel;
        }
    }
}
