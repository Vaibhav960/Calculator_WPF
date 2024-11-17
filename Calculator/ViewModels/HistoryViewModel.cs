using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.ViewModels
{
    public class HistoryViewModel
    {
        // Observable collection to hold history of calculations
        public ObservableCollection<string> Calculations { get; set; }

        public HistoryViewModel()
        {
            Calculations = new ObservableCollection<string>();
        }

        // Method to add calculation to history
        public void AddToHistory(string calculation)
        {
            Calculations.Insert(0, calculation); 
        }

        // Method to clear the history
        public void ClearHistory()
        {
            Calculations.Clear();
        }
    }

}
