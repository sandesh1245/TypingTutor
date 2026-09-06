using System;

namespace TypingTutor.Models
{
    public class UserModel
    {
        public string UserId { get; set; } = "guest";
        public string DisplayName { get; set; } = "Guest Candidate";
        public string PasswordHash { get; set; } = string.Empty;
        public string PasswordSalt { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime LastLoginAt { get; set; } = DateTime.Now;
        public bool IsGuest { get; set; } = true;

        public string AvatarInitials
        {
            get
            {
                if (IsGuest || string.IsNullOrWhiteSpace(DisplayName))
                    return "ET";

                var parts = DisplayName.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 1)
                {
                    return parts[0].Length >= 2 
                        ? parts[0].Substring(0, 2).ToUpperInvariant() 
                        : parts[0].ToUpperInvariant();
                }

                return $"{char.ToUpperInvariant(parts[0][0])}{char.ToUpperInvariant(parts[1][0])}";
            }
        }
    }
}
