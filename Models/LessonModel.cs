using System;

namespace TypingTutor.Models
{
    public class LessonModel
    {
        public string Id { get; set; } = string.Empty;
        public PassageLanguage Language { get; set; } = PassageLanguage.English;
        public int LessonNumber { get; set; }
        public string Category { get; set; } = "Home Row"; // Home Row, Top Row, Bottom Row, Shift, Numbers
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string TargetKeys { get; set; } = string.Empty;
        public string PracticeText { get; set; } = string.Empty;
        public string RecommendedFinger { get; set; } = "Left Index / Right Index";
        public bool IsCompleted { get; set; } = false;
        public double BestWpm { get; set; } = 0;
        public double BestAccuracy { get; set; } = 0;
    }
}
