using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using TypingTutor.Models;
using TypingTutor.Services;

namespace TypingTutor.Views
{
    public partial class CoursesView : UserControl
    {
        private PassageLanguage _currentLanguage = PassageLanguage.English;
        private LessonModel? _currentLesson;

        private class LanguageComboItem
        {
            public string DisplayName { get; set; } = string.Empty;
            public PassageLanguage Language { get; set; }
            public override string ToString() => DisplayName;
        }

        public CoursesView()
        {
            InitializeComponent();
            PopulateLanguages();

            CourseKeyboard.OnKeyClicked += (keyText) =>
            {
                if (string.IsNullOrEmpty(keyText)) return;
                int caret = TxtLessonInput.CaretIndex;
                TxtLessonInput.Text = TxtLessonInput.Text.Insert(caret, keyText);
                TxtLessonInput.CaretIndex = caret + keyText.Length;
                TxtLessonInput.Focus();
            };
        }

        private void PopulateLanguages()
        {
            if (CmbCourseLanguage == null) return;

            CmbCourseLanguage.Items.Clear();
            CmbCourseLanguage.Items.Add(new LanguageComboItem { DisplayName = "🇬🇧 English QWERTY", Language = PassageLanguage.English });
            CmbCourseLanguage.Items.Add(new LanguageComboItem { DisplayName = "🇮🇳 Hindi (Remington Gail)", Language = PassageLanguage.Hindi_Remington });
            CmbCourseLanguage.Items.Add(new LanguageComboItem { DisplayName = "🇮🇳 Hindi (Inscript / Mangal)", Language = PassageLanguage.Hindi_Inscript });
            CmbCourseLanguage.Items.Add(new LanguageComboItem { DisplayName = "🇮🇳 Hindi (Kruti Dev 010)", Language = PassageLanguage.Hindi_Kruti });
            CmbCourseLanguage.Items.Add(new LanguageComboItem { DisplayName = "🇮🇳 Marathi (मराठी)", Language = PassageLanguage.Marathi });
            CmbCourseLanguage.Items.Add(new LanguageComboItem { DisplayName = "🇮🇳 Punjabi (ਪੰਜਾਬੀ ਰਾਵੀ)", Language = PassageLanguage.Punjabi_Raavi });
            CmbCourseLanguage.Items.Add(new LanguageComboItem { DisplayName = "🇮🇳 Gujarati (ગુજરાતી)", Language = PassageLanguage.Gujarati });
            CmbCourseLanguage.Items.Add(new LanguageComboItem { DisplayName = "🇮🇳 Bengali (বাংলা)", Language = PassageLanguage.Bengali });
            CmbCourseLanguage.Items.Add(new LanguageComboItem { DisplayName = "🇮🇳 Tamil (தமிழ்)", Language = PassageLanguage.Tamil });
            CmbCourseLanguage.Items.Add(new LanguageComboItem { DisplayName = "🇮🇳 Telugu (తెలుగు)", Language = PassageLanguage.Telugu });

            CmbCourseLanguage.SelectedIndex = 0;
        }

        private void CmbCourseLanguage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CmbCourseLanguage?.SelectedItem is LanguageComboItem item)
            {
                _currentLanguage = item.Language;

                if (BdrCourseIndicBadge != null)
                {
                    BdrCourseIndicBadge.Visibility = (_currentLanguage == PassageLanguage.English) ? Visibility.Collapsed : Visibility.Visible;
                    TxtCourseIndicMode.Text = $"{item.DisplayName.Replace("🇮🇳 ", "").Replace("🇬🇧 ", "")} Active";
                }

                CourseKeyboard?.SetLayout(_currentLanguage);
                LoadCourseLessons(_currentLanguage);
            }
        }

        private void LoadCourseLessons(PassageLanguage language)
        {
            if (LstLessons == null) return;
            var lessons = LessonService.Instance.GetLessonsByLanguage(language);
            LstLessons.ItemsSource = lessons;

            if (TxtCourseProgress != null)
            {
                TxtCourseProgress.Text = $"{lessons.Count} Structured Lessons";
            }

            if (lessons.Any())
            {
                LstLessons.SelectedIndex = 0;
            }
        }

        private void LstLessons_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LstLessons.SelectedItem is LessonModel lesson)
            {
                _currentLesson = lesson;
                TxtLessonTitle.Text = lesson.Title;
                TxtLessonDesc.Text = lesson.Description;
                TxtFingerGuide.Text = lesson.RecommendedFinger;

                TxtLessonInput.Text = string.Empty;
                RenderPracticeText();
            }
        }

        private class LessonTextToken
        {
            public string Text { get; set; } = string.Empty;
            public int StartIndex { get; set; }
            public int EndIndex => StartIndex + Text.Length;
            public bool IsWhitespace { get; set; }
        }

        private static List<LessonTextToken> TokenizePracticeText(string text)
        {
            var tokens = new List<LessonTextToken>();
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
                tokens.Add(new LessonTextToken
                {
                    Text = text.Substring(start, i - start),
                    StartIndex = start,
                    IsWhitespace = isWs
                });
            }
            return tokens;
        }

        private void RenderPracticeText()
        {
            if (_currentLesson == null) return;

            string target = _currentLesson.PracticeText;
            string typed = TxtLessonInput.Text;

            var doc = new FlowDocument();
            var para = new Paragraph();

            int typedLen = typed.Length;
            var tokens = TokenizePracticeText(target);

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
                    string userWord = typed.Length >= token.EndIndex
                        ? typed.Substring(token.StartIndex, token.Text.Length)
                        : (typed.Length > token.StartIndex ? typed.Substring(token.StartIndex) : "");

                    if (userWord == token.Text)
                    {
                        run.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#10B981"));
                    }
                    else
                    {
                        run.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EF4444"));
                        run.TextDecorations = TextDecorations.Underline;
                    }
                }
                else if (i == activeTokenIndex)
                {
                    // Active current word
                    activeRun = run;
                    run.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0284C7"));
                    run.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E0F2FE"));
                    run.FontWeight = FontWeights.Bold;

                    if (typedLen < target.Length)
                    {
                        CourseKeyboard?.HighlightKey(target[typedLen]);
                    }
                }
                else
                {
                    // Upcoming untyped word
                    run.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#64748B"));
                }

                para.Inlines.Add(run);
            }

            doc.Blocks.Add(para);
            RtbPracticeText.Document = doc;

            if (activeTokenIndex == -1)
            {
                CourseKeyboard?.ResetHighlights();
            }
            else if (activeRun != null)
            {
                activeRun.BringIntoView();
            }
        }

        private void TxtLessonInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            RenderPracticeText();
        }

        private void TxtLessonInput_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (_currentLanguage == PassageLanguage.English) return;

            if (e.Text.Length > 0 && e.Text[0] > 127)
            {
                return;
            }

            if (e.Text.Length > 0)
            {
                char rawChar = e.Text[0];
                bool isShift = (System.Windows.Input.Keyboard.Modifiers & System.Windows.Input.ModifierKeys.Shift) == System.Windows.Input.ModifierKeys.Shift;

                if (IndicKeyboardService.Instance.TryTranslate(rawChar, isShift, _currentLanguage, out string translated))
                {
                    e.Handled = true;
                    int caret = TxtLessonInput.CaretIndex;
                    TxtLessonInput.Text = TxtLessonInput.Text.Insert(caret, translated);
                    TxtLessonInput.CaretIndex = caret + translated.Length;
                }
            }
        }
    }
}
