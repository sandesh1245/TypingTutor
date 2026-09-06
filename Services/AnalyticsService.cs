using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using TypingTutor.Models;

namespace TypingTutor.Services
{
    public class AnalyticsService
    {
        private static AnalyticsService? _instance;
        public static AnalyticsService Instance => _instance ??= new AnalyticsService();

        private readonly List<TypingAttempt> _allAttempts = new List<TypingAttempt>();
        private readonly string _historyFilePath;

        public event Action? OnHistoryUpdated;

        public AnalyticsService()
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var folderPath = Path.Combine(appData, "ExamTypingTutor");
            Directory.CreateDirectory(folderPath);
            _historyFilePath = Path.Combine(folderPath, "attempt_history.json");

            LoadHistory();

            AuthService.Instance.OnUserChanged += (user) =>
            {
                OnHistoryUpdated?.Invoke();
            };
        }

        public string ActiveUserId => AuthService.Instance.CurrentUserId;

        public List<TypingAttempt> GetAttemptHistory()
        {
            string currentUid = ActiveUserId;
            return _allAttempts
                .Where(a => string.Equals(a.UserId, currentUid, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(a => a.Timestamp)
                .ToList();
        }

        public void LogAttempt(TypingAttempt attempt)
        {
            if (string.IsNullOrWhiteSpace(attempt.UserId))
            {
                attempt.UserId = ActiveUserId;
            }

            _allAttempts.Add(attempt);
            SaveHistory();
            OnHistoryUpdated?.Invoke();
        }

        public double GetBestNetWpm()
        {
            var userAttempts = GetAttemptHistory();
            if (!userAttempts.Any()) return 0;
            return Math.Round(userAttempts.Max(a => a.NetWpm), 1);
        }

        public double GetAverageNetWpm()
        {
            var userAttempts = GetAttemptHistory();
            if (!userAttempts.Any()) return 0;
            return Math.Round(userAttempts.Average(a => a.NetWpm), 1);
        }

        public double GetAverageAccuracy()
        {
            var userAttempts = GetAttemptHistory();
            if (!userAttempts.Any()) return 0;
            return Math.Round(userAttempts.Average(a => a.AccuracyPercent), 1);
        }

        public int GetTotalTestsCount()
        {
            return GetAttemptHistory().Count;
        }

        public Dictionary<char, int> GetTopWeakKeys(int count = 10)
        {
            var userAttempts = GetAttemptHistory();
            var weakMap = new Dictionary<char, int>();
            foreach (var attempt in userAttempts)
            {
                if (attempt.WeakKeyCounts != null)
                {
                    foreach (var kvp in attempt.WeakKeyCounts)
                    {
                        if (weakMap.ContainsKey(kvp.Key))
                        {
                            weakMap[kvp.Key] += kvp.Value;
                        }
                        else
                        {
                            weakMap[kvp.Key] = kvp.Value;
                        }
                    }
                }
            }

            return weakMap.OrderByDescending(kv => kv.Value).Take(count).ToDictionary(kv => kv.Key, kv => kv.Value);
        }

        public void ClearCurrentUserHistory()
        {
            string currentUid = ActiveUserId;
            _allAttempts.RemoveAll(a => string.Equals(a.UserId, currentUid, StringComparison.OrdinalIgnoreCase));
            SaveHistory();
            OnHistoryUpdated?.Invoke();
        }

        private void SaveHistory()
        {
            try
            {
                var json = JsonSerializer.Serialize(_allAttempts, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_historyFilePath, json);
            }
            catch { }
        }

        private void LoadHistory()
        {
            try
            {
                if (File.Exists(_historyFilePath))
                {
                    var json = File.ReadAllText(_historyFilePath);
                    var list = JsonSerializer.Deserialize<List<TypingAttempt>>(json);
                    if (list != null)
                    {
                        _allAttempts.Clear();
                        _allAttempts.AddRange(list);
                    }
                }
            }
            catch { }
        }
    }
}
