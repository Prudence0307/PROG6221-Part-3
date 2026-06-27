using System;

namespace CyberChatBot.Models
{
    public class ActivityLogEntry
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Action { get; set; }
        public string Details { get; set; }
        public DateTime Timestamp { get; set; }
    }
}