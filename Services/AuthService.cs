using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using TypingTutor.Models;

namespace TypingTutor.Services
{
    public class AuthService
    {
        private static AuthService? _instance;
        public static AuthService Instance => _instance ??= new AuthService();

        private readonly string _usersFilePath;
        private readonly string _sessionFilePath;
        private readonly List<UserModel> _users = new List<UserModel>();

        public UserModel CurrentUser { get; private set; } = new UserModel
        {
            UserId = "guest",
            DisplayName = "Guest Candidate",
            IsGuest = true
        };

        public string CurrentUserId => CurrentUser.UserId;
        public bool IsLoggedIn => !CurrentUser.IsGuest;

        public event Action<UserModel>? OnUserChanged;

        public AuthService()
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var folderPath = Path.Combine(appData, "ExamTypingTutor");
            Directory.CreateDirectory(folderPath);

            _usersFilePath = Path.Combine(folderPath, "users.json");
            _sessionFilePath = Path.Combine(folderPath, "session.json");

            LoadUsers();
            RestoreLastSession();
        }

        public bool Register(string userId, string password, string displayName, out string error)
        {
            error = string.Empty;

            if (string.IsNullOrWhiteSpace(userId) || userId.Trim().Length < 3)
            {
                error = "User ID must be at least 3 characters long.";
                return false;
            }

            userId = userId.Trim();

            if (string.IsNullOrWhiteSpace(password) || password.Length < 4)
            {
                error = "Password must be at least 4 characters long.";
                return false;
            }

            if (_users.Any(u => u.UserId.Equals(userId, StringComparison.OrdinalIgnoreCase)))
            {
                error = $"User ID '{userId}' already exists. Please choose another or login.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(displayName))
            {
                displayName = userId;
            }

            string salt = GenerateSalt();
            string hash = HashPassword(password, salt);

            var newUser = new UserModel
            {
                UserId = userId,
                DisplayName = displayName.Trim(),
                PasswordSalt = salt,
                PasswordHash = hash,
                CreatedAt = DateTime.Now,
                LastLoginAt = DateTime.Now,
                IsGuest = false
            };

            _users.Add(newUser);
            SaveUsers();

            CurrentUser = newUser;
            SaveSession(newUser.UserId);
            OnUserChanged?.Invoke(CurrentUser);

            return true;
        }

        public bool Login(string userId, string password, out string error)
        {
            error = string.Empty;

            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(password))
            {
                error = "Please enter both User ID and Password.";
                return false;
            }

            userId = userId.Trim();
            var user = _users.FirstOrDefault(u => u.UserId.Equals(userId, StringComparison.OrdinalIgnoreCase));

            if (user == null)
            {
                error = "User ID not found. Please register first.";
                return false;
            }

            string computedHash = HashPassword(password, user.PasswordSalt);
            if (!computedHash.Equals(user.PasswordHash, StringComparison.Ordinal))
            {
                error = "Incorrect password. Please try again.";
                return false;
            }

            user.LastLoginAt = DateTime.Now;
            user.IsGuest = false;
            SaveUsers();

            CurrentUser = user;
            SaveSession(user.UserId);
            OnUserChanged?.Invoke(CurrentUser);

            return true;
        }

        public void ContinueAsGuest()
        {
            CurrentUser = new UserModel
            {
                UserId = "guest",
                DisplayName = "Guest Candidate",
                IsGuest = true
            };

            SaveSession("guest");
            OnUserChanged?.Invoke(CurrentUser);
        }

        public void Logout()
        {
            try
            {
                if (File.Exists(_sessionFilePath))
                {
                    File.Delete(_sessionFilePath);
                }
            }
            catch { }

            CurrentUser = new UserModel
            {
                UserId = "guest",
                DisplayName = "Guest Candidate",
                IsGuest = true
            };

            OnUserChanged?.Invoke(CurrentUser);
        }

        public List<UserModel> GetAllRegisteredUsers()
        {
            return _users.ToList();
        }

        private void SaveUsers()
        {
            try
            {
                var json = JsonSerializer.Serialize(_users, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_usersFilePath, json);
            }
            catch { }
        }

        private void LoadUsers()
        {
            try
            {
                if (File.Exists(_usersFilePath))
                {
                    var json = File.ReadAllText(_usersFilePath);
                    var list = JsonSerializer.Deserialize<List<UserModel>>(json);
                    if (list != null)
                    {
                        _users.Clear();
                        _users.AddRange(list);
                    }
                }
            }
            catch { }
        }

        private void SaveSession(string userId)
        {
            try
            {
                File.WriteAllText(_sessionFilePath, userId);
            }
            catch { }
        }

        private void RestoreLastSession()
        {
            try
            {
                if (File.Exists(_sessionFilePath))
                {
                    string savedId = File.ReadAllText(_sessionFilePath).Trim();
                    if (!string.IsNullOrEmpty(savedId) && !savedId.Equals("guest", StringComparison.OrdinalIgnoreCase))
                    {
                        var user = _users.FirstOrDefault(u => u.UserId.Equals(savedId, StringComparison.OrdinalIgnoreCase));
                        if (user != null)
                        {
                            user.IsGuest = false;
                            CurrentUser = user;
                            return;
                        }
                    }
                }
            }
            catch { }

            // Default to guest
            CurrentUser = new UserModel
            {
                UserId = "guest",
                DisplayName = "Guest Candidate",
                IsGuest = true
            };
        }

        private static string GenerateSalt()
        {
            byte[] saltBytes = new byte[16];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(saltBytes);
            return Convert.ToBase64String(saltBytes);
        }

        private static string HashPassword(string password, string salt)
        {
            using var sha256 = SHA256.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(password + salt);
            byte[] hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
