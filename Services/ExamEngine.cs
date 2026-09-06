using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;
using TypingTutor.Models;

namespace TypingTutor.Services
{
    public class ExamEngine
    {
        public TypingPassage TargetPassage { get; private set; }
        public ExamProfile Profile { get; private set; }

        public string TypedText { get; private set; } = string.Empty;
        public int ActiveCharIndex => TypedText.Length;

        public bool IsRunning { get; private set; }
        public bool IsCompleted { get; private set; }

        public TimeSpan RemainingTime { get; private set; }
        public TimeSpan ElapsedTime { get; private set; }

        public int TotalKeystrokes { get; private set; }
        public int CorrectKeystrokes { get; private set; }
        public int BackspaceCount { get; private set; }

        public int SubstitutionErrors { get; private set; }
        public int OmissionErrors { get; private set; }
        public int InsertionErrors { get; private set; }

        public Dictionary<char, int> WeakKeys { get; private set; } = new Dictionary<char, int>();
        public List<string> ErrorWords { get; private set; } = new List<string>();

        private DispatcherTimer? _timer;

        public event Action? OnStateChanged;
        public event Action<TypingAttempt>? OnExamCompleted;

        public ExamEngine(TypingPassage passage, ExamProfile profile)
        {
            TargetPassage = passage;
            Profile = profile;
            RemainingTime = TimeSpan.FromMinutes(profile.TimeLimitMinutes);
            ElapsedTime = TimeSpan.Zero;
        }

        public void Start()
        {
            if (IsRunning || IsCompleted) return;

            IsRunning = true;
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += Timer_Tick;
            _timer.Start();

            OnStateChanged?.Invoke();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (!IsRunning) return;

            ElapsedTime += TimeSpan.FromSeconds(1);

            if (Profile.TimeLimitMinutes > 0)
            {
                RemainingTime = TimeSpan.FromMinutes(Profile.TimeLimitMinutes) - ElapsedTime;
                if (RemainingTime <= TimeSpan.Zero)
                {
                    RemainingTime = TimeSpan.Zero;
                    FinishExam();
                    return;
                }
            }

            OnStateChanged?.Invoke();
        }

        public bool ProcessInput(string input)
        {
            if (!IsRunning && !IsCompleted)
            {
                Start();
            }

            if (IsCompleted) return false;

            // Handle backspace logic comparison
            if (input.Length < TypedText.Length)
            {
                // User pressed backspace
                if (Profile.BackspaceMode == BackspaceRule.Disabled)
                {
                    // Block backspace
                    return false;
                }

                if (Profile.BackspaceMode == BackspaceRule.RestrictedToWord)
                {
                    // Can only delete back to the start of the current word
                    int currentWordStart = TypedText.LastIndexOf(' ');
                    if (currentWordStart < 0) currentWordStart = 0;
                    else currentWordStart += 1;

                    if (input.Length < currentWordStart)
                    {
                        // Block deleting into previous word
                        return false;
                    }
                }

                BackspaceCount++;
                TypedText = input;
                RecalculateStats();
                OnStateChanged?.Invoke();
                return true;
            }

            // Normal typing input character added
            if (input.Length > TypedText.Length)
            {
                char typedChar = input[^1];
                TotalKeystrokes++;

                int targetIndex = input.Length - 1;
                if (targetIndex < TargetPassage.Content.Length)
                {
                    char expectedChar = TargetPassage.Content[targetIndex];
                    if (typedChar == expectedChar)
                    {
                        CorrectKeystrokes++;
                    }
                    else
                    {
                        // Record weak key error
                        if (WeakKeys.ContainsKey(expectedChar))
                        {
                            WeakKeys[expectedChar]++;
                        }
                        else
                        {
                            WeakKeys[expectedChar] = 1;
                        }
                    }
                }

                TypedText = input;
                RecalculateStats();

                if (TypedText.Length >= TargetPassage.Content.Length)
                {
                    FinishExam();
                }
                else
                {
                    OnStateChanged?.Invoke();
                }

                return true;
            }

            return true;
        }

        private void RecalculateStats()
        {
            // Reset error counters
            SubstitutionErrors = 0;
            OmissionErrors = 0;
            InsertionErrors = 0;
            ErrorWords.Clear();

            string target = TargetPassage.Content;
            int minLen = Math.Min(TypedText.Length, target.Length);

            for (int i = 0; i < minLen; i++)
            {
                if (TypedText[i] != target[i])
                {
                    SubstitutionErrors++;
                }
            }

            if (TypedText.Length < target.Length && IsCompleted)
            {
                OmissionErrors = target.Length - TypedText.Length;
            }
            else if (TypedText.Length > target.Length)
            {
                InsertionErrors = TypedText.Length - target.Length;
            }
        }

        public double CalculateGrossWpm()
        {
            double minutes = ElapsedTime.TotalMinutes;
            if (minutes <= 0.01) minutes = 0.01;
            return Math.Round((TotalKeystrokes / 5.0) / minutes, 1);
        }

        public double CalculateNetWpm()
        {
            double minutes = ElapsedTime.TotalMinutes;
            if (minutes <= 0.01) minutes = 0.01;

            int totalErrors = SubstitutionErrors + OmissionErrors + InsertionErrors;
            double netWords = (TotalKeystrokes / 5.0) - totalErrors;
            if (netWords < 0) netWords = 0;

            return Math.Round(netWords / minutes, 1);
        }

        public double CalculateAccuracy()
        {
            if (TotalKeystrokes == 0) return 100.0;
            double acc = (CorrectKeystrokes / (double)TotalKeystrokes) * 100.0;
            return Math.Round(Math.Max(0, Math.Min(100.0, acc)), 1);
        }

        public void FinishExam()
        {
            if (IsCompleted) return;

            IsCompleted = true;
            IsRunning = false;
            _timer?.Stop();

            RecalculateStats();

            double netWpm = CalculateNetWpm();
            double acc = CalculateAccuracy();
            bool passed = netWpm >= Profile.TargetWpm && acc >= Profile.MinAccuracyPercent;

            var attempt = new TypingAttempt
            {
                PassageId = TargetPassage.Id,
                PassageTitle = TargetPassage.Title,
                Language = TargetPassage.Language,
                ExamPresetName = Profile.ExamName,
                Timestamp = DateTime.Now,
                TimeSpentSeconds = ElapsedTime.TotalSeconds,
                GrossWpm = CalculateGrossWpm(),
                NetWpm = netWpm,
                AccuracyPercent = acc,
                TotalKeystrokes = TotalKeystrokes,
                CorrectKeystrokes = CorrectKeystrokes,
                BackspaceCount = BackspaceCount,
                SubstitutionErrors = SubstitutionErrors,
                OmissionErrors = OmissionErrors,
                InsertionErrors = InsertionErrors,
                WeakKeyCounts = new Dictionary<char, int>(WeakKeys),
                ErrorWords = new List<string>(ErrorWords),
                PassedExam = passed
            };

            AnalyticsService.Instance.LogAttempt(attempt);
            OnExamCompleted?.Invoke(attempt);
            OnStateChanged?.Invoke();
        }

        public void Pause()
        {
            if (!IsRunning) return;
            IsRunning = false;
            _timer?.Stop();
            OnStateChanged?.Invoke();
        }
    }
}
