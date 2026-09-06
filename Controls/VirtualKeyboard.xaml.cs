using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TypingTutor.Models;
using TypingTutor.Services;

namespace TypingTutor.Controls
{
    public partial class VirtualKeyboard : UserControl
    {
        private PassageLanguage _currentLanguage = PassageLanguage.English;

        public event Action<string>? OnKeyClicked;

        public VirtualKeyboard()
        {
            InitializeComponent();
            ThemeService.Instance.OnThemeChanged += ApplyTheme;
            ApplyTheme();
            AttachKeyClickHandlers();
        }

        private void AttachKeyClickHandlers()
        {
            Loaded += (s, e) =>
            {
                foreach (UIElement child in PnlKeyboardRows.Children)
                {
                    if (child is StackPanel row)
                    {
                        foreach (UIElement elem in row.Children)
                        {
                            if (elem is Border border)
                            {
                                border.Cursor = System.Windows.Input.Cursors.Hand;
                                border.MouseLeftButtonDown += (sender, args) =>
                                {
                                    if (border == Key_Space)
                                    {
                                        OnKeyClicked?.Invoke(" ");
                                        return;
                                    }

                                    if (border.Child is TextBlock tb && !string.IsNullOrEmpty(tb.Text))
                                    {
                                        // If space or modifier key
                                        if (border == Key_Backspace || border == Key_Tab || border == Key_CapsLock ||
                                            border == Key_LShift || border == Key_RShift || border == Key_Enter)
                                        {
                                            return;
                                        }

                                        // Take the first character of the key label
                                        string charToType = tb.Text.Split(' ')[0];
                                        OnKeyClicked?.Invoke(charToType);
                                    }
                                };
                            }
                        }
                    }
                }
            };
        }

        public void ApplyTheme()
        {
            var isDark = ThemeService.Instance.IsDarkTheme;

            if (BdrKeyboardOuter != null)
            {
                BdrKeyboardOuter.Background = isDark 
                    ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#161925"))
                    : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F1F5F9"));
                BdrKeyboardOuter.BorderBrush = isDark
                    ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2A2F45"))
                    : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#CBD5E1"));
            }

            if (TxtLayoutTitle != null)
            {
                TxtLayoutTitle.Foreground = isDark
                    ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F8FAFC"))
                    : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0F172A"));
            }

            ResetHighlights();
        }

        public void SetLayout(PassageLanguage language)
        {
            _currentLanguage = language;
            switch (language)
            {
                case PassageLanguage.Hindi_Remington:
                    TxtLayoutTitle.Text = "Hindi Mangal (Remington Gail Layout)";
                    TxtFingerGuide.Text = "Home Row: क म त न ल स य ह";
                    SetHindiRemingtonLabels();
                    break;
                case PassageLanguage.Hindi_Inscript:
                    TxtLayoutTitle.Text = "Hindi Mangal (Inscript Layout)";
                    TxtFingerGuide.Text = "Home Row: ो े ् ि ु र क त च";
                    SetHindiInscriptLabels();
                    break;
                case PassageLanguage.Hindi_Kruti:
                    TxtLayoutTitle.Text = "Hindi Kruti Dev 010 Layout";
                    TxtFingerGuide.Text = "Home Row: k e js g f r l n";
                    SetHindiKrutiLabels();
                    break;
                case PassageLanguage.Marathi:
                    TxtLayoutTitle.Text = "Marathi Devanagari Inscript Layout";
                    TxtFingerGuide.Text = "Home Row: ो े ् ि ु र क त च";
                    SetMarathiInscriptLabels();
                    break;
                case PassageLanguage.Punjabi_Raavi:
                    TxtLayoutTitle.Text = "Punjabi Gurmukhi (Raavi Layout)";
                    TxtFingerGuide.Text = "Home Row: ੋ ੇ ੍ ਿ ੁ ਰ ਕ ਤ ਚ";
                    SetPunjabiRaaviLabels();
                    break;
                case PassageLanguage.Gujarati:
                    TxtLayoutTitle.Text = "Gujarati Inscript Layout";
                    TxtFingerGuide.Text = "Home Row: ો ે ્ િ ુ ર ਕ ਤ ਚ";
                    SetGujaratiInscriptLabels();
                    break;
                case PassageLanguage.Bengali:
                    TxtLayoutTitle.Text = "Bengali Inscript Layout";
                    TxtFingerGuide.Text = "Home Row: ো ে ্ ি ু র ক ত চ";
                    SetBengaliInscriptLabels();
                    break;
                case PassageLanguage.Tamil:
                    TxtLayoutTitle.Text = "Tamil Inscript Layout";
                    TxtFingerGuide.Text = "Home Row: ா ே ் ி ு ர க த ச";
                    SetTamilInscriptLabels();
                    break;
                case PassageLanguage.Telugu:
                    TxtLayoutTitle.Text = "Telugu Inscript Layout";
                    TxtFingerGuide.Text = "Home Row: ో ే ్ ి ు ర క త చ";
                    SetTeluguInscriptLabels();
                    break;
                default:
                    TxtLayoutTitle.Text = "English QWERTY Layout";
                    TxtFingerGuide.Text = "Home Row: ASDF JKL;";
                    SetEnglishLabels();
                    break;
            }

            ResetHighlights();
        }

        public void HighlightKey(char keyChar)
        {
            ResetHighlights();

            bool needsShift = false;
            Border? targetBorder = null;

            if (_currentLanguage != PassageLanguage.English)
            {
                if (IndicKeyboardService.Instance.TryGetPhysicalKey(keyChar, _currentLanguage, out char physKey, out bool reqShift))
                {
                    needsShift = reqShift;
                    targetBorder = FindKeyBorder(physKey, out _);
                }
            }

            if (targetBorder == null)
            {
                targetBorder = FindKeyBorder(keyChar, out needsShift);
            }

            if (targetBorder != null)
            {
                var isDark = ThemeService.Instance.IsDarkTheme;
                var activeBg = isDark
                    ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0EA5E9"))
                    : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0284C7"));
                var activeBorder = isDark
                    ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#38BDF8"))
                    : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0369A1"));

                targetBorder.Background = activeBg;
                targetBorder.BorderBrush = activeBorder;
                if (targetBorder.Child is TextBlock tb)
                {
                    tb.Foreground = Brushes.White;
                    tb.FontWeight = FontWeights.Bold;
                }

                if (needsShift && Key_LShift != null)
                {
                    Key_LShift.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F59E0B"));
                    Key_LShift.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D97706"));
                    if (Key_LShift.Child is TextBlock shiftTb)
                    {
                        shiftTb.Foreground = Brushes.White;
                        shiftTb.FontWeight = FontWeights.Bold;
                    }
                }
            }
        }

        private Border? FindKeyBorder(char c, out bool needsShift)
        {
            needsShift = false;

            // 1. Space
            if (c == ' ') return Key_Space;

            // 2. Uppercase English letters
            if (c >= 'A' && c <= 'Z')
            {
                needsShift = true;
                return FindName("Key_" + c) as Border;
            }

            // 3. Lowercase English letters
            if (c >= 'a' && c <= 'z')
            {
                return FindName("Key_" + char.ToUpperInvariant(c)) as Border;
            }

            // 4. Digits
            if (c >= '0' && c <= '9')
            {
                return FindName("Key_" + c) as Border;
            }

            // 5. Shifted symbols on number row
            switch (c)
            {
                case '~': needsShift = true; return Key_Tilde;
                case '`': return Key_Tilde;
                case '!': needsShift = true; return Key_1;
                case '@': needsShift = true; return Key_2;
                case '#': needsShift = true; return Key_3;
                case '$': needsShift = true; return Key_4;
                case '%': needsShift = true; return Key_5;
                case '^': needsShift = true; return Key_6;
                case '&': needsShift = true; return Key_7;
                case '*': needsShift = true; return Key_8;
                case '(': needsShift = true; return Key_9;
                case ')': needsShift = true; return Key_0;
                case '_': needsShift = true; return Key_Minus;
                case '-': return Key_Minus;
                case '+': needsShift = true; return Key_Equal;
                case '=': return Key_Equal;
                case '{': needsShift = true; return Key_LBracket;
                case '[': return Key_LBracket;
                case '}': needsShift = true; return Key_RBracket;
                case ']': return Key_RBracket;
                case '|': needsShift = true; return Key_Backslash;
                case '\\': return Key_Backslash;
                case ':': needsShift = true; return Key_Semicolon;
                case ';': return Key_Semicolon;
                case '"': needsShift = true; return Key_Quote;
                case '\'': return Key_Quote;
                case '<': needsShift = true; return Key_Comma;
                case ',': return Key_Comma;
                case '>': needsShift = true; return Key_Period;
                case '.': return Key_Period;
                case '?': needsShift = true; return Key_Slash;
                case '/': return Key_Slash;
                case '\n':
                case '\r': return Key_Enter;
                case '\t': return Key_Tab;
            }

            // 6. Search key labels for Indic scripts / other layouts
            string str = c.ToString();
            foreach (UIElement child in PnlKeyboardRows.Children)
            {
                if (child is StackPanel row)
                {
                    foreach (UIElement elem in row.Children)
                    {
                        if (elem is Border b && b.Child is TextBlock tb)
                        {
                            if (tb.Text.Contains(str))
                            {
                                return b;
                            }
                        }
                    }
                }
            }

            return null;
        }

        public void ResetHighlights()
        {
            if (PnlKeyboardRows == null) return;

            var isDark = ThemeService.Instance.IsDarkTheme;

            var normalBg = isDark
                ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E2433"))
                : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));
            var normalBorder = isDark
                ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#334155"))
                : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#CBD5E1"));
            var normalText = isDark
                ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F8FAFC"))
                : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0F172A"));

            var wideBg = isDark
                ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#181D2A"))
                : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F1F5F9"));
            var wideBorder = isDark
                ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#475569"))
                : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#94A3B8"));
            var wideText = isDark
                ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#94A3B8"))
                : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#475569"));

            foreach (UIElement child in PnlKeyboardRows.Children)
            {
                if (child is StackPanel row)
                {
                    foreach (UIElement element in row.Children)
                    {
                        if (element is Border border)
                        {
                            bool isWide = border.Width > 45 || border == Key_Space;

                            border.Background = isWide ? wideBg : normalBg;
                            border.BorderBrush = isWide ? wideBorder : normalBorder;

                            if (border.Child is TextBlock tb)
                            {
                                tb.Foreground = isWide ? wideText : normalText;
                                tb.FontWeight = FontWeights.SemiBold;
                            }
                        }
                    }
                }
            }
        }

        private void SetEnglishLabels()
        {
            Txt_Q.Text = "Q"; Txt_W.Text = "W"; Txt_E.Text = "E"; Txt_R.Text = "R"; Txt_T.Text = "T";
            Txt_Y.Text = "Y"; Txt_U.Text = "U"; Txt_I.Text = "I"; Txt_O.Text = "O"; Txt_P.Text = "P";
            Txt_A.Text = "A"; Txt_S.Text = "S"; Txt_D.Text = "D"; Txt_F.Text = "F"; Txt_G.Text = "G";
            Txt_H.Text = "H"; Txt_J.Text = "J"; Txt_K.Text = "K"; Txt_L.Text = "L"; Txt_Semicolon.Text = "; :";
            Txt_Z.Text = "Z"; Txt_X.Text = "X"; Txt_C.Text = "C"; Txt_V.Text = "V"; Txt_B.Text = "B";
            Txt_N.Text = "N"; Txt_M.Text = "M";
        }

        private void SetHindiRemingtonLabels()
        {
            Txt_Q.Text = "ु"; Txt_W.Text = "ू"; Txt_E.Text = "म"; Txt_R.Text = "त"; Txt_T.Text = "ज";
            Txt_Y.Text = "ल"; Txt_U.Text = "न"; Txt_I.Text = "प"; Txt_O.Text = "व"; Txt_P.Text = "च";
            Txt_A.Text = "ो"; Txt_S.Text = "े"; Txt_D.Text = "्"; Txt_F.Text = "ि"; Txt_G.Text = "ह";
            Txt_H.Text = "ी"; Txt_J.Text = "र"; Txt_K.Text = "ा"; Txt_L.Text = "स"; Txt_Semicolon.Text = "य";
            Txt_Z.Text = "्र"; Txt_X.Text = "ग"; Txt_C.Text = "ब"; Txt_V.Text = "अ"; Txt_B.Text = "इ";
            Txt_N.Text = "द"; Txt_M.Text = "उ";
        }

        private void SetHindiInscriptLabels()
        {
            Txt_Q.Text = "ौ"; Txt_W.Text = "ै"; Txt_E.Text = "ा"; Txt_R.Text = "ी"; Txt_T.Text = "ू";
            Txt_Y.Text = "ब"; Txt_U.Text = "ह"; Txt_I.Text = "ग"; Txt_O.Text = "द"; Txt_P.Text = "ज";
            Txt_A.Text = "ो"; Txt_S.Text = "े"; Txt_D.Text = "्"; Txt_F.Text = "ि"; Txt_G.Text = "ु";
            Txt_H.Text = "प"; Txt_J.Text = "र"; Txt_K.Text = "क"; Txt_L.Text = "त"; Txt_Semicolon.Text = "च";
            Txt_Z.Text = "े"; Txt_X.Text = "ं"; Txt_C.Text = "म"; Txt_V.Text = "न"; Txt_B.Text = "व";
            Txt_N.Text = "ल"; Txt_M.Text = "स";
        }

        private void SetHindiKrutiLabels()
        {
            Txt_Q.Text = "ु"; Txt_W.Text = "ू"; Txt_E.Text = "म"; Txt_R.Text = "त"; Txt_T.Text = "ज";
            Txt_Y.Text = "ल"; Txt_U.Text = "न"; Txt_I.Text = "प"; Txt_O.Text = "व"; Txt_P.Text = "च";
            Txt_A.Text = "ks"; Txt_S.Text = "s"; Txt_D.Text = "्"; Txt_F.Text = "f"; Txt_G.Text = "g";
            Txt_H.Text = "h"; Txt_J.Text = "j"; Txt_K.Text = "k"; Txt_L.Text = "l"; Txt_Semicolon.Text = ";";
            Txt_Z.Text = "z"; Txt_X.Text = "x"; Txt_C.Text = "c"; Txt_V.Text = "v"; Txt_B.Text = "b";
            Txt_N.Text = "n"; Txt_M.Text = "m";
        }

        private void SetMarathiInscriptLabels()
        {
            SetHindiInscriptLabels(); // Uses Devanagari Inscript layout
        }

        private void SetPunjabiRaaviLabels()
        {
            Txt_Q.Text = "ੌ"; Txt_W.Text = "ੈ"; Txt_E.Text = "ਾ"; Txt_R.Text = "ੀ"; Txt_T.Text = "ੂ";
            Txt_Y.Text = "ਬ"; Txt_U.Text = "ਹ"; Txt_I.Text = "ਗ"; Txt_O.Text = "ਦ"; Txt_P.Text = "ਜ";
            Txt_A.Text = "ੋ"; Txt_S.Text = "ੇ"; Txt_D.Text = "੍"; Txt_F.Text = "ਿ"; Txt_G.Text = "ੁ";
            Txt_H.Text = "ਪ"; Txt_J.Text = "ਰ"; Txt_K.Text = "ਕ"; Txt_L.Text = "ਤ"; Txt_Semicolon.Text = "ਚ";
            Txt_Z.Text = "ੇ"; Txt_X.Text = "ੰ"; Txt_C.Text = "ਮ"; Txt_V.Text = "ਨ"; Txt_B.Text = "ਵ";
            Txt_N.Text = "ਲ"; Txt_M.Text = "ਸ";
        }

        private void SetGujaratiInscriptLabels()
        {
            Txt_Q.Text = "ૌ"; Txt_W.Text = "ૈ"; Txt_E.Text = "ા"; Txt_R.Text = "ી"; Txt_T.Text = "ૂ";
            Txt_Y.Text = "બ"; Txt_U.Text = "હ"; Txt_I.Text = "ગ"; Txt_O.Text = "દ"; Txt_P.Text = "જ";
            Txt_A.Text = "ો"; Txt_S.Text = "ે"; Txt_D.Text = "્"; Txt_F.Text = "િ"; Txt_G.Text = "ુ";
            Txt_H.Text = "પ"; Txt_J.Text = "ર"; Txt_K.Text = "ક"; Txt_L.Text = "ત"; Txt_Semicolon.Text = "ચ";
            Txt_Z.Text = "ે"; Txt_X.Text = "ં"; Txt_C.Text = "મ"; Txt_V.Text = "ન"; Txt_B.Text = "વ";
            Txt_N.Text = "લ"; Txt_M.Text = "સ";
        }

        private void SetBengaliInscriptLabels()
        {
            Txt_Q.Text = "ৌ"; Txt_W.Text = "ৈ"; Txt_E.Text = "া"; Txt_R.Text = "ী"; Txt_T.Text = "ূ";
            Txt_Y.Text = "ব"; Txt_U.Text = "হ"; Txt_I.Text = "গ"; Txt_O.Text = "দ"; Txt_P.Text = "জ";
            Txt_A.Text = "ো"; Txt_S.Text = "ে"; Txt_D.Text = "্"; Txt_F.Text = "ি"; Txt_G.Text = "ু";
            Txt_H.Text = "প"; Txt_J.Text = "র"; Txt_K.Text = "ক"; Txt_L.Text = "ত"; Txt_Semicolon.Text = "চ";
            Txt_Z.Text = "ে"; Txt_X.Text = "ং"; Txt_C.Text = "ম"; Txt_V.Text = "ন"; Txt_B.Text = "ব";
            Txt_N.Text = "ল"; Txt_M.Text = "স";
        }

        private void SetTamilInscriptLabels()
        {
            Txt_Q.Text = "ௌ"; Txt_W.Text = "ை"; Txt_E.Text = "ா"; Txt_R.Text = "ீ"; Txt_T.Text = "ூ";
            Txt_Y.Text = "ப"; Txt_U.Text = "ஹ"; Txt_I.Text = "க"; Txt_O.Text = "த"; Txt_P.Text = "ஜ";
            Txt_A.Text = "ோ"; Txt_S.Text = "ே"; Txt_D.Text = "்"; Txt_F.Text = "ி"; Txt_G.Text = "ு";
            Txt_H.Text = "ப"; Txt_J.Text = "ர"; Txt_K.Text = "க"; Txt_L.Text = "த"; Txt_Semicolon.Text = "ச";
            Txt_Z.Text = "ே"; Txt_X.Text = "ஂ"; Txt_C.Text = "ம"; Txt_V.Text = "ன"; Txt_B.Text = "வ";
            Txt_N.Text = "ல"; Txt_M.Text = "ஸ";
        }

        private void SetTeluguInscriptLabels()
        {
            Txt_Q.Text = "ౌ"; Txt_W.Text = "ై"; Txt_E.Text = "ా"; Txt_R.Text = "ీ"; Txt_T.Text = "ూ";
            Txt_Y.Text = "బ"; Txt_U.Text = "హ"; Txt_I.Text = "గ"; Txt_O.Text = "ద"; Txt_P.Text = "జ";
            Txt_A.Text = "ో"; Txt_S.Text = "ే"; Txt_D.Text = "్"; Txt_F.Text = "ి"; Txt_G.Text = "ు";
            Txt_H.Text = "ప"; Txt_J.Text = "ర"; Txt_K.Text = "క"; Txt_L.Text = "త"; Txt_Semicolon.Text = "చ";
            Txt_Z.Text = "ే"; Txt_X.Text = "ం"; Txt_C.Text = "మ"; Txt_V.Text = "న"; Txt_B.Text = "వ";
            Txt_N.Text = "ల"; Txt_M.Text = "స";
        }
    }
}
