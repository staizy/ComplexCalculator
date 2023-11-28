using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace ComplexCalculator
{
    public partial class CalculatorWindow : UserControl
    {
        public CalculatorWindow()
        {
            InitializeComponent();
            DataContext = new CalculatorViewModel();
        }
    }
}