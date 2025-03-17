using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Media.Imaging;

namespace _13032025
{
    public static class Calculator
    {
        public static int Factorial(int n)
        {
            if (n < 0) return 1;
            int res = 1;

            for (int i = 2; i <= n; i++)
            {
                res *= i;
            }

            return res;
        }
        public static double GetAngle(double value)
        {
            return Math.Round(Math.PI * value / 180, 8);
        }
    }
}
