using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TypingTutor.Models;
using TypingTutor.Services;

namespace TypingTutor.Views
{
    public partial class TypingGamesView : UserControl
    {
        private GameEngine _gameEngine = new GameEngine();

        public TypingGamesView()
        {
            InitializeComponent();

            _gameEngine.OnGameTick += Engine_OnGameTick;
            _gameEngine.OnGameOver += Engine_OnGameOver;

            InitializeLanguages();

            Focusable = true;
            Loaded += (s, e) => Focus();
            MouseDown += (s, e) => Focus();

            PreviewTextInput += TypingGamesView_PreviewTextInput;
            PreviewKeyDown += TypingGamesView_PreviewKeyDown;
        }

        private void InitializeLanguages()
        {
            CmbGameLanguage.Items.Clear();
            CmbGameLanguage.Items.Add(new ComboBoxItem { Content = "English", Tag = PassageLanguage.English });
            CmbGameLanguage.Items.Add(new ComboBoxItem { Content = "Hindi (Remington)", Tag = PassageLanguage.Hindi_Remington });
            CmbGameLanguage.Items.Add(new ComboBoxItem { Content = "Hindi (Inscript)", Tag = PassageLanguage.Hindi_Inscript });
            CmbGameLanguage.Items.Add(new ComboBoxItem { Content = "Hindi (Kruti Dev)", Tag = PassageLanguage.Hindi_Kruti });
            CmbGameLanguage.Items.Add(new ComboBoxItem { Content = "Marathi", Tag = PassageLanguage.Marathi });
            CmbGameLanguage.Items.Add(new ComboBoxItem { Content = "Punjabi (Raavi)", Tag = PassageLanguage.Punjabi_Raavi });
            CmbGameLanguage.Items.Add(new ComboBoxItem { Content = "Gujarati", Tag = PassageLanguage.Gujarati });
            CmbGameLanguage.Items.Add(new ComboBoxItem { Content = "Bengali", Tag = PassageLanguage.Bengali });
            CmbGameLanguage.Items.Add(new ComboBoxItem { Content = "Tamil", Tag = PassageLanguage.Tamil });
            CmbGameLanguage.Items.Add(new ComboBoxItem { Content = "Telugu", Tag = PassageLanguage.Telugu });
            CmbGameLanguage.SelectedIndex = 0;
        }

        private void CmbGameLanguage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CmbGameLanguage.SelectedItem is ComboBoxItem item && item.Tag is PassageLanguage lang)
            {
                _gameEngine.SetLanguage(lang);

                if (lang == PassageLanguage.English)
                {
                    BdrGameIndicBadge.Visibility = Visibility.Collapsed;
                }
                else
                {
                    BdrGameIndicBadge.Visibility = Visibility.Visible;
                    TxtGameIndicMode.Text = $"{lang.ToString().Replace('_', ' ')} Active";
                }
            }
            Focus();
        }

        private void BtnStartGame_Click(object sender, RoutedEventArgs e)
        {
            BdrOverlay.Visibility = Visibility.Collapsed;
            _gameEngine.StartNewGame();
            Focus();
        }

        private void Engine_OnGameTick()
        {
            Dispatcher.Invoke(() =>
            {
                TxtScore.Text = _gameEngine.Score.ToString("N0");
                TxtHighScore.Text = _gameEngine.HighScore.ToString("N0");

                string hearts = new string('❤', _gameEngine.Health);
                TxtHealth.Text = string.IsNullOrEmpty(hearts) ? "0" : hearts;
                TxtCurrentBuffer.Text = _gameEngine.CurrentInput;

                RenderFallingWords();
            });
        }

        private void RenderFallingWords()
        {
            GameCanvas.Children.Clear();
            var theme = ThemeService.Instance;

            foreach (var word in _gameEngine.ActiveWords)
            {
                var border = new Border
                {
                    Background = word.IsTargeted
                        ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0284C7"))
                        : new SolidColorBrush((Color)ColorConverter.ConvertFromString(theme.IsDarkTheme ? "#1E293B" : "#FFFFFF")),
                    BorderBrush = word.IsTargeted
                        ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#38BDF8"))
                        : new SolidColorBrush((Color)ColorConverter.ConvertFromString(theme.IsDarkTheme ? "#475569" : "#CBD5E1")),
                    BorderThickness = new Thickness(1.5),
                    CornerRadius = new CornerRadius(6),
                    Padding = new Thickness(12, 6, 12, 6)
                };

                var tb = new TextBlock
                {
                    Text = word.Text,
                    FontFamily = new FontFamily("Nirmala UI, Mangal, Segoe UI, sans-serif"),
                    Foreground = word.IsTargeted ? Brushes.White : theme.TextPrimaryBrush,
                    FontSize = 15,
                    FontWeight = FontWeights.Bold
                };

                border.Child = tb;
                Canvas.SetLeft(border, word.X);
                Canvas.SetTop(border, word.Y);

                GameCanvas.Children.Add(border);
            }
        }

        private void Engine_OnGameOver()
        {
            Dispatcher.Invoke(() =>
            {
                TxtOverlayTitle.Text = "GAME OVER";
                TxtOverlaySubtitle.Text = $"Final Score: {_gameEngine.Score:N0} points at Level {_gameEngine.Level} ({_gameEngine.Language}). Excellent reflex speed training!";
                BdrOverlay.Visibility = Visibility.Visible;
            });
        }

        private void TypingGamesView_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (!_gameEngine.IsRunning) return;

            if (e.Key == Key.Back)
            {
                _gameEngine.HandleBackspace();
                e.Handled = true;
            }
            else if (e.Key == Key.Escape || e.Key == Key.Space)
            {
                _gameEngine.ClearBuffer();
                e.Handled = true;
            }
        }

        private void TypingGamesView_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!_gameEngine.IsRunning || string.IsNullOrEmpty(e.Text)) return;

            char c = e.Text[0];

            // If an Indian language is active and standard ASCII character is typed, transliterate via IndicKeyboardService
            if (_gameEngine.Language != PassageLanguage.English && c <= 127 && !char.IsControl(c))
            {
                bool isShift = (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift;
                if (IndicKeyboardService.Instance.TryTranslate(c, isShift, _gameEngine.Language, out string translated))
                {
                    _gameEngine.ProcessTypedInput(translated);
                    e.Handled = true;
                    return;
                }
            }

            // Direct unicode / IME input or English input
            _gameEngine.ProcessTypedInput(e.Text);
            e.Handled = true;
        }
    }
}
