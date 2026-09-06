using System;
using System.IO;
using System.Windows;

namespace TypingTutor;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        AppDomain.CurrentDomain.UnhandledException += (s, args) =>
        {
            var msg = $"[AppDomain Unhandled] {args.ExceptionObject}\n";
            File.AppendAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "startup_error.log"), msg);
        };

        DispatcherUnhandledException += (s, args) =>
        {
            var msg = $"[Dispatcher Unhandled] {args.Exception}\n";
            File.AppendAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "startup_error.log"), msg);
            MessageBox.Show(args.Exception.Message + "\n" + args.Exception.StackTrace, "TypingTutor Error");
        };

        File.WriteAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "startup.log"), $"App.OnStartup fired at {DateTime.Now}\n");

        if (e.Args != null && Array.IndexOf(e.Args, "--test-auth") >= 0)
        {
            TypingTutor.Scratch.TestAuthAndHistory.RunTests();
            Shutdown(0);
            return;
        }
    }
}


