using CyberChatBot.Models;
using CyberChatBot.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace CyberChatBot.App.ViewModels
{
    public class ChatViewModel : INotifyPropertyChanged
    {
        private readonly ResponseService _responseService;
        private readonly SentimentService _sentimentService;
        private readonly NLPService _nlpService;
        private readonly ActivityLogService _logService;
        private readonly TaskService _taskService;
        private readonly User _currentUser;

        private string _userInput;
        private string _botStatus;
        private string _sentiment;
        private string _sentimentColor;

        public ObservableCollection<ChatMessage> Messages { get; set; }
        public ICommand SendMessageCommand { get; }

        public string UserInput
        {
            get => _userInput;
            set { _userInput = value; OnPropertyChanged(); }
        }

        public string BotStatus
        {
            get => _botStatus;
            set { _botStatus = value; OnPropertyChanged(); }
        }

        public string Sentiment
        {
            get => _sentiment;
            set { _sentiment = value; OnPropertyChanged(); }
        }

        public string SentimentColor
        {
            get => _sentimentColor;
            set { _sentimentColor = value; OnPropertyChanged(); }
        }

        public ChatViewModel(User user)
        {
            _currentUser = user;
            _responseService = new ResponseService();
            _sentimentService = new SentimentService();
            _nlpService = new NLPService();
            _logService = new ActivityLogService();
            _taskService = new TaskService();

            Messages = new ObservableCollection<ChatMessage>();
            SendMessageCommand = new RelayCommand(ExecuteSendMessage);

            BotStatus = $"Connected as {user.Name}";
            Sentiment = "😊 Neutral";
            SentimentColor = "#666";

            // Welcome message
            AddBotMessage($"Hello {user.Name}! I'm CyberChatBot. How can I help you today?");
            _logService.LogActivity(user.Id, "Chat Started", $"User {user.Name} started chat");
        }

        private void ExecuteSendMessage()
        {
            if (string.IsNullOrWhiteSpace(UserInput))
                return;

            var userMessage = UserInput.Trim();
            AddUserMessage(userMessage);
            _logService.LogActivity(_currentUser.Id, "User Message", userMessage);

            // Process the message
            ProcessMessage(userMessage);

            UserInput = string.Empty;
        }

        private void ProcessMessage(string message)
        {
            try
            {
                // Detect sentiment
                var sentiment = _sentimentService.DetectSentiment(message);
                Sentiment = GetSentimentEmoji(sentiment);
                SentimentColor = GetSentimentColor(sentiment);

                // Parse intent
                var intent = _nlpService.ExtractIntent(message);

                // Process based on intent
                string response = "";
                switch (intent["intent"])
                {
                    case "AddTask":
                        var taskDetails = _nlpService.ParseTask(message);
                        AddTaskFromChat(taskDetails);
                        response = "I've added your task! You can view it in the Tasks section.";
                        break;
                    case "ViewTasks":
                        response = "You can view all your tasks in the Tasks tab.";
                        break;
                    case "StartQuiz":
                        response = "Great! Head over to the Quiz tab to test your cybersecurity knowledge!";
                        break;
                    case "Help":
                        response = "I can help you with: \n• Managing tasks\n• Taking cybersecurity quizzes\n• General chat\n\nJust tell me what you need!";
                        break;
                    case "Exit":
                        response = "Goodbye! Come back soon!";
                        break;
                    default:
                        response = _responseService.GetResponse(message, new UserData { Name = _currentUser.Name });
                        break;
                }

                AddBotMessage(response);
                _logService.LogActivity(_currentUser.Id, "Bot Response", response);

                // Update user's last interaction
                _currentUser.LastInteraction = DateTime.Now;
                _currentUser.CurrentSentiment = sentiment;
            }
            catch (Exception ex)
            {
                AddBotMessage("I encountered an error processing your message. Please try again.");
                _logService.LogActivity(_currentUser.Id, "Error", ex.Message);
            }
        }

        private void AddTaskFromChat(Dictionary<string, string> taskDetails)
        {
            try
            {
                var task = new TaskItem
                {
                    Title = taskDetails["title"],
                    Description = taskDetails["description"],
                    ReminderDate = DateTime.Parse(taskDetails["reminder"]),
                    UserId = _currentUser.Id,
                    CreatedDate = DateTime.Now
                };
                _taskService.AddTask(task);
                _logService.LogActivity(_currentUser.Id, "Task Added", $"Title: {task.Title}");
            }
            catch (Exception ex)
            {
                _logService.LogActivity(_currentUser.Id, "Task Add Error", ex.Message);
            }
        }

        private void AddUserMessage(string text)
        {
            Messages.Add(new ChatMessage { Text = text, IsUser = true });
        }

        private void AddBotMessage(string text)
        {
            Messages.Add(new ChatMessage { Text = text, IsUser = false });
            BotStatus = $"Last response: {DateTime.Now:HH:mm:ss}";
        }

        private string GetSentimentEmoji(string sentiment)
        {
            return sentiment switch
            {
                "Positive" => "😊",
                "Negative" => "😔",
                _ => "😐"
            };
        }

        private string GetSentimentColor(string sentiment)
        {
            string v = sentiment switch
            {
                "Positive" => "#4CAF50",
                "Negative" => "#F44336",
                _ => "#666"
            };
            return v;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class ChatMessage
    {
        public string Text { get; set; }
        public bool IsUser { get; set; }
    }

    public class UserData
    {
        public string Name { get; set; }
        public string Sentiment { get; set; }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public RelayCommand(Action<object> executeSelectOption)
        {
        }

        public bool CanExecute(object parameter) => _canExecute?.Invoke() ?? true;
        public void Execute(object parameter) => _execute();
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}