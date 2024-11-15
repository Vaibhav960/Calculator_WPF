using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Models
{
    public class CalculatorModel
    {
        // Method for addition
        public double Add(double a, double b) => a + b;

        // Method for subtraction
        public double Subtract(double a, double b) => a - b;

        // Method for multiplication
        public double Multiply(double a, double b) => a * b;

        // Method for division
        public double Divide(double a, double b)
        {
            if (b == 0)
                throw new DivideByZeroException("Cannot divide by zero.");
            return a / b;
        }
    }
}

