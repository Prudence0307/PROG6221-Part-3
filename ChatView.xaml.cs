using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace CyberChatBot.App.Views
{
    public partial class ChatView : UserControl
    {
        public ChatView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            throw new NotImplementedException();
        }

        private void InputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var viewModel = DataContext as ViewModels.ChatViewModel;
                viewModel?.SendMessageCommand.Execute(null);
            }
        }
    }
}