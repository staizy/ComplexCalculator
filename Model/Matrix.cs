using System;
using System.Collections.Generic;
using System.Linq;

namespace ComplexCalculator.Model
{
    public class Matrix
    {
        public double[,] arr;
        public bool IsSquare
        {
            get { return arr.GetLength(0) == arr.GetLength(1); }
        }

        public Matrix(double[,] arr)
        {
            this.arr = arr;
        }

        public static Matrix FromStr(string str)
        {
            str = str.Trim();
            if (str[str.Length - 1] == '\n') str = str.Substring(0, str.Length - 1);
            string[] rows = str.Split('\n');
            int rowCount = rows.Length;
            int colCount = rows[0].Split(' ').Length;

            double[,] matrixArr = new double[rowCount, colCount];

            for (int i = 0; i < rowCount; i++)
            {
                string[] elements = rows[i].Split(' ');

                for (int j = 0; j < colCount; j++)
                {
                    double.TryParse(elements[j], out double element);
                    matrixArr[i, j] = element;
                }
            }
            return new Matrix(matrixArr);
        }

        public static ValidationResult ValidateMatrixString(string str)
        {
            if (string.IsNullOrEmpty(str)) return ValidationResult.No_Matrix;
            str = str.Trim();
            if (str[str.Length - 1] == '\n') str = str.Substring(0, str.Length - 1);

            string[] rows = str.Split('\n');
            int rowCount = rows.Length;
            int colCount = rows[0].Split(' ').Length;

            for (int i = 0; i < rowCount; i++)
            {
                string[] elements = rows[i].Split(' ');

                if (elements.Length != colCount) return ValidationResult.Error;
                foreach (string element in elements)
                {
                    if (!double.TryParse(element, out _)) return ValidationResult.Error;
                }
            }
            return ValidationResult.Valid;
        }

        public double CalculateDeterminant()
        {
            return CalculateDeterminant(arr);
        }

        public (int rows, int cols) Size
        {
            get => (arr.GetLength(0), arr.GetLength(1));
        }

        private double[,] Minor(int a, int b, double[,] arr)
        {
            double[,] result = new double[arr.GetLength(0) - 1, arr.GetLength(1) - 1];

            int row = 0, col = 0;
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                if (i == a) continue;
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    if (j == b) continue;
                    result[row, col] = arr[i, j];
                    col++;
                }
                row++;
                col = 0;
            }
            return result;
        }

        private double CalculateDeterminant(double[,] arr)
        {
            int n = arr.GetLength(0);
            double det = 0;
            if (n == 1) return arr[0, 0];
            else if (n == 2)
            {
                det = arr[0, 0] * arr[1, 1] - arr[0, 1] * arr[1, 0];
                return det;
            }
            else
            {
                for (int k = 0; k < n; k++) det += Math.Pow(-1, k) * arr[0, k] * CalculateDeterminant(Minor(0, k, arr));
                return det;
            }
        }

        public override string ToString()
        {
            var str = "";
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    str += arr[i, j].ToString("0.###") + " ";
                }
                str = str.Trim() + "\n";
            }
            return str;
        }

        public static Matrix operator +(Matrix m1, Matrix m2)
        {
            if (m1.Size != m2.Size) return new(new double[0, 0]);
            double[,] arr = new double[m1.Size.rows, m1.Size.cols];
            for (int i = 0; i < m1.Size.rows; i++)
            {
                for (int j = 0; j < m2.Size.cols; j++)
                {
                    arr[i, j] = m1.arr[i, j] + m2.arr[i, j];
                }
            }
            return new(arr);
        }

        public static Matrix operator -(Matrix m1, Matrix m2)
        {
            if (m1.Size != m2.Size) return new(new double[0, 0]);
            double[,] arr = new double[m1.Size.rows, m1.Size.cols];
            for (int i = 0; i < m1.Size.rows; i++)
            {
                for (int j = 0; j < m2.Size.cols; j++)
                {
                    arr[i, j] = m1.arr[i, j] - m2.arr[i, j];
                }
            }
            return new(arr);
        }

        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            int rows = m1.Size.rows;
            int cols = m2.Size.cols;
            int commonDimension = m1.Size.cols;

            double[,] result = new double[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    double sum = 0;
                    for (int k = 0; k < commonDimension; k++)
                    {
                        sum += m1.arr[i, k] * m2.arr[k, j];
                    }
                    result[i, j] = sum;
                }
            }

            return new Matrix(result);
        }

        public Matrix Transpose()
        {
            int rows = arr.GetLength(0);
            int cols = arr.GetLength(1);

            double[,] result = new double[cols, rows];

            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    result[i, j] = arr[j, i];
                }
            }

            return new Matrix(result);
        }

        public static Matrix operator *(Matrix m1, double nubmer)
        {
            int rows = m1.Size.rows;
            int cols = m1.Size.cols;

            double[,] result = new double[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    result[i, j] = m1.arr[i, j] * nubmer;
                }
            }

            return new Matrix(result);
        }

        public Matrix Inverse()
        {
            int size = arr.GetLength(0);
            double determinant = CalculateDeterminant(arr);

            if (determinant == 0)
                throw new InvalidOperationException("Матрица вырожденная, обратной матрицы не существует.");

            double[,] inverse = new double[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    double minorDeterminant = CalculateDeterminant(Minor(i, j, arr));
                    double cofactor = (i + j) % 2 == 0 ? minorDeterminant : -minorDeterminant;
                    inverse[j, i] = cofactor / determinant;
                }
            }
            return new Matrix(inverse);
        }

        public enum ValidationResult
        {
            Valid,
            Error,
            No_Matrix
        }
    }
}