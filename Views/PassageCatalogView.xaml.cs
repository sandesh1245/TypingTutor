using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TypingTutor.Models;
using TypingTutor.Services;

namespace TypingTutor.Views
{
    public partial class PassageCatalogView : UserControl
    {
        public event Action<TypingPassage>? OnSelectPassageForPractice;

        public PassageCatalogView()
        {
            InitializeComponent();

            CmbLanguage.Items.Add(new ComboBoxItem { Content = "All Languages", Tag = (PassageLanguage?)null });
            CmbLanguage.Items.Add(new ComboBoxItem { Content = "English", Tag = (PassageLanguage?)PassageLanguage.English });
            CmbLanguage.Items.Add(new ComboBoxItem { Content = "Hindi (Remington)", Tag = (PassageLanguage?)PassageLanguage.Hindi_Remington });
            CmbLanguage.Items.Add(new ComboBoxItem { Content = "Hindi (Inscript)", Tag = (PassageLanguage?)PassageLanguage.Hindi_Inscript });
            CmbLanguage.Items.Add(new ComboBoxItem { Content = "Hindi (Kruti Dev)", Tag = (PassageLanguage?)PassageLanguage.Hindi_Kruti });
            CmbLanguage.Items.Add(new ComboBoxItem { Content = "Marathi", Tag = (PassageLanguage?)PassageLanguage.Marathi });
            CmbLanguage.Items.Add(new ComboBoxItem { Content = "Punjabi (Raavi)", Tag = (PassageLanguage?)PassageLanguage.Punjabi_Raavi });
            CmbLanguage.Items.Add(new ComboBoxItem { Content = "Gujarati", Tag = (PassageLanguage?)PassageLanguage.Gujarati });
            CmbLanguage.Items.Add(new ComboBoxItem { Content = "Bengali", Tag = (PassageLanguage?)PassageLanguage.Bengali });
            CmbLanguage.Items.Add(new ComboBoxItem { Content = "Tamil", Tag = (PassageLanguage?)PassageLanguage.Tamil });
            CmbLanguage.Items.Add(new ComboBoxItem { Content = "Telugu", Tag = (PassageLanguage?)PassageLanguage.Telugu });
            CmbLanguage.SelectedIndex = 0;

            // Import modal language options
            CmbImportLanguage.Items.Add(new ComboBoxItem { Content = "English", Tag = PassageLanguage.English });
            CmbImportLanguage.Items.Add(new ComboBoxItem { Content = "Hindi (Remington)", Tag = PassageLanguage.Hindi_Remington });
            CmbImportLanguage.Items.Add(new ComboBoxItem { Content = "Hindi (Inscript)", Tag = PassageLanguage.Hindi_Inscript });
            CmbImportLanguage.Items.Add(new ComboBoxItem { Content = "Hindi (Kruti Dev)", Tag = PassageLanguage.Hindi_Kruti });
            CmbImportLanguage.Items.Add(new ComboBoxItem { Content = "Marathi", Tag = PassageLanguage.Marathi });
            CmbImportLanguage.Items.Add(new ComboBoxItem { Content = "Punjabi (Raavi)", Tag = PassageLanguage.Punjabi_Raavi });
            CmbImportLanguage.Items.Add(new ComboBoxItem { Content = "Gujarati", Tag = PassageLanguage.Gujarati });
            CmbImportLanguage.Items.Add(new ComboBoxItem { Content = "Bengali", Tag = PassageLanguage.Bengali });
            CmbImportLanguage.Items.Add(new ComboBoxItem { Content = "Tamil", Tag = PassageLanguage.Tamil });
            CmbImportLanguage.Items.Add(new ComboBoxItem { Content = "Telugu", Tag = PassageLanguage.Telugu });
            CmbImportLanguage.SelectedIndex = 0;

            CmbCategory.Items.Add("All");
            CmbCategory.Items.Add("SSC CGL/CHSL");
            CmbCategory.Items.Add("UPSSSC Junior Assistant");
            CmbCategory.Items.Add("High Court RO/ARO");
            CmbCategory.Items.Add("Railway NTPC");
            CmbCategory.Items.Add("State Govt Exams");
            CmbCategory.Items.Add("Custom Import");
            CmbCategory.SelectedIndex = 0;

            CmbDifficulty.Items.Add("All");
            CmbDifficulty.Items.Add("Easy");
            CmbDifficulty.Items.Add("Medium");
            CmbDifficulty.Items.Add("Hard");
            CmbDifficulty.SelectedIndex = 0;

            LoadPassages();
        }

        private int _currentPage = 1;
        private const int PageSize = 24;
        private List<TypingPassage> _filteredPassages = new();

        private void Filter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _currentPage = 1;
            LoadPassages();
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            _currentPage = 1;
            LoadPassages();
        }

        private void LoadPassages()
        {
            if (LstPassageCards == null) return;

            var all = PassageRepository.Instance.GetAllPassages();

            if (CmbLanguage.SelectedItem is ComboBoxItem langItem && langItem.Tag is PassageLanguage selectedLang)
            {
                all = all.Where(p => p.Language == selectedLang).ToList();
            }

            string selectedCat = CmbCategory.SelectedItem?.ToString() ?? "All";
            if (selectedCat != "All")
            {
                all = all.Where(p => p.Category.Equals(selectedCat, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            string selectedDiff = CmbDifficulty.SelectedItem?.ToString() ?? "All";
            if (selectedDiff != "All" && Enum.TryParse<DifficultyLevel>(selectedDiff, out var diffEnum))
            {
                all = all.Where(p => p.Difficulty == diffEnum).ToList();
            }

            string query = TxtSearch?.Text?.Trim() ?? string.Empty;
            if (!string.IsNullOrEmpty(query))
            {
                all = all.Where(p => p.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                                     p.Category.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            _filteredPassages = all;
            TxtPassageCount.Text = $"Showing {_filteredPassages.Count:N0} Passages";

            UpdatePageView();
        }

        private void UpdatePageView()
        {
            int total = _filteredPassages.Count;
            int totalPages = Math.Max(1, (int)Math.Ceiling(total / (double)PageSize));

            if (_currentPage > totalPages) _currentPage = totalPages;
            if (_currentPage < 1) _currentPage = 1;

            var pageItems = _filteredPassages.Skip((_currentPage - 1) * PageSize).Take(PageSize).ToList();
            LstPassageCards.ItemsSource = pageItems;

            int startIdx = total == 0 ? 0 : ((_currentPage - 1) * PageSize) + 1;
            int endIdx = Math.Min(startIdx + pageItems.Count - 1, total);

            if (TxtPageSummary != null)
            {
                TxtPageSummary.Text = total == 0
                    ? "No passages match the selected filters"
                    : $"Showing {startIdx} to {endIdx} of {total:N0} official exam passages";
            }

            if (TxtCurrentPageInfo != null)
            {
                TxtCurrentPageInfo.Text = $"Page {_currentPage} of {totalPages}";
            }

            if (BtnPrevPage != null) BtnPrevPage.IsEnabled = _currentPage > 1;
            if (BtnNextPage != null) BtnNextPage.IsEnabled = _currentPage < totalPages;
        }

        private void BtnPrevPage_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                UpdatePageView();
            }
        }

        private void BtnNextPage_Click(object sender, RoutedEventArgs e)
        {
            int totalPages = Math.Max(1, (int)Math.Ceiling(_filteredPassages.Count / (double)PageSize));
            if (_currentPage < totalPages)
            {
                _currentPage++;
                UpdatePageView();
            }
        }

        private void BtnPracticePassage_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is TypingPassage passage)
            {
                if (passage.IsPremium && !LicenseService.Instance.CurrentLicense.IsProActivated)
                {
                    MessageBox.Show("This official passage is locked for Free users. Please upgrade to Pro to access 800+ premium passages!", "Pro Feature Locked", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                OnSelectPassageForPractice?.Invoke(passage);
            }
        }

        private void BtnImportCustom_Click(object sender, RoutedEventArgs e)
        {
            if (!LicenseService.Instance.CanAccessCustomPassages())
            {
                MessageBox.Show("Custom passage import is a Premium Pro feature. Upgrade to Pro to upload unlimited custom passages!", "Pro Upgrade Required", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            TxtCustomTitle.Text = string.Empty;
            TxtCustomContent.Text = string.Empty;
            BdrImportModal.Visibility = Visibility.Visible;
        }

        private void BtnCancelImport_Click(object sender, RoutedEventArgs e)
        {
            BdrImportModal.Visibility = Visibility.Collapsed;
        }

        private void BtnSaveCustomPassage_Click(object sender, RoutedEventArgs e)
        {
            string title = TxtCustomTitle.Text.Trim();
            string content = TxtCustomContent.Text.Trim();

            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(content))
            {
                MessageBox.Show("Please enter both passage title and text content.", "Validation Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var selectedLang = PassageLanguage.English;
            if (CmbImportLanguage.SelectedItem is ComboBoxItem langItem && langItem.Tag is PassageLanguage lang)
            {
                selectedLang = lang;
            }

            var newPassage = new TypingPassage
            {
                Title = title,
                Content = content,
                Category = "Custom Import",
                Difficulty = DifficultyLevel.Medium,
                Language = selectedLang,
                IsCustomUserPassage = true
            };

            PassageRepository.Instance.AddCustomPassage(newPassage);
            BdrImportModal.Visibility = Visibility.Collapsed;
            LoadPassages();
        }
    }
}
