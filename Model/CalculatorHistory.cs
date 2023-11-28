namespace ComplexCalculator.Model
{
    public class CalculatorHistory : PropChange
    {
        private string calculatorExpression;
        private string calculatorResult;

        public string CalculatorExpression
        {
            get { return calculatorExpression; }
            set 
            {
                calculatorExpression = value;
                OnPropertyChanged("CalculatorExpression");
            }
        }
        public string CalculatorResult
        {
            get { return calculatorResult; }
            set 
            {
                calculatorResult = value;
                OnPropertyChanged("CalculatorResult");
            }
        }
    }
}
