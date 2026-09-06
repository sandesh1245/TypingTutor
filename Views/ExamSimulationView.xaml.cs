using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using TypingTutor.Controls;
using TypingTutor.Models;
using TypingTutor.Services;

namespace TypingTutor.Views
{
    public partial class ExamSimulationView : UserControl
    {
        private ExamEngine? _engine;
        private ExamProfile _currentProfile = ExamProfile.GetPresetProfile(ExamPreset.SscCglChsl);
        private TypingPassage? _currentPassage;

        public ExamSimulationView()
        {
            InitializeComponent();
            LoadExamPresets();
            LoadPassages();
            ThemeService.Instance.OnThemeChanged += RenderPassageText;

            KeyboardControl.OnKeyClicked += (keyText) =>
            {
                if (string.IsNullOrEmpty(keyText)) return;
                int caret = TxtUserInput.CaretIndex;
                TxtUserInput.Text = TxtUserInput.Text.Insert(caret, keyText);
                TxtUserInput.CaretIndex = caret + keyText.Length;
                TxtUserInput.Focus();
            };
        }

        private void LoadExamPresets()
        {
            CmbExamPreset.Items.Clear();
            CmbExamPreset.Items.Add(ExamProfile.GetPresetProfile(ExamPreset.SscCglChsl));
            CmbExamPreset.Items.Add(ExamProfile.GetPresetProfile(ExamPreset.UpssscJuniorAssistant));
            CmbExamPreset.Items.Add(ExamProfile.GetPresetProfile(ExamPreset.HighCourtRoAro));
            CmbExamPreset.Items.Add(ExamProfile.GetPresetProfile(ExamPreset.RailwayNtpc));
            CmbExamPreset.Items.Add(ExamProfile.GetPresetProfile(ExamPreset.Custom));

            CmbExamPreset.DisplayMemberPath = "ExamName";
            CmbExamPreset.SelectedIndex = 0;
        }

        private void LoadPassages()
        {
            var passages = PassageRepository.Instance.GetAllPassages();
            CmbPassages.ItemsSource = passages;
            CmbPassages.DisplayMemberPath = "Title";
            if (passages.Any())
            {
                CmbPassages.SelectedIndex = 0;
            }
        }

        private void CmbExamPreset_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CmbExamPreset.SelectedItem is ExamProfile profile)
            {
                _currentProfile = profile;
                TxtBackspaceRuleBadge.Text = $"Backspace: {profile.BackspaceMode}";
                TxtTargetWpmBadge.Text = $"Target: {profile.TargetWpm} WPM";
                TxtTimeLimitBadge.Text = $"Time: {profile.TimeLimitMinutes} Mins";
                ResetEngine();
            }
        }

        private void CmbPassages_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CmbPassages.SelectedItem is TypingPassage passage)
            {
                _currentPassage = passage;
                KeyboardControl.SetLayout(passage.Language);

                if (passage.Language != PassageLanguage.English)
                {
                    BdrIndicMode.Visibility = Visibility.Visible;
                    TxtIndicMode.Text = $"⌨ {passage.Language} IME: Active";
                }
                else
                {
                    BdrIndicMode.Visibility = Visibility.Collapsed;
                }

                ResetEngine();
            }
        }

        public void SelectPassage(TypingPassage passage)
        {
            if (CmbPassages.ItemsSource is IEnumerable<TypingPassage> items)
            {
                var found = items.FirstOrDefault(p => p.Id == passage.Id);
                if (found != null)
                {
                    CmbPassages.SelectedItem = found;
                    return;
                }
            }
            CmbPassages.SelectedItem = passage;
        }

        private void ResetEngine()
        {
            if (_currentPassage == null) return;

            if (_engine != null)
            {
                _engine.OnStateChanged -= Engine_OnStateChanged;
                _engine.OnExamCompleted -= Engine_OnExamCompleted;
            }

            _engine = new ExamEngine(_currentPassage, _currentProfile);
            _engine.OnStateChanged += Engine_OnStateChanged;
            _engine.OnExamCompleted += Engine_OnExamCompleted;

            TxtUserInput.Text = string.Empty;
            TxtTimer.Text = $"{_currentProfile.TimeLimitMinutes:00}:00";
            TxtNetWpm.Text = "0.0";
            TxtGrossWpm.Text = "0.0";
            TxtAccuracy.Text = "100%";
            TxtKeystrokes.Text = "0";

            RenderPassageText();
        }

        private void Engine_OnStateChanged()
        {
            if (_engine == null) return;

            TxtTimer.Text = $"{_engine.RemainingTime.Minutes:00}:{_engine.RemainingTime.Seconds:00}";
            TxtNetWpm.Text = _engine.CalculateNetWpm().ToString("F1");
            TxtGrossWpm.Text = _engine.CalculateGrossWpm().ToString("F1");
            TxtAccuracy.Text = $"{_engine.CalculateAccuracy():F1}%";
            TxtKeystrokes.Text = _engine.TotalKeystrokes.ToString();

            RenderPassageText();
        }

        private void Engine_OnExamCompleted(TypingAttempt attempt)
        {
            Dispatcher.Invoke(() =>
            {
                var dialog = new ResultDialog
                {
                    Owner = Window.GetWindow(this)
                };
                dialog.PopulateResults(attempt, _currentProfile);
                if (dialog.ShowDialog() == true && dialog.ShouldRetake)
                {
                    ResetEngine();
                }
            });
        }

        private class TextToken
        {
            public string Text { get; set; } = string.Empty;
            public int StartIndex { get; set; }
            public int EndIndex => StartIndex + Text.Length;
            public bool IsWhitespace { get; set; }
        }

        private static List<TextToken> TokenizePassage(string text)
        {
            var tokens = new List<TextToken>();
            if (string.IsNullOrEmpty(text)) return tokens;

            int i = 0;
            while (i < text.Length)
            {
                int start = i;
                bool isWs = char.IsWhiteSpace(text[i]);
                while (i < text.Length && char.IsWhiteSpace(text[i]) == isWs)
                {
                    i++;
                }
                tokens.Add(new TextToken
                {
                    Text = text.Substring(start, i - start),
                    StartIndex = start,
                    IsWhitespace = isWs
                });
            }
            return tokens;
        }

        private void RenderPassageText()
        {
            if (_currentPassage == null || _engine == null) return;

            string fullText = _currentPassage.Content;
            string typed = _engine.TypedText;

            var doc = new FlowDocument();
            var para = new Paragraph();

            int typedLen = typed.Length;
            var theme = ThemeService.Instance;

            var tokens = TokenizePassage(fullText);

            // Determine which non-whitespace token is currently active
            int activeTokenIndex = -1;
            for (int i = 0; i < tokens.Count; i++)
            {
                if (!tokens[i].IsWhitespace)
                {
                    if (typedLen < tokens[i].EndIndex)
                    {
                        activeTokenIndex = i;
                        break;
                    }
                }
            }

            Run? activeRun = null;

            for (int i = 0; i < tokens.Count; i++)
            {
                var token = tokens[i];

                if (token.IsWhitespace)
                {
                    if (token.Text.Contains('\n'))
                    {
                        int count = token.Text.Count(c => c == '\n');
                        for (int k = 0; k < count; k++)
                        {
                            para.Inlines.Add(new LineBreak());
                        }
                    }
                    else
                    {
                        para.Inlines.Add(new Run(token.Text));
                    }
                    continue;
                }

                var run = new Run(token.Text);

                if (activeTokenIndex == -1 || i < activeTokenIndex)
                {
                    // Completed word
                    if (_currentProfile.HighlightMistakes)
                    {
                        string userWord = typed.Length >= token.EndIndex
                            ? typed.Substring(token.StartIndex, token.Text.Length)
                            : (typed.Length > token.StartIndex ? typed.Substring(token.StartIndex) : "");

                        if (userWord == token.Text)
                        {
                            run.Foreground = theme.SuccessBrush;
                        }
                        else
                        {
                            run.Foreground = theme.ErrorBrush;
                            run.TextDecorations = TextDecorations.Underline;
                        }
                    }
                    else
                    {
                        run.Foreground = theme.TextPrimaryBrush;
                    }
                }
                else if (i == activeTokenIndex)
                {
                    // Active current word
                    activeRun = run;

                    if (_currentProfile.IsHighlightEnabled)
                    {
                        run.Foreground = theme.AccentBrush;
                        run.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(theme.IsDarkTheme ? "#1E293B" : "#E0F2FE"));
                        run.FontWeight = FontWeights.Bold;
                    }
                    else
                    {
                        run.Foreground = theme.TextPrimaryBrush;
                        run.FontWeight = FontWeights.Bold;
                    }

                    if (typedLen < fullText.Length)
                    {
                        KeyboardControl.HighlightKey(fullText[typedLen]);
                    }
                }
                else
                {
                    // Upcoming untyped word
                    run.Foreground = theme.TextSecondaryBrush;
                }

                para.Inlines.Add(run);
            }

            doc.Blocks.Add(para);
            RtbPassage.Document = doc;

            if (activeTokenIndex == -1)
            {
                KeyboardControl.ResetHighlights();
            }
            else if (_currentProfile.AutoScroll && activeRun != null)
            {
                activeRun.BringIntoView();
            }
        }

        private void TxtUserInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_engine == null) return;

            string text = TxtUserInput.Text;
            _engine.ProcessInput(text);
        }

        private void TxtUserInput_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (_currentPassage == null || _currentPassage.Language == PassageLanguage.English)
                return;

            // If incoming character is already a Unicode Indic character (from external IME), allow directly
            if (e.Text.Length > 0 && e.Text[0] > 127)
            {
                return;
            }

            if (e.Text.Length > 0)
            {
                char rawChar = e.Text[0];
                bool isShift = (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift;

                if (IndicKeyboardService.Instance.TryTranslate(rawChar, isShift, _currentPassage.Language, out string translated))
                {
                    e.Handled = true;

                    int caret = TxtUserInput.CaretIndex;
                    TxtUserInput.Text = TxtUserInput.Text.Insert(caret, translated);
                    TxtUserInput.CaretIndex = caret + translated.Length;
                }
            }
        }

        private void TxtUserInput_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (_engine == null) return;

            if (e.Key == Key.Back && _currentProfile.BackspaceMode == BackspaceRule.Disabled)
            {
                // Intercept & suppress Backspace key if Disabled by exam rule!
                e.Handled = true;
            }
        }

        private void BtnFinishTest_Click(object sender, RoutedEventArgs e)
        {
            _engine?.FinishExam();
        }
    }
}
