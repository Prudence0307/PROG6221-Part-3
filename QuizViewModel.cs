using CyberChatBot.Models;
using CyberChatBot.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace CyberChatBot.App.ViewModels
{
    public class QuizViewModel : INotifyPropertyChanged
    {
        private readonly QuizService _quizService;
        private readonly ActivityLogService _logService;
        private readonly User _currentUser;

        private string _currentQuestion;
        private ObservableCollection<string> _currentOptions;
        private string _feedbackText;
        private string _feedbackColor;
        private bool _showFeedback;
        private bool _showExplanation;
        private string _explanation;
        private bool _showNextButton;
        private string _quizProgress;
        private string _scoreText;
        private bool _showResult;
        private string _resultTitle;
        private string _resultScore;
        private string _resultMessage;
        private string _resultColor;
        private string _selectedOption;

        public string CurrentQuestion
        {
            get => _currentQuestion;
            set { _currentQuestion = value; OnPropertyChanged(); }
        }

        public ObservableCollection<string> CurrentOptions
        {
            get => _currentOptions;
            set { _currentOptions = value; OnPropertyChanged(); }
        }

        public string FeedbackText
        {
            get => _feedbackText;
            set { _feedbackText = value; OnPropertyChanged(); }
        }

        public string FeedbackColor
        {
            get => _feedbackColor;
            set { _feedbackColor = value; OnPropertyChanged(); }
        }

        public bool ShowFeedback
        {
            get => _showFeedback;
            set { _showFeedback = value; OnPropertyChanged(); }
        }

        public bool ShowExplanation
        {
            get => _showExplanation;
            set { _showExplanation = value; OnPropertyChanged(); }
        }

        public string Explanation
        {
            get => _explanation;
            set { _explanation = value; OnPropertyChanged(); }
        }

        public bool ShowNextButton
        {
            get => _showNextButton;
            set { _showNextButton = value; OnPropertyChanged(); }
        }

        public string QuizProgress
        {
            get => _quizProgress;
            set { _quizProgress = value; OnPropertyChanged(); }
        }

        public string ScoreText
        {
            get => _scoreText;
            set { _scoreText = value; OnPropertyChanged(); }
        }

        public bool ShowResult
        {
            get => _showResult;
            set { _showResult = value; OnPropertyChanged(); }
        }

        public string ResultTitle
        {
            get => _resultTitle;
            set { _resultTitle = value; OnPropertyChanged(); }
        }

        public string ResultScore
        {
            get => _resultScore;
            set { _resultScore = value; OnPropertyChanged(); }
        }

        public string ResultMessage
        {
            get => _resultMessage;
            set { _resultMessage = value; OnPropertyChanged(); }
        }

        public string ResultColor
        {
            get => _resultColor;
            set { _resultColor = value; OnPropertyChanged(); }
        }

        public ICommand SelectOptionCommand { get; }
        public ICommand NextQuestionCommand { get; }
        public ICommand ResetQuizCommand { get; }

        public QuizViewModel(User user)
        {
            _currentUser = user;
            _quizService = new QuizService();
            _logService = new ActivityLogService();

            CurrentOptions = new ObservableCollection<string>();
            SelectOptionCommand = new RelayCommand(ExecuteSelectOption);
            NextQuestionCommand = new RelayCommand(ExecuteNextQuestion);
            ResetQuizCommand = new RelayCommand(ExecuteResetQuiz);

            LoadFirstQuestion();
        }

        private void LoadFirstQuestion()
        {
            try
            {
                _quizService.LoadQuestions();
                ShowNextQuestion();
                _logService.LogActivity(_currentUser.Id, "Quiz Started", "User started cybersecurity quiz");
            }
            catch (Exception ex)
            {
                _logService.LogActivity(_currentUser.Id, "Error Loading Quiz", ex.Message);
            }
        }

        private void ShowNextQuestion()
        {
            var question = _quizService.GetNextQuestion();
            if (question != null)
            {
                CurrentQuestion = question.Question;
                CurrentOptions.Clear();
                foreach (var option in question.Options)
                {
                    CurrentOptions.Add(option);
                }

                QuizProgress = $"Question {_quizService.GetCurrentQuestionIndex() + 1}/{_quizService.GetTotalQuestions()}";
                ScoreText = $"Score: {_quizService.GetScore()}/{_quizService.GetTotalQuestions()}";

                ShowFeedback = false;
                ShowExplanation = false;
                ShowNextButton = false;
                ShowResult = false;
                _selectedOption = null;
            }
            else
            {
                ShowQuizResult();
            }
        }

        private void ExecuteSelectOption(object parameter)
        {
            if (parameter is string selectedOption && !_quizService.IsQuestionAnswered(_quizService.GetCurrentQuestionIndex()))
            {
                _selectedOption = selectedOption;
                var currentQuestion = _quizService.GetNextQuestion();
                if (currentQuestion == null) return;

                var isCorrect = currentQuestion.CorrectAnswer.Equals(selectedOption, StringComparison.OrdinalIgnoreCase);

                // Record the answer
                _quizService.AnswerQuestion(currentQuestion.Id, selectedOption);

                // Show feedback
                FeedbackText = isCorrect ? "✅ Correct!" : $"❌ Incorrect. The correct answer was: {currentQuestion.CorrectAnswer}";
                FeedbackColor = isCorrect ? "#4CAF50" : "#F44336";
                Explanation = currentQuestion.Explanation;

                ShowFeedback = true;
                ShowExplanation = true;
                ShowNextButton = true;

                ScoreText = $"Score: {_quizService.GetScore()}/{_quizService.GetTotalQuestions()}";

                _logService.LogActivity(_currentUser.Id, "Quiz Answer",
                    $"Question: {currentQuestion.Question}, Correct: {isCorrect}");
            }
        }

        private void ExecuteNextQuestion()
        {
            ShowNextQuestion();
        }

        private void ExecuteResetQuiz()
        {
            _quizService.ResetQuiz();
            ShowResult = false;
            ShowNextButton = false;
            ShowFeedback = false;
            LoadFirstQuestion();
            _logService.LogActivity(_currentUser.Id, "Quiz Reset", "User reset the quiz");
        }

        private void ShowQuizResult()
        {
            var score = _quizService.GetScore();
            var total = _quizService.GetTotalQuestions();
            var percentage = (double)score / total * 100;

            ShowResult = true;
            ShowFeedback = false;
            ShowNextButton = false;

            if (percentage >= 80)
            {
                ResultTitle = "🎉 Excellent!";
                ResultColor = "#4CAF50";
                ResultMessage = "You have excellent cybersecurity knowledge! Keep up the great work!";
            }
            else if (percentage >= 60)
            {
                ResultTitle = "👍 Good Job!";
                ResultColor = "#FF9800";
                ResultMessage = "You have good cybersecurity knowledge. Review the topics you missed to improve!";
            }
            else
            {
                ResultTitle = "📚 Keep Learning!";
                ResultColor = "#F44336";
                ResultMessage = "Don't worry! Cybersecurity is a vast field. Review the material and try again!";
            }

            ResultScore = $"You scored {score} out of {total} ({percentage:F0}%)";
            QuizProgress = "Quiz Complete!";

            _logService.LogActivity(_currentUser.Id, "Quiz Completed", $"Score: {score}/{total}");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
