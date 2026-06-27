using CyberChatBot.Data;
using CyberChatBot.Models;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CyberChatBot.Services
{
    public class QuizService
    {
        private readonly DatabaseHelper _dbHelper;
        private List<QuizQuestion> _questions;
        private int _currentIndex;
        private int _score;
        private Dictionary<int, bool> _answeredQuestions;

        public QuizService()
        {
            _dbHelper = new DatabaseHelper();
            _questions = new List<QuizQuestion>();
            _currentIndex = 0;
            _score = 0;
            _answeredQuestions = new Dictionary<int, bool>();
        }

        public void LoadQuestions()
        {
            _questions.Clear();
            try
            {
                string query = "SELECT * FROM QuizQuestions ORDER BY Id";
                var reader = _dbHelper.ExecuteReader(query);
                while (reader.Read())
                {
                    var optionsJson = reader.GetString("Options");
                    var options = JsonConvert.DeserializeObject<List<string>>(optionsJson);

                    _questions.Add(new QuizQuestion
                    {
                        Id = reader.GetInt32("Id"),
                        Question = reader.GetString("Question"),
                        Type = reader.GetString("Type"),
                        Options = options,
                        CorrectAnswer = reader.GetString("CorrectAnswer"),
                        Explanation = reader.GetString("Explanation")
                    });
                }
                _currentIndex = 0;
                _score = 0;
                _answeredQuestions.Clear();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading quiz questions: {ex.Message}");
            }
        }

        public QuizQuestion GetNextQuestion()
        {
            if (_questions.Count == 0)
            {
                LoadQuestions();
            }

            if (_currentIndex < _questions.Count)
            {
                return _questions[_currentIndex];
            }
            return null; // Quiz complete
        }

        public bool AnswerQuestion(int questionId, string answer)
        {
            var question = _questions.Find(q => q.Id == questionId);
            if (question == null || _answeredQuestions.ContainsKey(questionId))
                return false;

            bool isCorrect = question.CorrectAnswer.Equals(answer, StringComparison.OrdinalIgnoreCase);
            if (isCorrect)
            {
                _score++;
            }

            _answeredQuestions[questionId] = isCorrect;
            _currentIndex++;
            return true;
        }

        public string GetCurrentExplanation()
        {
            if (_currentIndex > 0 && _currentIndex <= _questions.Count)
            {
                var question = _questions[_currentIndex - 1];
                return question.Explanation;
            }
            return string.Empty;
        }

        public int GetScore() => _score;
        public int GetTotalQuestions() => _questions.Count;
        public int GetCurrentQuestionIndex() => _currentIndex;
        public bool IsQuizComplete() => _currentIndex >= _questions.Count;
        public void ResetQuiz()
        {
            _currentIndex = 0;
            _score = 0;
            _answeredQuestions.Clear();
        }

        public bool IsQuestionAnswered(int questionId)
        {
            return _answeredQuestions.ContainsKey(questionId);
        }
    }
}