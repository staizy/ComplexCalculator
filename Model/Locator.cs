using System.Collections.ObjectModel;

namespace ComplexCalculator.Model
{
    public static class Locator
    {
        public static string expression = "";
        public static string result = "";
        public static ObservableCollection<CalculatorHistory> history = new ObservableCollection<CalculatorHistory>();

        public static string matrix1Str = "";
        public static string matrix2Str = "";
        public static string m_result = "";
    }
}
