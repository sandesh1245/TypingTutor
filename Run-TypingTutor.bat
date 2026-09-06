@echo off
title TypingTutor (ExamTyping) - Government Exam Simulator
cls
echo ========================================================
echo   ExamTyping - TypingTutor Desktop Platform
echo   Offline Simulation Suite for Govt Typing Exams
echo ========================================================
echo.

taskkill /F /IM TypingTutor.exe >nul 2>&1
echo Building C# WPF Application...
dotnet build -c Debug
if errorlevel 1 (
    echo [ERROR] Build failed! Please check your .NET 9 SDK installation.
    pause
    exit /b 1
)

echo Launching TypingTutor Desktop Application...
start "" "bin\Debug\net9.0-windows\TypingTutor.exe"
echo Done! App is running.
timeout /t 2 >nul
exit /b 0
