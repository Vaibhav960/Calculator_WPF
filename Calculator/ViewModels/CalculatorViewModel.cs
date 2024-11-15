using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Calculator.Models;

namespace Calculator.ViewModels
{
    public class CalculatorViewModel : INotifyPropertyChanged
    {
        private CalculatorModel _calculatorModel = new CalculatorModel();
        private double _firstOperand;
        private string _operation;
        private bool _isNewEntry = true;
        private string _displayText = "0";
        private string _expressionDisplayText = ""; // Holds the expression, e.g., "6 + 3 ="
        public HistoryViewModel History { get; set; }
        public ICommand DeleteHistoryCommand { get; }
        public ICommand BackspaceCommand { get; }

        public string DisplayText
        {
            get => _displayText;
            set
            {
                _displayText = value;
                OnPropertyChanged(nameof(DisplayText));
            }
        }

        public string ExpressionDisplayText
        {
            get => _expressionDisplayText;
            set
            {
                _expressionDisplayText = value;
                OnPropertyChanged(nameof(ExpressionDisplayText));
            }
        }

      

        public RelayCommand NumberCommand { get; }
        public RelayCommand OperationCommand { get; }
        public RelayCommand ClearCommand { get; }
        public RelayCommand CalculateCommand { get; }

        public CalculatorViewModel()
        {
            // Initialize the history
            History = new HistoryViewModel();
            NumberCommand = new RelayCommand(ExecuteNumberCommand);
            OperationCommand = new RelayCommand(ExecuteOperationCommand);
            ClearCommand = new RelayCommand(ExecuteClearCommand);
            CalculateCommand = new RelayCommand(ExecuteCalculateCommand);
            History = new HistoryViewModel(); // Initialize HistoryViewModel
            DeleteHistoryCommand = new RelayCommand(DeleteHistory);
            BackspaceCommand = new RelayCommand(ExecuteBackspace, CanExecuteBackspace);

        }

        private void DeleteHistory(object parameter)
        {
            History.ClearHistory(); // Clears the history
        }

        private void ExecuteNumberCommand(object parameter)
        {
            if (_isNewEntry)
            {
                _displayText = parameter.ToString();
                _isNewEntry = false;
            }
            else
            {
                _displayText += parameter.ToString();
            }
            OnPropertyChanged(nameof(DisplayText));
        }

        private void ExecuteOperationCommand(object parameter)
        {
            // If an operation is already in progress and we haven't entered a new number, execute the current operation first
            if (!string.IsNullOrEmpty(_operation) && !_isNewEntry)
            {
                ExecuteCalculateCommand(null);  // Execute the current operation first
            }

            string symbol = parameter.ToString() switch
            {
                "Add" => "+",
                "Subtract" => "-",
                "Multiply" => "x",
                "Divide" => "/",
                _ => ""
            };

            // If no operation has been set yet, calculate the first operand
            if (string.IsNullOrEmpty(_operation))
            {
                _firstOperand = double.Parse(DisplayText);
            }

            // Set the new operation
            _operation = parameter.ToString();  // Update the operation with the new one

            // Update the expression display text
            ExpressionDisplayText = $"{_firstOperand} {symbol} ";

            _isNewEntry = true;

            // Trigger property changed notifications
            OnPropertyChanged(nameof(DisplayText));
            OnPropertyChanged(nameof(ExpressionDisplayText));
        }




        private void ExecuteClearCommand(object parameter)
        {
            DisplayText = "0";
            ExpressionDisplayText = "";
            _firstOperand = 0;
            _operation = null;
            _isNewEntry = true;
            OnPropertyChanged(nameof(DisplayText));
            OnPropertyChanged(nameof(ExpressionDisplayText));
        }


    
        private void ExecuteCalculateCommand(object parameter)
        {
            // If no operation, return early
            if (string.IsNullOrEmpty(_operation))
            {
                return;
            }

            // Second operand will be the current display text
            double secondOperand = double.Parse(DisplayText);
            double result = 0;

            switch (_operation)
            {
                case "Add":
                    result = _calculatorModel.Add(_firstOperand, secondOperand);
                    break;
                case "Subtract":
                    result = _calculatorModel.Subtract(_firstOperand, secondOperand);
                    break;
                case "Multiply":
                    result = _calculatorModel.Multiply(_firstOperand, secondOperand);
                    break;
                case "Divide":
                    try
                    {
                        result = _calculatorModel.Divide(_firstOperand, secondOperand);
                    }
                    catch (DivideByZeroException)
                    {
                        DisplayText = "Error";
                        return;
                    }
                    break;
            }

            // Update the expression display text (append to the expression if '=' was pressed before)
            if (!string.IsNullOrEmpty(ExpressionDisplayText))
            {
                ExpressionDisplayText += secondOperand + "=";
            }
            else
            {
                ExpressionDisplayText = $"{_firstOperand} {_operation} {secondOperand} =";
            }

            // Display the result in the display box
            DisplayText = result.ToString();


            // Add the current operation and result to history
            History.AddToHistory($"{ExpressionDisplayText} {result}");

            // Set the result as the first operand for the next operation
            _firstOperand = result;

            // Set the operation to null as it's now a result, awaiting the next operand or operator
            _operation = null;

            // Make sure the next number input resets the entry state
            _isNewEntry = true;

            // Notify property changes
            OnPropertyChanged(nameof(DisplayText));
            OnPropertyChanged(nameof(ExpressionDisplayText));
        }
        private void ExecuteBackspace(object parameter)
        {
            // Only allow backspace if it's not a result (i.e., you're entering a new expression)
            if (!_isNewEntry && !string.IsNullOrEmpty(DisplayText))
            {
                // Remove the last character from the display text
                if (DisplayText.Length > 1)
                {
                    DisplayText = DisplayText.Substring(0, DisplayText.Length - 1);
                }
                else
                {
                    // If only one character is left, reset it to "0"
                    DisplayText = "0";
                }

                // Trigger property change
                OnPropertyChanged(nameof(DisplayText));
            }
        }


        private bool CanExecuteBackspace(object parameter)
        {
            // Backspace is allowed only if we are not in the result state and there's text to delete
            return !_isNewEntry && !string.IsNullOrEmpty(DisplayText);
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
