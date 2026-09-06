; Script generated for Inno Setup Compiler
; ExamTyping Tutor - Official Windows Setup Script

#define MyAppName "ExamTyping Tutor"
#define MyAppVersion "2.5.0"
#define MyAppPublisher "ExamTyping Suite"
#define MyAppURL "https://github.com/typing-tutor"
#define MyAppExeName "TypingTutor.exe"

[Setup]
AppId={{D37E84B1-2F10-4A59-86E9-9C5DE3112026}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={autopf}\{#MyAppName}
DisableProgramGroupPage=yes
LicenseFiles=
OutputDir=..\Publish\InnoSetup
OutputBaseFilename=ExamTypingTutor_Setup_v2.5.0
Compression=lzma2/ultra64
SolidCompression=yes
WizardStyle=modern
PrivilegesRequired=lowest

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "..\Publish\SelfContained\{#MyAppExeName}"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{autoprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent
