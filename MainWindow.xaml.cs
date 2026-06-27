using System.Windows;
using CyberChatBot.App.Views;
using CyberChatBot.App.ViewModels;
using CyberChatBot.Models;
using System;

namespace CyberChatBot.App
{
    public partial class MainWindow : Window
    {
        public User CurrentUser { get; private set; }
        public object MainFrame { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void InitializeComponent()
        {
            throw new NotImplementedException();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ShowNamePrompt();
        }

        private void ShowNamePrompt()
        {
            var namePrompt = new NamePromptWindow();
            if (namePrompt.ShowDialog() == true)
            {
                CurrentUser = new User
                {
                    Id = 1, // In a real app, this would be from database
                    Name = namePrompt.EnteredName,
                    CurrentSentiment = "Neutral",
                    LastInteraction = DateTime.Now
                };

                var chatView = new ChatView();
                chatView.DataContext = new ChatViewModel(CurrentUser);
                MainFrame = chatView;
            }
            else
            {
                Application.Current.Shutdown();
            }
        }

        private void NavigateTo<TView>(object viewModel) where TView : new()
        {
            var view = new TView();
            if (view is FrameworkElement element)
            {
                element.DataContext = viewModel;
            }
            MainFrame = view;
        }

        private void ChatButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentUser != null)
            {
                var chatView = new ChatView();
                chatView.DataContext = new ChatViewModel(CurrentUser);
                MainFrame = chatView;
            }
        }

        private void TasksButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentUser != null)
            {
                var taskView = new TaskViewModel(CurrentUser);
                MainFrame = taskView;
            }
        }

        private void QuizButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentUser != null)
            {
                var quizView = new QuizView();
                quizView.DataContext = new QuizViewModel(CurrentUser);
                MainFrame = quizView;
            }
        }

        private void LogsButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentUser != null)
            {
                var logView = new ActivityLogView();
                logView.DataContext = new ActivityLogViewModel(CurrentUser);
                MainFrame = logView;
            }
        }
    }
}