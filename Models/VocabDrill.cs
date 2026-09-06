using System;
using System.Collections.Generic;

namespace TypingTutor.Models
{
    public class VocabDrill
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N");
        public string Title { get; set; } = string.Empty;
        public string Category { get; set; } = "High Frequency Words"; // High Frequency, Double Characters, Symbols & Numbers, Weak Key Target
        public PassageLanguage Language { get; set; } = PassageLanguage.English;
        public List<string> WordList { get; set; } = new List<string>();
        public string Description { get; set; } = string.Empty;
        public bool IsPremium { get; set; } = false;
    }
}
