using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading; // Add this for DispatcherTimer

namespace testApp
{
    public partial class MainWindow : Window
    {
        private Calculator calculator;
        private DispatcherTimer clockTimer;

        public MainWindow()
        {
            InitializeComponent();
            calculator = new Calculator();

            // Set DataContext for binding
            txtCurrentEquation.DataContext = calculator;
            lstEquationHistory.DataContext = calculator;

            // Set DataContext for the result TextBox as well
            this.DataContext = calculator; // Setting DataContext for the whole window


            // Initialize the clock
            InitializeClock();
        }


        private void InitializeClock()
        {
            // Create a timer that updates every second
            clockTimer = new DispatcherTimer();
            clockTimer.Interval = TimeSpan.FromSeconds(1);
            clockTimer.Tick += ClockTimer_Tick;
            clockTimer.Start(); // Start the timer
        }

        private void ClockTimer_Tick(object sender, EventArgs e)
        {
            // Update the clock TextBlock with the current time
            txtClock.Text = DateTime.Now.ToString("HH:mm:ss");
        }
        private void Calculator_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Update the ListBox to reflect changes in EquationHistory
            if (e.PropertyName == nameof(Calculator.EquationHistory))
            {
                lstEquationHistory.Items.Refresh(); // Refresh the ListBox to update its items
                if (lstEquationHistory.Items.Count > 0)
                {
                    lstEquationHistory.ScrollIntoView(lstEquationHistory.Items[lstEquationHistory.Items.Count - 1]); // Scroll to the last item
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                string content = btn.Content.ToString();
                if (content == "+" || content == "-" || content == "*" || content == "/")
                {
                    calculator.SetOperation(content);
                }
                else if (content == "=")
                {
                    calculator.Calculate();
                }
                else if (content == "C")
                {
                    calculator.Clear();
                }
                else if (content == "CE")
                {
                    calculator.ClearEntry();
                }
                else if (content == "←")
                {
                    calculator.Backspace();
                }
                else
                {
                    calculator.AppendNumber(content);
                }
            }
        }
    }
}
