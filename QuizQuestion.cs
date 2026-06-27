using System.Collections.Generic;

namespace CyberChatBot.Models
{
    public class QuizQuestion
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string Type { get; set; } // "MultipleChoice" or "TrueFalse"
        public List<string> Options { get; set; }
        public string CorrectAnswer { get; set; }
        public string Explanation { get; set; }
    }
}