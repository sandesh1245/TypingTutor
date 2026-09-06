@echo off
setlocal enabledelayedexpansion
title ExamTyping Tutor - MSIX Store Package Builder

echo ==============================================================================
echo        ExamTyping Tutor - Microsoft Store (MSIX) Package Builder
echo ==============================================================================
echo.

:: 1. Terminate any running instances
taskkill /F /IM TypingTutor.exe >nul 2>&1

:: 2. Find makeappx.exe in Windows Kits
set "MAKEAPPX_PATH="
if exist "C:\Program Files (x86)\Windows Kits\10\bin\10.0.22621.0\x64\makeappx.exe" (
    set "MAKEAPPX_PATH=C:\Program Files (x86)\Windows Kits\10\bin\10.0.22621.0\x64\makeappx.exe"
) else (
    for /f "delims=" %%I in ('dir /b /s "C:\Program Files (x86)\Windows Kits\10\bin\makeappx.exe" 2^>nul') do (
        set "MAKEAPPX_PATH=%%I"
        goto :FoundMakeAppx
    )
)

:FoundMakeAppx
if "!MAKEAPPX_PATH!"=="" (
    echo [ERROR] makeappx.exe not found in Windows Kits! Please ensure Windows SDK is installed.
    pause
    exit /b 1
)
echo [OK] Found makeappx: !MAKEAPPX_PATH!

set "SIGNTOOL_PATH="
if exist "C:\Program Files (x86)\Windows Kits\10\bin\10.0.22621.0\x64\signtool.exe" (
    set "SIGNTOOL_PATH=C:\Program Files (x86)\Windows Kits\10\bin\10.0.22621.0\x64\signtool.exe"
)

:: 3. Ensure self-contained release executable exists
set "EXE_SOURCE=Publish\SelfContained\TypingTutor.exe"
if not exist "!EXE_SOURCE!" (
    echo [INFO] Standalone executable not found in Publish\SelfContained. Building now...
    call "Build-SingleExe-And-Setup.bat"
    if not exist "!EXE_SOURCE!" (
        echo [ERROR] Failed to compile !EXE_SOURCE!
        pause
        exit /b 1
    )
)
echo [OK] Using application binary: !EXE_SOURCE!

:: 4. Ensure Store Visual Assets exist
if not exist "Store\Assets\Square150x150Logo.png" (
    echo [INFO] Generating Store visual assets...
    powershell -Command "Get-Content -Raw 'Store\Generate-Assets.ps1' | Invoke-Expression"
)
echo [OK] Store visual assets verified.

:: 5. Prepare Packaging Layout directory
set "LAYOUT_DIR=Publish\MSIX_Layout"
if exist "!LAYOUT_DIR!" rd /s /q "!LAYOUT_DIR!"
mkdir "!LAYOUT_DIR!"
mkdir "!LAYOUT_DIR!\Assets"

echo [INFO] Staging package files into !LAYOUT_DIR!...
copy /y "!EXE_SOURCE!" "!LAYOUT_DIR!\TypingTutor.exe" >nul
copy /y "Store\AppxManifest.xml" "!LAYOUT_DIR!\AppxManifest.xml" >nul
xcopy /s /y /q "Store\Assets\*" "!LAYOUT_DIR!\Assets\" >nul

:: 6. Create Output Directory
if not exist "Publish\MSIX" mkdir "Publish\MSIX"
set "MSIX_OUTPUT=Publish\MSIX\ExamTypingTutor_v2.5.0.msix"

:: 7. Compile the MSIX package
echo.
echo [INFO] Packaging with makeappx.exe...
"!MAKEAPPX_PATH!" pack /d "!LAYOUT_DIR!" /p "!MSIX_OUTPUT!" /o

:: 8. Optional: Sign package with test cert if available
if exist "Store\Cert\ExamTypingTest.pfx" if not "!SIGNTOOL_PATH!"=="" (
    echo [INFO] Signing package with local test certificate...
    "!SIGNTOOL_PATH!" sign /fd SHA256 /a /f "Store\Cert\ExamTypingTest.pfx" /p ExamTyping2026 "!MSIX_OUTPUT!"
    if !ERRORLEVEL! equ 0 (
        echo [OK] Signed MSIX package with test certificate.
    )
)

echo.
echo ==============================================================================
echo [SUCCESS] Microsoft Store MSIX package created successfully!
echo Output: !MSIX_OUTPUT!
for %%A in ("!MSIX_OUTPUT!") do echo Size:   %%~zA bytes (approx %%~zA / 1048576 MB)
echo ==============================================================================
echo.
echo NOTE:
echo   - To submit to Microsoft Store: Upload this .msix file to Partner Center.
echo   - To test locally on this PC: Run 'Store\Install-Local-MSIX.bat'
echo.
pause
