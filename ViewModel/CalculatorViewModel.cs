using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using ComplexCalculator.Model;

namespace ComplexCalculator
{
    public class CalculatorViewModel : PropChange
    {
        private CalculatorModel calculator = new CalculatorModel();

        public string Expression
        {
            get { return Locator.expression; }
            set
            {
                Locator.expression = value;
                OnPropertyChanged("Expression");
            }
        }

        public string Result
        {
            get { return Locator.result; }
            set
            {
                Locator.result = value;
                OnPropertyChanged("Result");
            }
        }

        private CalculatorHistory selectedValue;
        public CalculatorHistory SelectedValue
        {
            get
            {
                return selectedValue;
            }
            set
            {
                selectedValue = value;
                OnPropertyChanged("SelectedValue");
                OnPropertyChanged("ExpressionResult");
            }
        }

        public ICommand NumberCommand { get; set; }
        public ICommand OperatorCommand { get; set; }
        public ICommand CalculateCommand { get; set; }
        public ICommand ClearCommand { get; set; }
        public ICommand ClearLastCharCommand { get; set; }
        public ICommand CopyResultCommand { get; set; }
        public ICommand CopyExpressionCommand  { get; set; }

        public CalculatorViewModel()
        {
            NumberCommand = new RelayCommand(AddNumber);
            OperatorCommand = new RelayCommand(AddOperator);
            CalculateCommand = new RelayCommand(Calculate);
            ClearCommand = new RelayCommand(Clear);
            ClearLastCharCommand = new RelayCommand(ClearLastChar);
            CopyResultCommand = new RelayCommand(CopyResultItem);
            CopyExpressionCommand = new RelayCommand(CopyExpressionItem);
        }

        private void CopyResultItem(object parameter)
        {
            if (SelectedValue != null) Clipboard.SetText(SelectedValue.CalculatorResult);
            else MessageBox.Show("Вы не выбрали выражение из списка!");
        }

        private void CopyExpressionItem(object parameter)
        {
            if (SelectedValue != null) Clipboard.SetText(SelectedValue.CalculatorExpression);
            else MessageBox.Show("Вы не выбрали выражение из списка!");
        }

        private void Clear(object parameter)
        {
            Expression = "";
        }

        private void ClearLastChar(object parameter)
        {
            if (Expression.Length > 0) Expression = Expression.Substring(0, Expression.Length - 1);
        }

        private void AddNumber(object parameter)
        {
            string number = parameter as string;
            if (number != null)
            {
                Expression += number;
            }
        }

        private void AddOperator(object parameter)
        {
            string op = parameter as string;
            if (op != null)
            {
                Expression += op;
            }
        }

        public ObservableCollection<CalculatorHistory> ExpressionResult
        {
            get { return Locator.history; }
        }

        private void Calculate(object parameter)
        {
            try
            {
                Result = calculator.Calculate(Expression).ToString();
                Locator.history.Add(new CalculatorHistory { CalculatorExpression = Expression, CalculatorResult = Result });
                OnPropertyChanged("ExpressionResult");
            }
            catch (Exception ex)
            {
                Result = "Error: " + ex.Message;
            }
        }
    }
}

