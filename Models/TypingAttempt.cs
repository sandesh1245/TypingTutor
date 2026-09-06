using System;
using System.Collections.Generic;

namespace TypingTutor.Models
{
    public class TypingAttempt
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N");
        public string UserId { get; set; } = string.Empty;
        public string PassageId { get; set; } = string.Empty;
        public string PassageTitle { get; set; } = string.Empty;
        public PassageLanguage Language { get; set; } = PassageLanguage.English;
        public string ExamPresetName { get; set; } = "Standard Practice";
        public DateTime Timestamp { get; set; } = DateTime.Now;

        public double TimeSpentSeconds { get; set; }
        public double GrossWpm { get; set; }
        public double NetWpm { get; set; }
        public double AccuracyPercent { get; set; }

        public int TotalKeystrokes { get; set; }
        public int CorrectKeystrokes { get; set; }
        public int BackspaceCount { get; set; }

        public int TotalWordsTyped { get; set; }
        public int SubstitutionErrors { get; set; }
        public int OmissionErrors { get; set; }
        public int InsertionErrors { get; set; }

        public Dictionary<char, int> WeakKeyCounts { get; set; } = new Dictionary<char, int>();
        public List<string> ErrorWords { get; set; } = new List<string>();

        public bool PassedExam { get; set; }
    }
}
