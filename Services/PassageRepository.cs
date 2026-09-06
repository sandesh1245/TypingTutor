using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using TypingTutor.Models;

namespace TypingTutor.Services
{
    public class PassageRepository
    {
        private static PassageRepository? _instance;
        public static PassageRepository Instance => _instance ??= new PassageRepository();

        private readonly List<TypingPassage> _passages = new List<TypingPassage>();
        private readonly string _customPassagesFilePath;

        public event Action? OnPassagesUpdated;

        public PassageRepository()
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var folderPath = Path.Combine(appData, "ExamTypingTutor");
            Directory.CreateDirectory(folderPath);
            _customPassagesFilePath = Path.Combine(folderPath, "custom_passages.json");

            InitializeDefaultPassages();
            LoadCustomPassages();
        }

        public List<TypingPassage> GetAllPassages()
        {
            return _passages.ToList();
        }

        public List<TypingPassage> GetPassages(PassageLanguage lang, DifficultyLevel? diff = null, string? category = null)
        {
            var query = _passages.Where(p => p.Language == lang);

            if (diff.HasValue)
            {
                query = query.Where(p => p.Difficulty == diff.Value);
            }

            if (!string.IsNullOrEmpty(category) && category != "All")
            {
                query = query.Where(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
            }

            return query.ToList();
        }

        public TypingPassage? GetPassageById(string id)
        {
            return _passages.FirstOrDefault(p => p.Id == id);
        }

        public void AddCustomPassage(TypingPassage passage)
        {
            passage.IsCustomUserPassage = true;
            _passages.Add(passage);
            SaveCustomPassages();
            OnPassagesUpdated?.Invoke();
        }

        public bool DeleteCustomPassage(string id)
        {
            var passage = _passages.FirstOrDefault(p => p.Id == id && p.IsCustomUserPassage);
            if (passage != null)
            {
                _passages.Remove(passage);
                SaveCustomPassages();
                OnPassagesUpdated?.Invoke();
                return true;
            }
            return false;
        }

        private void SaveCustomPassages()
        {
            try
            {
                var customs = _passages.Where(p => p.IsCustomUserPassage).ToList();
                var json = JsonSerializer.Serialize(customs, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_customPassagesFilePath, json);
            }
            catch { }
        }

        private void LoadCustomPassages()
        {
            try
            {
                if (File.Exists(_customPassagesFilePath))
                {
                    var json = File.ReadAllText(_customPassagesFilePath);
                    var customs = JsonSerializer.Deserialize<List<TypingPassage>>(json);
                    if (customs != null)
                    {
                        foreach (var p in customs)
                        {
                            p.IsCustomUserPassage = true;
                            _passages.Add(p);
                        }
                    }
                }
            }
            catch { }
        }

        private void InitializeDefaultPassages()
        {
            var generated = PassageDataGenerator.GenerateAllPassages();
            _passages.AddRange(generated);
        }
    }
}
