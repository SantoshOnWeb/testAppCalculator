using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Text;

namespace testApp
{
    public class Calculator : INotifyPropertyChanged
    {
        private StringBuilder _equationBuilder = new StringBuilder();
        private ObservableCollection<string> _equationHistory = new ObservableCollection<string>();
        private double _result = 0;
        private bool _isNewEquation = true;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Result
        {
            get => _result.ToString();
            set
            {
                if (double.TryParse(value, out var result))
                {
                    _result = result;
                    OnPropertyChanged(nameof(Result));
                }
            }
        }

        public ObservableCollection<string> EquationHistory
        {
            get => _equationHistory;
            private set
            {
                _equationHistory = value;
                OnPropertyChanged(nameof(EquationHistory));
            }
        }

        public string CurrentEquation
        {
            get => _equationBuilder.ToString();
            set
            {
                _equationBuilder.Clear();
                _equationBuilder.Append(value);
                OnPropertyChanged(nameof(CurrentEquation));
            }
        }

        public void AppendNumber(string number)
        {
            if (_isNewEquation)
            {
                _equationBuilder.Clear();
                _isNewEquation = false;
            }
            _equationBuilder.Append(number);
            OnPropertyChanged(nameof(CurrentEquation));
        }

        public void SetOperation(string operation)
        {
            if (!_isNewEquation)
            {
                _equationBuilder.Append($" {operation} ");
                OnPropertyChanged(nameof(CurrentEquation));
            }
        }

        public void Clear()
        {
            _equationBuilder.Clear();
            _result = 0;
            OnPropertyChanged(nameof(Result));
            OnPropertyChanged(nameof(CurrentEquation));
            _isNewEquation = true;
        }

        public void ClearEntry()
        {
            if (_equationBuilder.Length > 0)
            {
                _equationBuilder.Clear();
                OnPropertyChanged(nameof(CurrentEquation));
            }
        }

        public void Calculate()
        {
            try
            {
                string currentEquation = _equationBuilder.ToString(); // Keep the equation unchanged
                _result = Convert.ToDouble(new DataTable().Compute(currentEquation, null)); // Compute the result

                OnPropertyChanged(nameof(Result)); // Trigger update for result textbox

                // Format the equation with the result for history
                string equationWithResult = $"{currentEquation} = {_result}";

                // Add the current equation and result to history
                _equationHistory.Add(equationWithResult);
                OnPropertyChanged(nameof(EquationHistory)); // Notify the UI to refresh the history

                _isNewEquation = true; // Mark the beginning of a new equation
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message); // Handle any errors
            }
        }




        public void Backspace()
        {
            if (_equationBuilder.Length > 0)
            {
                _equationBuilder.Remove(_equationBuilder.Length - 1, 1);
                OnPropertyChanged(nameof(CurrentEquation));
                _isNewEquation = false;
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
