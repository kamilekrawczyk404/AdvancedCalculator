using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace _13032025
{
    class ExpressionPreprocessor
    {
        public static string PreprocessExpression(string expression)
        {
            // Step 1: Replace "12%2" with "12%*2" (percentage handling)
            expression = Regex.Replace(expression, @"(\d+)%(\d+)", "$1%⨯$2");

            // Step 2: Replace "10π" or "10e" with "10*π" or "10*e" (implicit multiplication)
            expression = Regex.Replace(expression, @"((\d+)(π|e)|((π|e)(\d+)))", "$1⨯$2");

            // Step 3: Add implicit multiplication between adjacent parentheses or numbers
            expression = Regex.Replace(expression, @"\)(\d+|\()", ")⨯$1");

            return expression;
        }
    }
}
