@echo off
title Install Local MSIX - ExamTyping Tutor
echo ==============================================================================
echo        ExamTyping Tutor - Local MSIX Package Sideload Installer
echo ==============================================================================
echo.

powershell -Command "Get-Content -Raw '%~dp0Install-Local-MSIX.ps1' | Invoke-Expression"

if %ERRORLEVEL% neq 0 (
    echo.
    echo [ERROR] Installation script encountered an issue. See details above.
)
echo.
pause
