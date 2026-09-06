using System;

namespace TypingTutor.Models
{
    public enum PassageLanguage
    {
        English,
        Hindi_Remington,  // Mangal Remington Gail
        Hindi_Inscript,   // Mangal Inscript
        Hindi_Kruti,      // Kruti Dev 010
        Marathi,          // Devanagari Inscript
        Punjabi_Raavi,    // Gurmukhi Raavi
        Gujarati,         // Gujarati Inscript
        Bengali,          // Bengali Inscript
        Tamil,            // Tamil Inscript
        Telugu            // Telugu Inscript
    }

    public enum DifficultyLevel
    {
        Easy,
        Medium,
        Hard
    }

    public class TypingPassage
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N");
        public string Title { get; set; } = string.Empty;
        public string Category { get; set; } = "General"; // SSC, UPSSSC, High Court, Railway, State Govt, Custom
        public DifficultyLevel Difficulty { get; set; } = DifficultyLevel.Medium;
        public PassageLanguage Language { get; set; } = PassageLanguage.English;
        public string Content { get; set; } = string.Empty;
        public int WordCount => Content.Split(new[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Length;
        public int KeystrokeCount => Content.Length;
        public bool IsPremium { get; set; } = false;
        public bool IsCustomUserPassage { get; set; } = false;
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public string LanguageDisplayName => Language switch
        {
            PassageLanguage.English => "English",
            PassageLanguage.Hindi_Remington => "Hindi (Remington)",
            PassageLanguage.Hindi_Inscript => "Hindi (Inscript)",
            PassageLanguage.Hindi_Kruti => "Hindi (Kruti Dev)",
            PassageLanguage.Marathi => "Marathi",
            PassageLanguage.Punjabi_Raavi => "Punjabi (Raavi)",
            PassageLanguage.Gujarati => "Gujarati",
            PassageLanguage.Bengali => "Bengali",
            PassageLanguage.Tamil => "Tamil",
            PassageLanguage.Telugu => "Telugu",
            _ => Language.ToString()
        };
    }
}
