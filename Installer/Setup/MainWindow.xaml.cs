using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;

namespace TypingTutorSetup
{
    public partial class MainWindow : Window
    {
        private string _targetExePath = string.Empty;

        public MainWindow()
        {
            InitializeComponent();

            string defaultPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "Programs",
                "ExamTypingTutor");

            TxtInstallPath.Text = defaultPath;
        }

        private void BtnBrowse_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFolderDialog
            {
                Title = "Select Installation Directory",
                InitialDirectory = TxtInstallPath.Text
            };

            if (dialog.ShowDialog() == true)
            {
                TxtInstallPath.Text = dialog.FolderName;
            }
        }

        private async void BtnInstall_Click(object sender, RoutedEventArgs e)
        {
            string installDir = TxtInstallPath.Text.Trim();
            if (string.IsNullOrWhiteSpace(installDir))
            {
                MessageBox.Show("Please specify a valid installation directory.", "Setup Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            bool makeDesktop = ChkDesktopShortcut.IsChecked == true;
            bool makeStartMenu = ChkStartMenuShortcut.IsChecked == true;

            BtnInstall.IsEnabled = false;
            BtnCancel.IsEnabled = false;
            PanelProgress.Visibility = Visibility.Visible;

            try
            {
                await Task.Run(() =>
                {
                    UpdateStatus("Creating destination directory...");
                    Directory.CreateDirectory(installDir);

                    _targetExePath = Path.Combine(installDir, "TypingTutor.exe");

                    UpdateStatus("Extracting application payload...");
                    ExtractEmbeddedPayload(_targetExePath);

                    if (makeDesktop)
                    {
                        UpdateStatus("Creating Desktop shortcut...");
                        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                        string lnkPath = Path.Combine(desktopPath, "ExamTyping Tutor.lnk");
                        CreateShortcut(lnkPath, _targetExePath, "ExamTyping Tutor - Typing Tutor & Exam Suite");
                    }

                    if (makeStartMenu)
                    {
                        UpdateStatus("Creating Start Menu shortcut & registering...");
                        string startMenuPath = Environment.GetFolderPath(Environment.SpecialFolder.Programs);
                        string lnkPath = Path.Combine(startMenuPath, "ExamTyping Tutor.lnk");
                        CreateShortcut(lnkPath, _targetExePath, "ExamTyping Tutor - Typing Tutor & Exam Suite");

                        RegisterUninstaller(installDir, _targetExePath);
                    }
                });

                PanelSetupConfig.Visibility = Visibility.Collapsed;
                PanelCompletion.Visibility = Visibility.Visible;
                BtnInstall.Visibility = Visibility.Collapsed;
                BtnCancel.Visibility = Visibility.Collapsed;
                BtnFinish.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Installation failed: {ex.Message}", "Setup Error", MessageBoxButton.OK, MessageBoxImage.Error);
                BtnInstall.IsEnabled = true;
                BtnCancel.IsEnabled = true;
                PanelProgress.Visibility = Visibility.Collapsed;
            }
        }

        public static void ExtractEmbeddedPayload(string targetPath)
        {
            var assembly = Assembly.GetExecutingAssembly();
            string? resourceName = assembly.GetManifestResourceNames()
                .FirstOrDefault(n => n.EndsWith("TypingTutor.exe", StringComparison.OrdinalIgnoreCase));

            if (string.IsNullOrEmpty(resourceName))
            {
                // Fallback if built side-by-side
                string localExe = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TypingTutor.exe");
                if (File.Exists(localExe))
                {
                    File.Copy(localExe, targetPath, true);
                    return;
                }
                throw new FileNotFoundException("Embedded application payload was not found in the installer.");
            }

            using var resStream = assembly.GetManifestResourceStream(resourceName);
            if (resStream == null)
            {
                throw new FileNotFoundException("Failed to open embedded payload stream.");
            }

            using var fileStream = new FileStream(targetPath, FileMode.Create, FileAccess.Write);
            resStream.CopyTo(fileStream);
        }

        public static void CreateShortcut(string shortcutPath, string targetPath, string description)
        {
            try
            {
                Type? shellType = Type.GetTypeFromProgID("WScript.Shell");
                if (shellType != null)
                {
                    dynamic? shell = Activator.CreateInstance(shellType);
                    if (shell != null)
                    {
                        dynamic shortcut = shell.CreateShortcut(shortcutPath);
                        shortcut.TargetPath = targetPath;
                        shortcut.WorkingDirectory = Path.GetDirectoryName(targetPath);
                        shortcut.Description = description;
                        shortcut.Save();
                    }
                }
            }
            catch { }
        }

        public static void RegisterUninstaller(string installDir, string targetExePath)
        {
            try
            {
                // Create uninstaller batch script
                string uninstallerBat = Path.Combine(installDir, "uninstall.cmd");
                File.WriteAllText(uninstallerBat, 
                    $"@echo off\r\n" +
                    $"taskkill /F /IM TypingTutor.exe >nul 2>&1\r\n" +
                    $"reg delete \"HKCU\\Software\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\ExamTypingTutor\" /f >nul 2>&1\r\n" +
                    $"del /q \"{Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "ExamTyping Tutor.lnk")}\" >nul 2>&1\r\n" +
                    $"del /q \"{Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Programs), "ExamTyping Tutor.lnk")}\" >nul 2>&1\r\n" +
                    $"echo ExamTyping Tutor uninstalled successfully.\r\n" +
                    $"timeout /t 2 >nul\r\n" +
                    $"(goto) 2>nul & rmdir /s /q \"{installDir}\"\r\n");

                using var key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Uninstall\ExamTypingTutor");
                if (key != null)
                {
                    key.SetValue("DisplayName", "ExamTyping - Professional Typing Tutor & Examination Suite");
                    key.SetValue("DisplayVersion", "2.5.0");
                    key.SetValue("Publisher", "ExamTyping Suite");
                    key.SetValue("InstallLocation", installDir);
                    key.SetValue("DisplayIcon", targetExePath);
                    key.SetValue("UninstallString", $"cmd.exe /c \"{uninstallerBat}\"");
                    key.SetValue("NoModify", 1, RegistryValueKind.DWord);
                    key.SetValue("NoRepair", 1, RegistryValueKind.DWord);
                }
            }
            catch { }
        }

        private void UpdateStatus(string message)
        {
            Dispatcher.Invoke(() =>
            {
                TxtStatus.Text = message;
            });
        }

        private void BtnFinish_Click(object sender, RoutedEventArgs e)
        {
            if (ChkLaunchAfterInstall.IsChecked == true && !string.IsNullOrEmpty(_targetExePath) && File.Exists(_targetExePath))
            {
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = _targetExePath,
                        WorkingDirectory = Path.GetDirectoryName(_targetExePath),
                        UseShellExecute = true
                    });
                }
                catch { }
            }

            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
