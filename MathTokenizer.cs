using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace _13032025
{
    class MathTokenizer
    {
        public static string[] FormatConstants(string value, double toInsert)
        {
            string formatted = "";

            for (int i = 0; i < value.Length; i++)
            {
                string item = value[i].ToString();

                // item is not a special character (PI or Euler number)
                if (item != "e" && item != "π")
                {
                    formatted += item;
                    continue;
                }

                formatted += formatted.Length > 0 ? (" ⨯ " + toInsert.ToString()) : toInsert.ToString();

                // there are remaining characters, merge them using multiplication
                if (i != value.Length - 1)
                {
                    formatted += " ⨯ ";
                }
            }

            return formatted.Split(" ");
        }

        public static List<string> Tokenize(string expression)
        {
            Trace.WriteLine(expression);
            List<string> tokens = new List<string>();

            string pattern = @"-\(\d+(?:,\d+)?\)|-?\d*(?:,\d+)?(?:[eE][+-]?\d+)?π\d*(?:,\d+)?(?:[eE][+-]?\d+)?|-?\d*(?:,\d+)?(?:[eE][+-]?\d+)?e\d*(?:,\d+)?(?:[eE][+-]?\d+)?|-?\d+(?:,\d+)?(?:[eE][+-]?\d+)?|¯¹|[+÷\-⨯^%!²()]|√|(?:sin|cos|tan|log|ln)";

            foreach (Match match in Regex.Matches(expression, pattern))
            {
                if (match.Value.Contains("e"))
                {
                    string[] asTokens = MathTokenizer.FormatConstants(match.Value, Math.E);
                    foreach (string token in asTokens)
                    {
                        tokens.Add(token);
                    }
                }
                else if (match.Value.Contains("π"))
                {

                    string[] asTokens = MathTokenizer.FormatConstants(match.Value, Math.PI);
                    foreach (string token in asTokens)
                    {
                        tokens.Add(token);
                    }
                }
                else
                {
                    tokens.Add(match.Value);
                }
            }

            return tokens;
        }
    }
}
