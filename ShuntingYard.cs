using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _13032025
{
    class ShuntingYard
    {
        public static List<string> ConvertToRPN(List<string> tokens)
        {
            Stack<string> operators = new Stack<string>();
            List<string> output = new List<string>();

            foreach(string token in tokens)
            {
                if (double.TryParse(token, out _)){
                    output.Add(token);
                } else if (token == "(")
                {
                    operators.Push(token);
                } else if (token == ")")
                {
                    while (operators.Count > 0 && operators.Peek() != "(")
                    {
                        output.Add(operators.Pop());
                    }
                    operators.Pop();
                } else if (token == "√")
                {
                    operators.Push(token);
                } else
                {
                    while (operators.Count > 0 && MathEvaluator.GetPrecedence(token) <= MathEvaluator.GetPrecedence(operators.Peek()))
                    {
                        output.Add(operators.Pop());
                    }
                    operators.Push(token);
                }
            }

            while (operators.Count > 0)
            {
                output.Add(operators.Pop());
            }

            return output;
        }
    }
}
