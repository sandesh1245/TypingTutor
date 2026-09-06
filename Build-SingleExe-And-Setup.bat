@echo off
setlocal enabledelayedexpansion

echo ============================================================
echo   ExamTyping Tutor - Build Single EXE and Setup Installer
echo ============================================================
echo.

cd /d "%~dp0"

echo [1/3] Terminating any running instances...
taskkill /F /IM TypingTutor.exe >nul 2>&1
taskkill /F /IM TypingTutor-Setup.exe >nul 2>&1

echo [2/3] Publishing Standalone Single-File Executable (win-x64)...
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:EnableCompressionInSingleFile=true -o Publish\SelfContained
if %errorlevel% neq 0 (
    echo Error: Failed to publish standalone executable.
    pause
    exit /b %errorlevel%
)
echo Standalone Single EXE generated at: Publish\SelfContained\TypingTutor.exe
echo.

echo [3/3] Building and Packaging Setup Installer (TypingTutor-Setup.exe)...
dotnet publish Installer\Setup\Setup.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:EnableCompressionInSingleFile=true -o Publish\Setup
if %errorlevel% neq 0 (
    echo Error: Failed to publish Setup Installer.
    pause
    exit /b %errorlevel%
)
echo Setup Installer generated at: Publish\Setup\TypingTutor-Setup.exe
echo.

echo ============================================================
echo   BUILD COMPLETE!
echo   1. Single Portable EXE:  Publish\SelfContained\TypingTutor.exe
echo   2. Windows Setup Wizard: Publish\Setup\TypingTutor-Setup.exe
echo ============================================================
pause
