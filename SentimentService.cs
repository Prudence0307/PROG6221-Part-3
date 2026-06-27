using System;
using System.Collections.Generic;

namespace CyberChatBot.Services
{
    public class SentimentService
    {
        private readonly Dictionary<string, string> _sentimentKeywords;

        public SentimentService()
        {
            _sentimentKeywords = new Dictionary<string, string>
            {
                ["happy"] = "Positive",
                ["great"] = "Positive",
                ["good"] = "Positive",
                ["nice"] = "Positive",
                ["excellent"] = "Positive",
                ["sad"] = "Negative",
                ["bad"] = "Negative",
                ["terrible"] = "Negative",
                ["angry"] = "Negative",
                ["frustrated"] = "Negative",
                ["okay"] = "Neutral",
                ["fine"] = "Neutral"
            };
        }

        public string DetectSentiment(string text)
        {
            var input = text.ToLower();
            foreach (var kvp in _sentimentKeywords)
            {
                if (input.Contains(kvp.Key))
                {
                    return kvp.Value;
                }
            }
            return "Neutral";
        }
    }
}
