using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _13032025
{
    class RPNEvaluator
    {
        public static string EvaluateRPN(List <string> rpnTokens)
        {
            Stack<double> stack = new Stack<double>();

            foreach(string token in rpnTokens)
            {
                if (double.TryParse(token, out double num))
                {
                    stack.Push(num);
                }
                else
                {
                    double b = stack.Pop();

                    if (token == "√") {
                        if (b >= 0)
                        {
                            stack.Push(Math.Sqrt(b));
                        } else
                        {
                            return "Error: Negative square root number";
                        }
                    }
                    else if (token == "^") stack.Push(Math.Pow(stack.Pop(), b));
                    else if (token == "²") stack.Push(Math.Pow(b, 2));
                    else if (token == "ln") {
                        if (b >= 0)
                        {
                            stack.Push(Math.Log(b));
                        } else
                        {
                            return "Error: Negative logarithm number";
                        }
                    }
                    else if (token == "log") {
                        if (b >= 0)
                        {
                            stack.Push(Math.Log10(b));
                        }
                        else
                        {
                            return "Error: Negative logarithm number";
                        }
                    }
                    else if (token == "sin") stack.Push(Math.Sin(Calculator.GetAngle(b)));
                    else if (token == "cos") stack.Push(Math.Cos(Calculator.GetAngle(b)));
                    else if (token == "tan") stack.Push(Math.Tan(Calculator.GetAngle(b)));
                    else if (token == "¯¹") stack.Push(Math.Round(1 / b, 8));
                    else if (token == "!") {
                        int asInt = (int)b;
                        if (b != asInt)
                        {
                            return "Error: Invalid factorial value";
                        } else {
                            stack.Push(Calculator.Factorial(asInt));
                        }
                    } 
                    else if (token == "%") stack.Push(stack.Count > 0 ? stack.Pop() * (b / 100) : b / 100);
                    else
                    {
                        double a = stack.Pop();

                        switch (token)
                        {
                            case "+": stack.Push(Math.Round(a + b, 8)); break;
                            case "-": stack.Push(Math.Round(a - b, 8)); break;
                            case "⨯": stack.Push(Math.Round(a * b, 8)); break;
                            case "^": stack.Push(Math.Round(Math.Pow(a, b), 8)); break;
                            case "÷":
                                if (b == 0) return "Error: Dividing by 0";
                                else stack.Push(Math.Round(a / b, 8)); break;
                        }
                    }
                }
            }

            return stack.Pop().ToString();
        }
    }
}
