using System.ComponentModel;
using System.Windows;

namespace ComplexCalculator
{
    public partial class MenuWindow : Window
    {
        public MenuWindow()
        {
            InitializeComponent();
            DataContext = new MenuViewModel();
        }
    }
}