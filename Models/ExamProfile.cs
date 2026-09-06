using System;

namespace TypingTutor.Models
{
    public enum BackspaceRule
    {
        Allowed,            // Unlimited backspace
        Disabled,           // SSC style - backspace completely blocked
        RestrictedToWord    // High Court style - allowed only within active word
    }

    public enum ExamPreset
    {
        SscCglChsl,
        UpssscJuniorAssistant,
        HighCourtRoAro,
        RailwayNtpc,
        Custom
    }

    public class ExamProfile
    {
        public ExamPreset Preset { get; set; }
        public string ExamName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int TargetWpm { get; set; } = 35;
        public double MinAccuracyPercent { get; set; } = 95.0;
        public BackspaceRule BackspaceMode { get; set; } = BackspaceRule.Allowed;
        public int TimeLimitMinutes { get; set; } = 10;
        public bool IsHighlightEnabled { get; set; } = true;
        public bool AutoScroll { get; set; } = true;
        public bool HighlightMistakes { get; set; } = true;
        public string PenaltyCalculationRule { get; set; } = "SSC Standard (5 Keystrokes = 1 Word)";

        public static ExamProfile GetPresetProfile(ExamPreset preset)
        {
            return preset switch
            {
                ExamPreset.SscCglChsl => new ExamProfile
                {
                    Preset = ExamPreset.SscCglChsl,
                    ExamName = "SSC CGL / CHSL DEST",
                    Description = "Official SSC Skill Test mode. 15 minutes test, strictly 35 WPM (Eng) / 30 WPM (Hin). Disabled backspace penalty mode.",
                    TargetWpm = 35,
                    MinAccuracyPercent = 95.0,
                    BackspaceMode = BackspaceRule.Disabled,
                    TimeLimitMinutes = 15,
                    IsHighlightEnabled = false,
                    AutoScroll = true,
                    HighlightMistakes = false,
                    PenaltyCalculationRule = "Keystroke / 5 - Half/Full Error Penalties"
                },
                ExamPreset.UpssscJuniorAssistant => new ExamProfile
                {
                    Preset = ExamPreset.UpssscJuniorAssistant,
                    ExamName = "UPSSSC Junior Assistant",
                    Description = "UPSSSC Hindi (25 WPM Mangal/Kruti) & English (30 WPM) typing exam simulation with 5 min timer.",
                    TargetWpm = 30,
                    MinAccuracyPercent = 85.0,
                    BackspaceMode = BackspaceRule.Allowed,
                    TimeLimitMinutes = 5,
                    IsHighlightEnabled = true,
                    AutoScroll = true,
                    HighlightMistakes = true,
                    PenaltyCalculationRule = "Standard Speed & Accuracy Threshold"
                },
                ExamPreset.HighCourtRoAro => new ExamProfile
                {
                    Preset = ExamPreset.HighCourtRoAro,
                    ExamName = "High Court RO / ARO",
                    Description = "Allahabad / High Court exam pattern with restricted backspace (only inside current word) and 500 word passages.",
                    TargetWpm = 25,
                    MinAccuracyPercent = 90.0,
                    BackspaceMode = BackspaceRule.RestrictedToWord,
                    TimeLimitMinutes = 15,
                    IsHighlightEnabled = true,
                    AutoScroll = true,
                    HighlightMistakes = true,
                    PenaltyCalculationRule = "Strict Omission & Word Penalty"
                },
                ExamPreset.RailwayNtpc => new ExamProfile
                {
                    Preset = ExamPreset.RailwayNtpc,
                    ExamName = "Railway NTPC Typing Test",
                    Description = "RRB NTPC Typing Skill Test format (30 WPM Eng / 25 WPM Hin), 10 minutes duration.",
                    TargetWpm = 30,
                    MinAccuracyPercent = 95.0,
                    BackspaceMode = BackspaceRule.Allowed,
                    TimeLimitMinutes = 10,
                    IsHighlightEnabled = false,
                    AutoScroll = true,
                    HighlightMistakes = false,
                    PenaltyCalculationRule = "RRB 5% Mistake Allowance Formula"
                },
                _ => new ExamProfile
                {
                    Preset = ExamPreset.Custom,
                    ExamName = "Custom Exam Simulation",
                    Description = "User-defined mock exam conditions and rules.",
                    TargetWpm = 35,
                    MinAccuracyPercent = 90.0,
                    BackspaceMode = BackspaceRule.Allowed,
                    TimeLimitMinutes = 10,
                    IsHighlightEnabled = true,
                    AutoScroll = true,
                    HighlightMistakes = true,
                    PenaltyCalculationRule = "Standard Net WPM"
                }
            };
        }
    }
}
