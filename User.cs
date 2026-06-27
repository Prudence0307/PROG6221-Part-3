using System;
using System.Collections.Generic;

namespace CyberChatBot.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CurrentSentiment { get; set; }
        public DateTime LastInteraction { get; set; }
        public Dictionary<string, string> Memory { get; set; } = new Dictionary<string, string>();
    }
}
