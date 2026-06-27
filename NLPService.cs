using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CyberChatBot.Services
{
    public class NLPService
    {
        public Dictionary<string, string> ExtractIntent(string input)
        {
            var result = new Dictionary<string, string>();
            var text = input.ToLower().Trim();

            // Task extraction
            if (text.Contains("add task") || text.Contains("create task") || text.Contains("new task"))
            {
                result["intent"] = "AddTask";
                result["task"] = ExtractTaskDetails(text);
            }
            else if (text.Contains("view task") || text.Contains("show task") || text.Contains("list task"))
            {
                result["intent"] = "ViewTasks";
            }
            else if (text.Contains("quiz") || text.Contains("game") || text.Contains("play"))
            {
                result["intent"] = "StartQuiz";
            }
            else if (text.Contains("help"))
            {
                result["intent"] = "Help";
            }
            else if (text.Contains("logout") || text.Contains("exit") || text.Contains("goodbye"))
            {
                result["intent"] = "Exit";
            }
            else
            {
                result["intent"] = "Chat";
            }

            return result;
        }

        private string ExtractTaskDetails(string text)
        {
            // Simple extraction - would be enhanced with more sophisticated parsing
            var match = Regex.Match(text, @"task (?:titled?|named?) (.*?)(?: with reminder on | at |$)", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                return match.Groups[1].Value.Trim();
            }
            return "New Task";
        }

        public Dictionary<string, string> ParseTask(string text)
        {
            var result = new Dictionary<string, string>();
            var titleMatch = Regex.Match(text, @"title (.*?)(?: description | reminder |$)", RegexOptions.IgnoreCase);
            var descMatch = Regex.Match(text, @"description (.*?)(?: reminder |$)", RegexOptions.IgnoreCase);
            var reminderMatch = Regex.Match(text, @"reminder (.*?)(?:$)", RegexOptions.IgnoreCase);

            result["title"] = titleMatch.Success ? titleMatch.Groups[1].Value.Trim() : "Untitled Task";
            result["description"] = descMatch.Success ? descMatch.Groups[1].Value.Trim() : "";
            result["reminder"] = reminderMatch.Success ? reminderMatch.Groups[1].Value.Trim() : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd HH:mm");

            return result;
        }
    }
}
