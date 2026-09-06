using System;
using System.IO;
using System.Text.Json;
using TypingTutor.Models;

namespace TypingTutor.Services
{
    public class LicenseService
    {
        private static LicenseService? _instance;
        public static LicenseService Instance => _instance ??= new LicenseService();

        public UserLicense CurrentLicense { get; private set; } = new UserLicense();

        public event Action? OnLicenseChanged;

        private readonly string _licenseFilePath;

        public LicenseService()
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var folderPath = Path.Combine(appData, "ExamTypingTutor");
            Directory.CreateDirectory(folderPath);
            _licenseFilePath = Path.Combine(folderPath, "license.json");

            LoadLicense();
        }

        public bool ActivateLicense(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) return false;

            var cleanKey = key.Trim().ToUpperInvariant();
            if (cleanKey == UserLicense.ValidProKey || cleanKey.StartsWith("TYPING-PRO-"))
            {
                CurrentLicense.IsProActivated = true;
                CurrentLicense.LicenseKey = cleanKey;
                CurrentLicense.ActivationDate = DateTime.Now;
                CurrentLicense.RegisteredUser = "Licensed Candidate";

                SaveLicense();
                OnLicenseChanged?.Invoke();
                return true;
            }

            return false;
        }

        public void DeactivateLicense()
        {
            CurrentLicense.IsProActivated = false;
            CurrentLicense.LicenseKey = string.Empty;
            CurrentLicense.ActivationDate = null;
            CurrentLicense.RegisteredUser = "Offline Candidate";

            SaveLicense();
            OnLicenseChanged?.Invoke();
        }

        public bool CanAccessPassage(TypingPassage passage)
        {
            if (!passage.IsPremium) return true;
            return CurrentLicense.IsProActivated;
        }

        public bool CanAccessCustomPassages()
        {
            return CurrentLicense.IsProActivated;
        }

        public bool CanAccessAiDrills()
        {
            return CurrentLicense.IsProActivated;
        }

        private void LoadLicense()
        {
            try
            {
                if (File.Exists(_licenseFilePath))
                {
                    var json = File.ReadAllText(_licenseFilePath);
                    var license = JsonSerializer.Deserialize<UserLicense>(json);
                    if (license != null)
                    {
                        CurrentLicense = license;
                    }
                }
            }
            catch
            {
                CurrentLicense = new UserLicense();
            }
        }

        private void SaveLicense()
        {
            try
            {
                var json = JsonSerializer.Serialize(CurrentLicense, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_licenseFilePath, json);
            }
            catch { }
        }
    }
}
