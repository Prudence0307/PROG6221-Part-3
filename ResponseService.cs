using CyberChatBot.App.ViewModels;
using System;
using System.Collections.Generic;

namespace CyberChatBot.Services
{
    public class ResponseService
    {
        private readonly Dictionary<string, List<string>> _keywordResponses;
        private readonly Random _random;

        public ResponseService()
        {
            _random = new Random();
            _keywordResponses = new Dictionary<string, List<string>>
            {
                ["hello"] = new List<string> { "Hello there!", "Hi! How can I help you?", "Greetings!" },
                ["help"] = new List<string> { "I can help you with tasks, quizzes, or just chat!", "What would you like help with?", "I'm here to assist you!" },
                ["task"] = new List<string> { "I can help you manage tasks. Would you like to add or view tasks?", "Let's work on your tasks!", "Task management is my specialty!" },
                ["weather"] = new List<string> { "I don't have weather data, but I can help with other things!", "Weather isn't my strength, but I'm good at tasks and quizzes!" },
                ["thank"] = new List<string> { "You're welcome!", "Happy to help!", "Anytime!" },
                ["goodbye"] = new List<string> { "Goodbye!", "See you later!", "Take care!" },
                ["game"] = new List<string> { "Would you like to play a cybersecurity quiz?", "Let's play a quiz! I have 10 questions ready." },
                ["quiz"] = new List<string> { "Ready for a quiz? Let's test your cybersecurity knowledge!", "I have a 10-question quiz prepared. Shall we start?" },
                ["name"] = new List<string> { "Nice to meet you!", "That's a cool name!", "I'll remember that!" },
                ["default"] = new List<string> { "I'm not sure about that. Try saying 'help' for options.", "Interesting! Can you tell me more?", "That's beyond my knowledge, but I'm learning!" }
            };
        }

        public string GetResponse(string userInput, UserData userData = null)
        {
            var input = userInput.ToLower().Trim();

            // Check for keyword matches
            foreach (var keyword in _keywordResponses.Keys)
            {
                if (input.Contains(keyword))
                {
                    var responses = _keywordResponses[keyword];
                    return responses[_random.Next(responses.Count)];
                }
            }

            // Return default response
            var defaults = _keywordResponses["default"];
            return defaults[_random.Next(defaults.Count)];
        }
    }
}
