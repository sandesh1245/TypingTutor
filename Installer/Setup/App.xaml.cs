using System;
using System.IO;
using System.Linq;
using System.Windows;

namespace TypingTutorSetup
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var silentFlags = new[] { "/s", "-s", "/silent", "--silent", "/verysilent", "--verysilent", "/quiet", "--quiet", "/qn" };
            bool isSilent = e.Args.Any(arg => silentFlags.Contains(arg.ToLowerInvariant()));

            if (isSilent)
            {
                try
                {
                    string defaultPath = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        "Programs",
                        "ExamTypingTutor");

                    Directory.CreateDirectory(defaultPath);
                    string targetExe = Path.Combine(defaultPath, "TypingTutor.exe");
                    TypingTutorSetup.MainWindow.ExtractEmbeddedPayload(targetExe);

                    string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                    string deskLnk = Path.Combine(desktopPath, "ExamTyping Tutor.lnk");
                    TypingTutorSetup.MainWindow.CreateShortcut(deskLnk, targetExe, "ExamTyping Tutor - Typing Tutor & Exam Suite");

                    string progPath = Environment.GetFolderPath(Environment.SpecialFolder.Programs);
                    string progLnk = Path.Combine(progPath, "ExamTyping Tutor.lnk");
                    TypingTutorSetup.MainWindow.CreateShortcut(progLnk, targetExe, "ExamTyping Tutor - Typing Tutor & Exam Suite");

                    TypingTutorSetup.MainWindow.RegisterUninstaller(defaultPath, targetExe);

                    Shutdown(0);
                    return;
                }
                catch (Exception)
                {
                    Shutdown(1);
                    return;
                }
            }

            new MainWindow().Show();
        }
    }
}
