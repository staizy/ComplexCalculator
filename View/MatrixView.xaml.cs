using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace ComplexCalculator
{
    public partial class MatrixWindow : UserControl
    {
        public MatrixWindow()
        {
            InitializeComponent();
            DataContext = new MatrixViewModel();
        }
    }
}