using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using TypingTutor.Models;
using TypingTutor.Services;

namespace TypingTutor.Controls
{
    public partial class ResultDialog : Window
    {
        public bool ShouldRetake { get; private set; }

        public ResultDialog()
        {
            InitializeComponent();
        }

        public void PopulateResults(TypingAttempt attempt, ExamProfile profile)
        {
            var user = AuthService.Instance.CurrentUser;
            TxtExamTitle.Text = $"{profile.ExamName} Scorecard";
            TxtPassageTitle.Text = $"Candidate: {user.DisplayName} (@{user.UserId}) | Passage: {attempt.PassageTitle}";

            CardNetWpm.Value = attempt.NetWpm.ToString("F1");
            CardNetWpm.Subtitle = $"Target: {profile.TargetWpm} WPM";

            CardGrossWpm.Value = attempt.GrossWpm.ToString("F1");
            CardAccuracy.Value = $"{attempt.AccuracyPercent:F1}%";
            CardAccuracy.Subtitle = $"Target: {profile.MinAccuracyPercent:F1}%";

            TxtTotalKeystrokes.Text = attempt.TotalKeystrokes.ToString("N0");
            TxtSubErrors.Text = attempt.SubstitutionErrors.ToString();
            TxtOmErrors.Text = attempt.OmissionErrors.ToString();
            TxtBackspaces.Text = attempt.BackspaceCount.ToString();

            if (attempt.PassedExam)
            {
                BdrResultBadge.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#059669"));
                TxtResultStatus.Text = "QUALIFIED";
            }
            else
            {
                BdrResultBadge.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DC2626"));
                TxtResultStatus.Text = "NOT QUALIFIED";
            }

            if (attempt.WeakKeyCounts != null && attempt.WeakKeyCounts.Any())
            {
                var keys = string.Join(", ", attempt.WeakKeyCounts.OrderByDescending(k => k.Value).Take(4).Select(k => $"'{k.Key}'"));
                TxtWeakKeysAdvice.Text = $"Key mistype focus: {keys}. Practice these characters in the Vocabulary Builder tab to boost accuracy.";
            }
            else
            {
                TxtWeakKeysAdvice.Text = "Outstanding performance! You kept errors minimal across all character keys.";
            }
        }

        private void BtnRetake_Click(object sender, RoutedEventArgs e)
        {
            ShouldRetake = true;
            DialogResult = true;
            Close();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            ShouldRetake = false;
            DialogResult = false;
            Close();
        }
    }
}
