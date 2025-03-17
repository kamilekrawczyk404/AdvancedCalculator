using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _13032025
{
    class MathEvaluator
    {

        public static readonly Dictionary<string, int> OperatorPrecedence = new Dictionary<string, int>
        {
            {"(", 0 }, {")", 0},
            {"!", 4 }, {"^", 4 }, {"√", 4}, {"sin", 4 }, { "cos", 4}, { "tan", 4}, { "ln", 4}, { "log", 4}, {"¯¹" ,4}, {"²", 4},
            {"⨯", 2 }, {"÷", 2}, {"%", 2},
            {"+", 1 }, {"-", 1}
        };

        public static int GetPrecedence(string op)
        {
            return OperatorPrecedence.ContainsKey(op) ? OperatorPrecedence[op] : -1;
        }
    }
}
