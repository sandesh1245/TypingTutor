using System;

namespace TypingTutor.Models
{
    public class UserLicense
    {
        public bool IsProActivated { get; set; } = false;
        public string LicenseKey { get; set; } = string.Empty;
        public DateTime? ActivationDate { get; set; }
        public string RegisteredUser { get; set; } = "Offline Candidate";

        public static string ValidProKey => "TYPING-PRO-2026";
    }
}
