@echo off
setlocal enabledelayedexpansion
title ExamTyping Tutor - Master Publish Pipeline

echo ==============================================================================
echo        ExamTyping Tutor - Complete Multi-Channel Publishing Pipeline
echo ==============================================================================
echo.

cd /d "%~dp0"

:: 1. Terminate running instances
echo [1/4] Closing any active instances...
taskkill /F /IM TypingTutor.exe >nul 2>&1
taskkill /F /IM TypingTutor-Setup.exe >nul 2>&1

:: 2. Build Single Portable EXE
echo.
echo [2/4] Publishing Standalone Single EXE (win-x64)...
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:EnableCompressionInSingleFile=true -o Publish\SelfContained TypingTutor.csproj
if !ERRORLEVEL! neq 0 (
    echo [ERROR] Failed to compile Standalone Single EXE!
    pause
    exit /b !ERRORLEVEL!
)
echo [OK] Single EXE generated: Publish\SelfContained\TypingTutor.exe

:: 3. Build Windows Setup Wizard
echo.
echo [3/4] Publishing Windows Setup Installer...
dotnet publish Installer\Setup\Setup.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:EnableCompressionInSingleFile=true -o Publish\Setup
if !ERRORLEVEL! neq 0 (
    echo [ERROR] Failed to compile Setup Installer!
    pause
    exit /b !ERRORLEVEL!
)
echo [OK] Setup Installer generated: Publish\Setup\TypingTutor-Setup.exe

:: 4. Build Microsoft Store MSIX Package
echo.
echo [4/4] Packaging Microsoft Store MSIX package...
call "Build-MSIX-Package.bat"
if !ERRORLEVEL! neq 0 (
    echo [ERROR] Failed to compile MSIX package!
    pause
    exit /b !ERRORLEVEL!
)

echo.
echo ==============================================================================
echo   ALL CHANNELS PUBLISHED SUCCESSFULLY!
echo.
echo   1. Portable Standalone EXE : Publish\SelfContained\TypingTutor.exe
echo   2. Windows Setup Installer : Publish\Setup\TypingTutor-Setup.exe
echo   3. Microsoft Store Package : Publish\MSIX\ExamTypingTutor_v2.5.0.msix
echo ==============================================================================
echo.
pause
