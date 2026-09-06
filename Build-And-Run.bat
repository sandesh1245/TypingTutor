@echo off
title TypingTutor - Build & Run Script
cls
echo ========================================================
echo   ExamTyping - TypingTutor Desktop Platform
echo   Building and Launching Application...
echo ========================================================
echo.

echo [1/3] Closing any active instances of TypingTutor...
taskkill /F /IM TypingTutor.exe >nul 2>&1

echo [2/3] Building C# WPF Solution (.NET 9)...
dotnet build -c Debug
if errorlevel 1 (
    echo.
    echo [ERROR] Build failed! Please review the error messages above.
    echo.
    pause
    exit /b 1
)

echo.
echo [3/3] Launching TypingTutor Desktop Application...
start "" "bin\Debug\net9.0-windows\TypingTutor.exe"
echo Done! TypingTutor is running.
timeout /t 2 >nul
exit /b 0
