using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TypingTutor.Models;
using TypingTutor.Services;

namespace TypingTutor.Views
{
    public partial class VocabBuilderView : UserControl
    {
        private VocabDrill? _activeDrill;
        private int _currentWordIndex = 0;
        private int _completedCount = 0;
        private int _errorCount = 0;

        private class VocabCategoryItem
        {
            public string DisplayName { get; set; } = string.Empty;
            public PassageLanguage? Language { get; set; }
            public override string ToString() => DisplayName;
        }

        public VocabBuilderView()
        {
            InitializeComponent();
            LoadDrills();
        }

        private void LoadDrills()
        {
            if (CmbDrillCategory != null)
            {
                CmbDrillCategory.Items.Clear();
                CmbDrillCategory.Items.Add(new VocabCategoryItem { DisplayName = "All Languages (All Drills)", Language = null });
                CmbDrillCategory.Items.Add(new VocabCategoryItem { DisplayName = "🇬🇧 English Drills", Language = PassageLanguage.English });
                CmbDrillCategory.Items.Add(new VocabCategoryItem { DisplayName = "🇮🇳 Hindi (Remington Gail) Drills", Language = PassageLanguage.Hindi_Remington });
                CmbDrillCategory.Items.Add(new VocabCategoryItem { DisplayName = "🇮🇳 Hindi (Inscript / Mangal) Drills", Language = PassageLanguage.Hindi_Inscript });
                CmbDrillCategory.Items.Add(new VocabCategoryItem { DisplayName = "🇮🇳 Hindi (Kruti Dev 010) Drills", Language = PassageLanguage.Hindi_Kruti });
                CmbDrillCategory.Items.Add(new VocabCategoryItem { DisplayName = "🇮🇳 Marathi (मराठी) Drills", Language = PassageLanguage.Marathi });
                CmbDrillCategory.Items.Add(new VocabCategoryItem { DisplayName = "🇮🇳 Punjabi (ਪੰਜਾਬੀ ਰਾਵੀ) Drills", Language = PassageLanguage.Punjabi_Raavi });
                CmbDrillCategory.Items.Add(new VocabCategoryItem { DisplayName = "🇮🇳 Gujarati (ગુજરાતી) Drills", Language = PassageLanguage.Gujarati });
                CmbDrillCategory.Items.Add(new VocabCategoryItem { DisplayName = "🇮🇳 Bengali (বাংলা) Drills", Language = PassageLanguage.Bengali });
                CmbDrillCategory.Items.Add(new VocabCategoryItem { DisplayName = "🇮🇳 Tamil (தமிழ்) Drills", Language = PassageLanguage.Tamil });
                CmbDrillCategory.Items.Add(new VocabCategoryItem { DisplayName = "🇮🇳 Telugu (తెలుగు) Drills", Language = PassageLanguage.Telugu });

                CmbDrillCategory.SelectedIndex = 0;
            }

            FilterDrills();
        }

        private void FilterDrills()
        {
            if (LstDrills == null) return;

            var selected = CmbDrillCategory?.SelectedItem as VocabCategoryItem;
            List<VocabDrill> drills;

            if (selected == null || selected.Language == null)
            {
                drills = VocabularyService.Instance.GetAllDrills();
                if (BdrVocabIndicBadge != null) BdrVocabIndicBadge.Visibility = Visibility.Collapsed;
            }
            else
            {
                drills = VocabularyService.Instance.GetDrills(selected.Language.Value);
                if (BdrVocabIndicBadge != null)
                {
                    bool isIndic = selected.Language.Value != PassageLanguage.English;
                    BdrVocabIndicBadge.Visibility = isIndic ? Visibility.Visible : Visibility.Collapsed;
                    TxtVocabIndicMode.Text = $"{selected.DisplayName.Replace("🇮🇳 ", "").Replace("🇬🇧 ", "").Replace(" Drills", "")} Active";
                }
            }

            LstDrills.ItemsSource = drills;
            if (drills.Any())
            {
                LstDrills.SelectedIndex = 0;
            }
        }

        private void CmbDrillCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterDrills();
        }

        private void LstDrills_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LstDrills.SelectedItem is VocabDrill drill)
            {
                _activeDrill = drill;
                _currentWordIndex = 0;
                _completedCount = 0;
                _errorCount = 0;

                if (BdrVocabIndicBadge != null)
                {
                    bool isIndic = drill.Language != PassageLanguage.English;
                    BdrVocabIndicBadge.Visibility = isIndic ? Visibility.Visible : Visibility.Collapsed;
                    TxtVocabIndicMode.Text = $"{drill.Language} Active";
                }

                UpdateWordTarget();
            }
        }

        private void UpdateWordTarget()
        {
            if (_activeDrill == null || !_activeDrill.WordList.Any()) return;

            if (_currentWordIndex >= _activeDrill.WordList.Count)
            {
                _currentWordIndex = 0;
            }

            TxtCurrentWord.Text = _activeDrill.WordList[_currentWordIndex];

            var nextThree = _activeDrill.WordList.Skip(_currentWordIndex + 1).Take(3);
            TxtNextWords.Text = "Next: " + string.Join(", ", nextThree);

            TxtCompletedCount.Text = $"{_completedCount} / {_activeDrill.WordList.Count}";
            TxtWordErrors.Text = _errorCount.ToString();
            TxtVocabInput.Text = string.Empty;
        }

        private void TxtVocabInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space || e.Key == Key.Enter)
            {
                string input = TxtVocabInput.Text.Trim();
                string expected = TxtCurrentWord.Text.Trim();

                if (input.Equals(expected, StringComparison.OrdinalIgnoreCase))
                {
                    _completedCount++;
                    _currentWordIndex++;
                }
                else
                {
                    _errorCount++;
                }

                UpdateWordTarget();
                e.Handled = true;
            }
        }

        private void TxtVocabInput_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (_activeDrill == null || _activeDrill.Language == PassageLanguage.English)
                return;

            if (e.Text.Length > 0 && e.Text[0] > 127)
                return;

            if (e.Text.Length > 0)
            {
                char rawChar = e.Text[0];
                bool isShift = (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift;

                if (IndicKeyboardService.Instance.TryTranslate(rawChar, isShift, _activeDrill.Language, out string translated))
                {
                    e.Handled = true;
                    int caret = TxtVocabInput.CaretIndex;
                    TxtVocabInput.Text = TxtVocabInput.Text.Insert(caret, translated);
                    TxtVocabInput.CaretIndex = caret + translated.Length;
                }
            }
        }
    }
}
