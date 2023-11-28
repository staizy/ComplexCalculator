using System;
using System.Collections.Generic;

namespace ComplexCalculator.Model
{
    public class CalculatorModel
    {
        private Dictionary<char, int> priority = new Dictionary<char, int>()
        {
            {'(', 0},
            {')', 0},
            {'+', 1},
            {'-', 1},
            {'*', 2},
            {'/', 2},
            {'s', 3},
            {'c', 3},
            {'t', 3},
            {'g', 3},
            {'~', 3},
            {'^', 3},
            {'q', 3},
            {'!', 3},
            {'%', 2}
        };

        private double GetNumberFromString(string str, ref int ind)
        {
            string num = "";
            bool foundDecimalPoint = false;

            while (ind < str.Length)
            {
                char c = str[ind];
                if (Char.IsDigit(c) || c == '.' || (c == ',' && !foundDecimalPoint))
                {
                    num += c;
                    if (c == '.' || c == ',')
                    {
                        foundDecimalPoint = true;
                    }
                    ind++;
                }
                else
                {
                    ind--;
                    break;
                }
            }
            return Double.Parse(num);
        }

        private double evaluate(double b, double a, char oper)
        {
            switch (oper)
            {
                case '+': return a + b;
                case '-': return a - b;
                case '*': return a * b;
                case '/': return a / b;
                case '^': return Math.Pow(a,b);
                case '%': return a % b;
                default:
                    throw new Exception("Неправильный оператор " + oper);
            }
        }

        private double evaluateFunc(double a, char oper)
        {
            switch (oper)
            {
                case 's': return Math.Round(Math.Sin(a), 8);
                case 'c': return Math.Round(Math.Cos(a), 8);
                case 't': return Math.Round(Math.Tan(a), 8);
                case 'g': return Math.Round((1.0 / Math.Tan(a)), 8);
                case 'q': return Math.Sqrt(a);
                case '!': return Factorial(a);
                default:
                    throw new Exception("Неправильный оператор " + oper);
            }
        }

        public double Factorial(double f)
        {
            if (f > 777 || f < 0) throw new ArgumentException("Невозможно посчитать факториал этого числа.");
            if (f == 0) return 1;
            else return f * Factorial(f - 1);
        }

        public double Calculate(string expression)
        {
            Stack<double> numbers = new Stack<double>();
            Stack<char> operators = new Stack<char>();
            for (int pos = 0; pos < expression.Length; pos++)
            {
                char c = expression[pos];
                if (Char.IsDigit(c))
                {
                    numbers.Push(GetNumberFromString(expression, ref pos));
                }
                else if (c == 'p' && expression[++pos] == 'i')
                {
                    numbers.Push(3.141592653589793);
                }
                else if (c == 'e')
                {
                    numbers.Push(2.718281828459045);
                }
                else if (c == '(')
                {
                    operators.Push('(');
                }
                else if (c == ')')
                {
                    while (operators.Count > 0 && operators.Peek() != '(')
                    {
                        if (operators.Peek() == 's' || operators.Peek() == 'c' || operators.Peek() == 't' || 
                            operators.Peek() == '!' || operators.Peek() == 'g' || operators.Peek() == 'q')
                        {
                            numbers.Push(evaluateFunc(numbers.Pop(), operators.Pop()));
                        }
                        else
                        {
                            numbers.Push(evaluate(numbers.Pop(), numbers.Pop(), operators.Pop()));
                        }
                    }
                    if (operators.Count > 0 && operators.Peek() == '(')
                    {
                        operators.Pop();
                    }
                    else
                    {
                        throw new InvalidOperationException("Несоответсвие операторов");
                    }
                }
                else if (c == '-' && (pos == 0 || expression[pos - 1] == '('))
                {
                    operators.Push('~');
                }
                else if ((c == 's' && expression[pos+1] == 'i' && expression[pos + 2] == 'n') ||
                         (c == 'c' && expression[pos+1] == 'o' && expression[pos + 2] == 's') ||
                         (c == 't' && expression[pos+1] == 'a' && expression[pos + 2] == 'n'))
                {
                    operators.Push(c);
                    pos += 2;
                }

                else if (c == '!')
                {
                    operators.Push('!');
                }

                else if ((c == 'c' && expression[pos+1] == 't' && expression[pos + 2] == 'g'))
                {
                    operators.Push(expression[pos+2]);
                    pos += 2;
                }

                else if ((c == 's' && expression[pos + 1] == 'q' && expression[pos + 2] == 'r' && expression[pos + 3] == 't'))
                {
                    operators.Push(expression[pos + 1]);
                    pos += 3;
                }

                else
                {
                    while (operators.Count > 0 && priority[operators.Peek()] >= priority[c])
                    {
                        if (operators.Peek() == 's' || operators.Peek() == 'c' || operators.Peek() == 't' || operators.Peek() == 'g' ||
                            operators.Peek() == '!' || operators.Peek() == 'g' || operators.Peek() == 'q')
                        {
                            numbers.Push(evaluateFunc(numbers.Pop(), operators.Pop()));
                        }
                        else
                        {
                            numbers.Push(evaluate(numbers.Pop(), numbers.Pop(), operators.Pop()));
                        }
                    }
                    operators.Push(c);
                }
            }

            while (operators.Count > 0)
            {
                if (operators.Peek() == '(' || operators.Peek() == ')')
                {
                    throw new InvalidOperationException("Несоответствие скобок.");
                }
                if (operators.Peek() == 's' || operators.Peek() == 'c' || operators.Peek() == 't' || operators.Peek() == 'g' ||
                    operators.Peek() == '!' || operators.Peek() == 'g' || operators.Peek() == 'q') 
                {
                    numbers.Push(evaluateFunc(numbers.Pop(), operators.Pop()));
                }
                else
                {
                    numbers.Push(evaluate(numbers.Pop(), numbers.Pop(), operators.Pop()));
                }
            }

            if (numbers.Count == 1)
            {
                return numbers.Pop();
            }
            else
            {
                throw new InvalidOperationException("Ошибка в выражении.");
            }
        }
    } 
}