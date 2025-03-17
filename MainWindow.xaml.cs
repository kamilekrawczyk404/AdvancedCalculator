using System;
using System.Configuration;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _13032025
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string MainInput = "";
        private string SecondaryInput = "";
        private bool NextClearMain = false;
        private bool NextClearAll = false;
        private bool AppendingOperators = true;
        private string Operation = "";

        private string[] Operators = { "+", "-", "⨯", "÷", "^" };
        private string[] SingleOperatorsPrefixes = { "√x", "sin", "cos", "tan", "log", "ln" };
        private string[] SingleOperatorsSuffixes = { "x!", "x¯¹", "%", "x²" };
        public MainWindow()
        {
            InitializeComponent();
        }

        private bool IsError()
        {
            return this.MainInput.ToLower().StartsWith("error");
        }

        private void ClearAll()
        {
            this.SecondaryInput = prevBlock.Text = "";
            this.MainInput = currentBlock.Text = "0";
        }

        private void CancelAnEntry()
        {
            this.MainInput = currentBlock.Text = "0";
            ClearButton.Content = "C";
            this.NextClearAll = false;
        }

        private void UpdateSingleParameterUI(string input, string function, bool after = false)
        {
            string withBrackets = "(" + input + ")";
            string additionalSpace = this.SecondaryInput.Length > 0 ? " " : "";

            string formatted = after ? (additionalSpace + withBrackets + function) : (additionalSpace + function + withBrackets);

            this.SecondaryInput = this.NextClearAll ? formatted : this.SecondaryInput + formatted;

            // Update UI
            currentBlock.Text = this.MainInput;
            prevBlock.Text = this.SecondaryInput;
        }

        private void HandleOtherOperators(string input, string op)
        {
            switch (op)
            {
                case "sin":
                    this.UpdateSingleParameterUI(input, "sin");
                    break;
                case "cos":
                    this.UpdateSingleParameterUI(input, "cos");
                    break;
                case "tan":
                    this.UpdateSingleParameterUI(input, "tan");
                    break;
                case "log":
                    this.UpdateSingleParameterUI(input, "log");
                    break;
                case "ln":
                    this.UpdateSingleParameterUI(input, "ln");              
                    break;
                case "√x":
                    this.UpdateSingleParameterUI(input, "√");
                    break;
                case "x!":
                    this.UpdateSingleParameterUI(input, "!", true);
                    break;
                case "x²":
                    this.UpdateSingleParameterUI(input, "²", true);
                    break;
                case "x¯¹":
                    this.UpdateSingleParameterUI(input, "¯¹", true);
                    break;
                default:
                    this.MainInput = "";
                    this.SecondaryInput = "";
                    break;
            }

            this.NextClearAll = true;
        }

        private void SolveUsingPolishNotation(string values)
        {
            List<string> tokens = MathTokenizer.Tokenize(values);
            List<string> rpn = ShuntingYard.ConvertToRPN(tokens);
            this.MainInput = RPNEvaluator.EvaluateRPN(rpn);
        }

        public void Calculate(object sender, RoutedEventArgs e)
        {
            // Prevent other actions when error occurs
            if (this.MainInput.Length == 0 || this.IsError())
            {
                return;
            }


            if (sender is Button btn)
            {
                string op = btn.Content.ToString();

                // If user click the "=" button, ensure that there is something to calculate
                if (op == "=" && (this.SecondaryInput.Length == 0 || (this.MainInput.Length >= 0 && this.SecondaryInput.EndsWith("="))))
                {
                    return;
                }

                // Prevent from calculating output when it's already calculated and when user has not provided the second value yet
                if (op != "%")
                {
                    // None of the operators has been clicked
                    // Append to the secondary input formatted values
                    if (this.SingleOperatorsPrefixes.Any(o => o == op) || this.SingleOperatorsSuffixes.Any(o => o == op))
                    {
                        this.HandleOtherOperators(this.MainInput, op);
                    }
                    // 
                    else if (this.Operators.Any(o => this.SecondaryInput.EndsWith(o)))
                    {
                        this.SecondaryInput += (" " + this.MainInput + " =");
                        this.SolveUsingPolishNotation(this.SecondaryInput);
                        this.NextClearAll = true;
                    }
                    //else if (this.SecondaryInput.Length > 0 && this.MainInput.Length > 0 && this.SecondaryInput.EndsWith("="))
                    //{
                    //    this.SecondaryInput = "";
                    //    this.HandleOtherOperators(this.MainInput, op);
                    //    this.SolveUsingPolishNotation(this.SecondaryInput);
                    //}
                    // Calculate secondary input
                    else
                    {
                        this.SecondaryInput += " =";
                        Trace.WriteLine(this.SecondaryInput);
                        this.SolveUsingPolishNotation(this.SecondaryInput);
                        this.NextClearAll = true;
                    }

                    this.Operation = op;
                    this.AppendingOperators = true;
                    this.NextClearMain = true;

                    // Update UI
                    this.prevBlock.Text = this.SecondaryInput;
                    this.currentBlock.Text = this.MainInput;
                }
                // Percentage logic
                else
                {
                    string value = this.MainInput + "%";

                    this.MainInput = value;
                    this.SecondaryInput += (" " + value);

                    this.currentBlock.Text = this.MainInput;
                    this.prevBlock.Text = this.SecondaryInput;
                }
            }
        }

        public void AppendOperator(object sender, RoutedEventArgs e)
        {
            // When error occurs, prevent other actions
            if (this.IsError())
            {
                return;
            }

            if (sender is Button btn)
            {
                string op = btn.Content.ToString() ?? "";
                // Power operator is represented with two characters
                // In the polish notation logic we need to convert in into "^"
                op = op == "xʸ" ? "^" : op;

                this.Operation = op;
                this.NextClearAll = false;
                this.NextClearMain = true;

                // Prevent appending an operator if the string is empty
                if (string.IsNullOrEmpty(this.MainInput))
                {
                    return;
                }

                // Something can be calculated
                if (this.SecondaryInput.EndsWith("="))
                {
                    this.SecondaryInput = this.MainInput + " " + op;
                }
                // User want to change current operator
                else if (this.Operators.Any(o => this.SecondaryInput.EndsWith(o)) && !this.AppendingOperators)
                {
                    this.SecondaryInput = this.SecondaryInput.Substring(0, this.SecondaryInput.Length - 2) + " " + op;
                }
                // Secondary input contains values, but it doesn't have any opeartion, append only it
                else if (this.SecondaryInput.Length > 0 && !this.Operators.Any(o => this.SecondaryInput.EndsWith(o)))
                {
                    this.SecondaryInput += (" " + op);
                }
                // Append multiple tokens
                else
                {
                    this.AppendingOperators = false;
                    this.SecondaryInput += (" " + this.MainInput + " " + op);
                }

                // Update UI
                currentBlock.Text = MainInput;
                prevBlock.Text = SecondaryInput;
            }
        }

        public void AppendValue(object sender, RoutedEventArgs e)
        {
            // Prevent appending values when error occurs
            if (this.IsError())
            {
                return;
            }

            if (sender is Button btn)
            {
                string value = btn.Content.ToString() ?? "";

                // User calculated some value, appending next value will clear the UI
                if (this.NextClearAll)
                {
                    this.NextClearAll = false;
                    this.ClearAll();
                }

                ClearButton.Content = "CE";
                this.AppendingOperators = true;

                // Prevent adding another "0" if already "0"
                if (value == "0" && this.MainInput == "0")
                {
                    return;
                }

                // Prevent adding multiple decimal points
                if (value == "," && this.MainInput.Contains(","))
                {
                    return;
                }

                // If it's a decimal point and the current value is empty, start with "0."
                if (value == ",")
                {
                    if (this.MainInput.Length == 0 || this.NextClearMain)
                    {
                        this.MainInput = "0,";
                        this.NextClearMain = false;
                    } else
                    {
                        this.MainInput += ",";
                    }
                }
                // If currentCalculation is "0", replace it with the new value
                else if (this.MainInput == "0" || NextClearMain)
                {
                    if (this.Operation == "=")
                    {
                        this.prevBlock.Text = this.SecondaryInput = "";
                        this.Operation = "";
                    }

                    NextClearMain = false;

                    this.MainInput = value;
                }
                // Otherwise, append the new value
                else
                {
                    this.MainInput += value;
                }

                // Update the UI
                currentBlock.Text = this.MainInput;
            }
        }

        public void EraseValue(object sender, RoutedEventArgs e)
        {
            int length = this.MainInput.Length;

            this.NextClearAll = false;
            this.NextClearMain = false;

            // Prevent erasing empty value
            if (this.MainInput == "0")
            {
                return;
            }

            // Prevent letting only minus sign
            if ((this.MainInput.StartsWith("-") && length == 2) || this.IsError())
            {
                this.MainInput = "0";
            }
            // Erase string by one character from the end
            else
            {
                this.MainInput = length > 1 ? this.MainInput.Substring(0, length - 1) : "0";
            }
                
            // Update UI
            currentBlock.Text = this.MainInput;
        }

        public void ChangeSign(object sender, RoutedEventArgs e)
        {
            // Prevent changing sign when inputs are empty, there is zero or error occurs
            if (this.MainInput == "0" || this.MainInput == "0." || this.MainInput.Length == 0 || this.IsError())
            {
                return;
            }

            string number;

            // Current number is negative, make it positive
            if (this.MainInput.StartsWith("-"))
            {
                number = this.MainInput.Substring(1, this.MainInput.Length - 1);
            }
            // From positive to negative
            else
            {
                number = "-" + this.MainInput;
            }

            // Update UI
            this.currentBlock.Text = this.MainInput = number;
        }

        public void Clear(object sender, RoutedEventArgs e)
        {
            if (ClearButton.Content.ToString() == "CE")
            {
                this.CancelAnEntry();
            } else
            {
                this.ClearAll();
            }
        }       
    }
}