using System;
using System.Windows.Media;

namespace TypingTutor.Services
{
    public class ThemeService
    {
        private static ThemeService? _instance;
        public static ThemeService Instance => _instance ??= new ThemeService();

        public bool IsDarkTheme { get; private set; } = false; // Pure Light Theme default

        public event Action? OnThemeChanged;

        public void ToggleTheme()
        {
            IsDarkTheme = !IsDarkTheme;
            OnThemeChanged?.Invoke();
        }

        public Brush WindowBackgroundBrush => GetBrush(IsDarkTheme ? "#090D16" : "#F8FAFC");
        public Brush CardBackgroundBrush => GetBrush(IsDarkTheme ? "#131722" : "#FFFFFF");
        public Brush HeaderBackgroundBrush => GetBrush(IsDarkTheme ? "#161925" : "#F1F5F9");
        public Brush SubCardBackgroundBrush => GetBrush(IsDarkTheme ? "#1E2433" : "#F8FAFC");
        public Brush BorderBrush => GetBrush(IsDarkTheme ? "#2A2F45" : "#E2E8F0");

        public Brush TextPrimaryBrush => GetBrush(IsDarkTheme ? "#F8FAFC" : "#0F172A");
        public Brush TextSecondaryBrush => GetBrush(IsDarkTheme ? "#94A3B8" : "#475569");
        public Brush TextMutedBrush => GetBrush(IsDarkTheme ? "#64748B" : "#64748B");

        public Brush AccentBrush => GetBrush(IsDarkTheme ? "#0EA5E9" : "#0284C7");
        public Brush SuccessBrush => GetBrush(IsDarkTheme ? "#10B981" : "#16A34A");
        public Brush ErrorBrush => GetBrush(IsDarkTheme ? "#EF4444" : "#DC2626");
        public Brush ActiveCursorBrush => GetBrush(IsDarkTheme ? "#F59E0B" : "#D97706");

        private static SolidColorBrush GetBrush(string hex)
        {
            var brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hex));
            brush.Freeze();
            return brush;
        }
    }
}
