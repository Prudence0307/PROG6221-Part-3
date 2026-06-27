using System.Windows;
using System.Windows.Input;

namespace CyberChatBot.App.Views
{
    public partial class NamePromptWindow : Window
    {
        public string EnteredName { get; private set; }

        public NamePromptWindow()
        {
            InitializeComponent();
            NameTextBox.Focus();
        }

        private void EnterChat_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateInput())
            {
                EnteredName = NameTextBox.Text.Trim();
                DialogResult = true;
                Close();
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void NameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                EnterChat_Click(sender, e);
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text))
            {
                MessageBox.Show("Please enter a valid name.", "Input Required",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (NameTextBox.Text.Length < 2)
            {
                MessageBox.Show("Name must be at least 2 characters long.", "Invalid Name",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }
    }
}
