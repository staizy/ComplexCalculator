using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ComplexCalculator.Model;

namespace ComplexCalculator
{
    public class MatrixViewModel : PropChange
    {
        private Matrix matrix1;
        private Matrix matrix2;
        private string matrix1ValidationStr;
        private string matrix2ValidationStr;
        public int Matrix1RowCount { get; set; }
        public int Matrix1ColCount { get; set; }
        public string Matrix1FillNumber { get; set; } = "0";
        public int Matrix2RowCount { get; set; }
        public int Matrix2ColCount { get; set; }
        public string Matrix2FillNumber { get; set; } = "0";
        public static ComboBoxItem[] Operations { get; set; } = new ComboBoxItem[] { new ComboBoxItem { Content = "Сумма" }, new ComboBoxItem { Content = "Вычитание" },
                                                         new ComboBoxItem { Content = "Определитель" },  new ComboBoxItem { Content = "Умножение" },
                                                        new ComboBoxItem { Content = "Транспонирование" }, new ComboBoxItem { Content = "Обратная матрица" },
                                                        new ComboBoxItem { Content = "Умножить на" } };
        public ComboBoxItem OperationType { get; set; } = Operations[0];


        public MatrixViewModel()
        {
            Build1Matrix = new RelayCommand(Bulid1ByNumber);
            Build2Matrix = new RelayCommand(Bulid2ByNumber);
            GetResult = new RelayCommand(CalculateResult);
            CopyResultCommand = new RelayCommand(CopyResultItem);
            Matrix1ValidationStr = "NoMatrix";
            Matrix2ValidationStr = "NoMatrix";
        }

        private void CopyResultItem(object parameter)
        {
            if (Locator.m_result != "") Clipboard.SetText(Locator.m_result.ToString());
            else MessageBox.Show("Результирующая матрица пуста, копировать нечего!");
        }

        public string Result
        {
            get { return Locator.m_result; }
            set
            {
                Locator.m_result = value;
                OnPropertyChanged(nameof(Result));
            }
        }

        /*string FormatString(string str)
        {
            var res = "";
            str = str.Replace("\r", "");
            str = Regex.Replace(str, @"\n+", "\n");
            foreach (var i in str.Split("\n"))
            {
                if (i.Length == 1)
                {
                    if (i[0] != ' ') res += i + "\n";
                }
                else
                {
                    res += Regex.Replace(i, @"\s+", " ") + '\n';
                }
            }
            return res;
        }*/

        public string Matrix1Str
        {
            get
            {
                var res = Matrix.ValidateMatrixString(Locator.matrix1Str);
                Matrix1ValidationStr = res.ToString();
                if (res == Matrix.ValidationResult.Valid) matrix1 = Matrix.FromStr(Locator.matrix1Str);
                return Locator.matrix1Str;
            }
            set
            {
                Locator.matrix1Str = value.Replace("\r", "").Replace("  ", " ").Replace(" \n", "\n").Replace("\n\n", "\n");
                //Locator.matrix1Str = FormatString(value);
                OnPropertyChanged(nameof(Matrix1Str));
                var res = Matrix.ValidateMatrixString(value);
                Matrix1ValidationStr = res.ToString();
                if (res == Matrix.ValidationResult.Valid) matrix1 = Matrix.FromStr(value);
            }
        }
        public string Matrix2Str
        {
            get 
            {
                var res = Matrix.ValidateMatrixString(Locator.matrix2Str);
                Matrix2ValidationStr = res.ToString();
                if (res == Matrix.ValidationResult.Valid) matrix2 = Matrix.FromStr(Locator.matrix2Str);
                return Locator.matrix2Str;
            }
            set
            {
                Locator.matrix2Str = value.Replace("\r", "").Replace("  ", " ").Replace(" \n", "\n").Replace("\n\n", "\n");
                OnPropertyChanged(nameof(Matrix2Str));
                var res = Matrix.ValidateMatrixString(value);
                Matrix2ValidationStr = res.ToString();
                if (res == Matrix.ValidationResult.Valid) matrix2 = Matrix.FromStr(value);
            }
        }

        public string Matrix1ValidationStr
        {
            get { return matrix1ValidationStr; }
            set
            {
                matrix1ValidationStr = value;
                OnPropertyChanged(nameof(Matrix1ValidationStr));
            }
        }
        public string Matrix2ValidationStr
        {
            get { return matrix2ValidationStr; }
            set
            {
                matrix2ValidationStr = value;
                OnPropertyChanged(nameof(Matrix2ValidationStr));
            }
        }

        public ICommand Build1Matrix { get; set; }
        public ICommand Build2Matrix { get; set; }
        public ICommand GetResult { get; set; }
        public ICommand CopyResultCommand { get; set; }

        private void Bulid1ByNumber(object parameter)
        {
            string str = "";
            if (!int.TryParse(Matrix1FillNumber.ToString(), out _))
            {
                MessageBox.Show("Это не число");
            }
            else
            {
                for (int i = 0; i < Matrix1RowCount; i++)
                {
                    for (int j = 0; j < Matrix1ColCount; j++)
                    {
                        str += Matrix1FillNumber.ToString() + " ";
                    }
                    str = str.Trim() + "\n";
                }
                Matrix1Str = str;
            }
        }

        private void Bulid2ByNumber(object parameter)
        {
            string str = "";
            if (!int.TryParse(Matrix2FillNumber.ToString(), out _))
            {
                MessageBox.Show("Это не число");
            }
            else
            {
                for (int i = 0; i < Matrix2RowCount; i++)
                {
                    for (int j = 0; j < Matrix2ColCount; j++)
                    {
                        str += Matrix2FillNumber.ToString() + " ";
                    }
                    str = str.Trim() + "\n";
                }
                Matrix2Str = str;
            }
        }

        private void CalculateResult(object parameter)
        {
            if (Matrix1ValidationStr != "Valid") { MessageBox.Show("Матрица должна иметь адекватный вид!"); return; }
            Matrix1Str = Matrix1Str.Trim();
            switch (OperationType.Content)
            {
                case "Определитель":
                    {
                        if (matrix1 == null) MessageBox.Show("Для вычисления определителя матрицы она должна быть не пустой");
                        else
                        {
                            if (!matrix1.IsSquare) MessageBox.Show("Для вычисления определителя матрицы она должна быть квадратной");
                            else if (matrix1.Size.rows > 10 || matrix1.Size.cols > 10) MessageBox.Show("Для вычисления определителя матрицы она должна быть размера не более 10 на 10 элементов.");
                            else
                            {
                                Result = matrix1.CalculateDeterminant().ToString();
                            }
                        }
                    }
                    break;
                case "Сумма":
                    {
                        if (matrix1 == null || matrix2 == null) MessageBox.Show("Для вычисления определителя матрицы она должна быть не пустой");
                        else if (matrix1.Size != matrix2.Size) MessageBox.Show("Матрицы должны быть одного размера!");
                        else Result = (matrix1 + matrix2).ToString();
                    }
                    break;
                case "Вычитание":
                    {
                        if (matrix1 == null || matrix2 == null) MessageBox.Show("Для вычисления определителя матрицы она должна быть не пустой");
                        else if (matrix1.Size != matrix2.Size) MessageBox.Show("Матрицы должны быть одного размера!");
                        else Result = (matrix1 - matrix2).ToString();
                    }
                    break;
                case "Умножение":
                    {
                        if (matrix1 == null || matrix2 == null) MessageBox.Show("Для вычисления определителя матрицы она должна быть не пустой");
                        else if (matrix1.Size.cols != matrix2.Size.rows) MessageBox.Show("Количество строк одной матрицы должно соответствовать количеству столбцов другой матрицы!");
                        else Result = (matrix1 * matrix2).ToString();
                    }
                    break;
                case "Транспонирование":
                    {
                        if (matrix1 == null) MessageBox.Show("Для вычисления определителя матрицы она должна быть не пустой");
                        else Result = matrix1.Transpose().ToString();
                    }
                    break;
                case "Умножить на":
                    {
                        if (matrix1 == null || matrix2 == null) MessageBox.Show("Для вычисления определителя матрицы она должна быть не пустой");
                        else if (matrix2.Size != (1, 1)) MessageBox.Show("Во втором окне должно быть одно число!");
                        else Result = (matrix1 * matrix2.arr[0, 0]).ToString();
                    }
                    break;
                case "Обратная матрица":
                    {
                        if (matrix1 == null) MessageBox.Show("Для вычисления определителя матрицы она должна быть не пустой");
                        else if (matrix1.Size.rows > 10 || matrix1.Size.cols > 10) MessageBox.Show("Для вычисления обратной матрицы она должна быть размера не более 10 на 10 элементов.");
                        else
                        {                            
                            try 
                            {
                                Result = matrix1.Inverse().ToString();
                            }
                            catch (Exception ex) 
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                    }
                    break;
            }
        }
    }
}
