# Store\Install-Local-MSIX.ps1
# Generates a local test certificate matching the manifest publisher, signs the MSIX, and installs it locally.

$ErrorActionPreference = "Stop"
$ProgressPreference = "SilentlyContinue"

$scriptDir = if ($PSScriptRoot) { $PSScriptRoot } else { "e:\PlayGround\TypingTutor\Store" }
$projectRoot = (Get-Item $scriptDir).Parent.FullName
$msixPath = Join-Path $projectRoot "Publish\MSIX\ExamTypingTutor_v2.5.0.msix"
$certDir = Join-Path $scriptDir "Cert"

if (-not (Test-Path $msixPath)) {
    Write-Host "[ERROR] MSIX package not found at: $msixPath" -ForegroundColor Red
    Write-Host "Please run 'Build-MSIX-Package.bat' first." -ForegroundColor Yellow
    exit 1
}

if (-not (Test-Path $certDir)) {
    New-Item -ItemType Directory -Path $certDir -Force | Out-Null
}

$pfxPath = Join-Path $certDir "ExamTypingTest.pfx"
$cerPath = Join-Path $certDir "ExamTypingTest.cer"
$certSubject = "CN=3DD1BBA5-3865-4441-ADF3-EA240BCCC42C"
$pfxPassword = "ExamTyping2026"

Write-Host "==============================================================================" -ForegroundColor Cyan
Write-Host "          ExamTyping Tutor - Local MSIX Sideload Installer" -ForegroundColor Cyan
Write-Host "==============================================================================" -ForegroundColor Cyan
Write-Host ""

# 1. Check or Create Test Signing Certificate (Pure Managed .NET)
Write-Host "[1/4] Checking Local Test Certificate ($certSubject)..." -ForegroundColor Yellow

if (-not (Test-Path $pfxPath) -or -not (Test-Path $cerPath)) {
    Write-Host "      Generating new self-signed certificate for local testing using .NET Crypto..." -ForegroundColor Gray
    
    $rsa = [System.Security.Cryptography.RSA]::Create(2048)
    $dn = New-Object System.Security.Cryptography.X509Certificates.X500DistinguishedName($certSubject)
    $req = New-Object System.Security.Cryptography.X509Certificates.CertificateRequest(
        $dn,
        $rsa,
        [System.Security.Cryptography.HashAlgorithmName]::SHA256,
        [System.Security.Cryptography.RSASignaturePadding]::Pkcs1
    )

    # Basic Constraints (End Entity)
    $basicConstraints = New-Object System.Security.Cryptography.X509Certificates.X509BasicConstraintsExtension($false, $false, 0, $true)
    $req.CertificateExtensions.Add($basicConstraints)

    # Key Usage (Digital Signature)
    $keyUsage = New-Object System.Security.Cryptography.X509Certificates.X509KeyUsageExtension(
        [System.Security.Cryptography.X509Certificates.X509KeyUsageFlags]::DigitalSignature,
        $true
    )
    $req.CertificateExtensions.Add($keyUsage)

    # Enhanced Key Usage (Code Signing - 1.3.6.1.5.5.7.3.3)
    $oidCollection = New-Object System.Security.Cryptography.OidCollection
    $codeSigningOid = New-Object System.Security.Cryptography.Oid("1.3.6.1.5.5.7.3.3")
    $oidCollection.Add($codeSigningOid) | Out-Null
    $enhancedKeyUsage = New-Object System.Security.Cryptography.X509Certificates.X509EnhancedKeyUsageExtension($oidCollection, $false)
    $req.CertificateExtensions.Add($enhancedKeyUsage)

    $notBefore = [DateTimeOffset]::UtcNow.AddDays(-1)
    $notAfter = [DateTimeOffset]::UtcNow.AddYears(5)
    $cert = $req.CreateSelfSigned($notBefore, $notAfter)

    # Export PFX with private key
    $pfxBytes = $cert.Export([System.Security.Cryptography.X509Certificates.X509ContentType]::Pfx, $pfxPassword)
    [System.IO.File]::WriteAllBytes($pfxPath, $pfxBytes)

    # Export CER without private key
    $cerBytes = $cert.Export([System.Security.Cryptography.X509Certificates.X509ContentType]::Cert)
    [System.IO.File]::WriteAllBytes($cerPath, $cerBytes)

    Write-Host "      Created certificate: $cerPath" -ForegroundColor Gray
} else {
    Write-Host "      Using existing certificate from: $pfxPath" -ForegroundColor Gray
}

# 2. Locate SignTool.exe and Sign the MSIX
Write-Host "[2/4] Signing MSIX package..." -ForegroundColor Yellow
$signtool = "C:\Program Files (x86)\Windows Kits\10\bin\10.0.22621.0\x64\signtool.exe"
if (-not (Test-Path $signtool)) {
    $signtool = (Get-ChildItem -Path "C:\Program Files (x86)\Windows Kits\10\bin" -Filter "signtool.exe" -Recurse -ErrorAction SilentlyContinue | Select-Object -First 1).FullName
}

if (-not $signtool) {
    Write-Host "[ERROR] signtool.exe not found in Windows Kits!" -ForegroundColor Red
    exit 1
}

& "$signtool" sign /fd SHA256 /a /f "$pfxPath" /p "$pfxPassword" "$msixPath"
if ($LASTEXITCODE -ne 0) {
    Write-Host "[ERROR] Failed to sign MSIX package." -ForegroundColor Red
    exit $LASTEXITCODE
}
Write-Host "      Package signed successfully." -ForegroundColor Green

# 3. Trust Certificate in CurrentUser\TrustedPeople (Standard location for Windows MSIX sideload testing)
Write-Host "[3/4] Ensuring certificate is trusted in CurrentUser\TrustedPeople..." -ForegroundColor Yellow
try {
    $testCert = New-Object System.Security.Cryptography.X509Certificates.X509Certificate2($cerPath)
    $peopleStore = New-Object System.Security.Cryptography.X509Certificates.X509Store(
        [System.Security.Cryptography.X509Certificates.StoreName]::TrustedPeople,
        [System.Security.Cryptography.X509Certificates.StoreLocation]::CurrentUser
    )
    $peopleStore.Open([System.Security.Cryptography.X509Certificates.OpenFlags]::ReadWrite)
    $foundPeople = $peopleStore.Certificates.Find([System.Security.Cryptography.X509Certificates.X509FindType]::FindByThumbprint, $testCert.Thumbprint, $false)
    if ($foundPeople.Count -eq 0) {
        $peopleStore.Add($testCert)
        Write-Host "      Added test certificate to CurrentUser\TrustedPeople." -ForegroundColor Gray
    } else {
        Write-Host "      Certificate is already present in CurrentUser\TrustedPeople." -ForegroundColor Gray
    }
    $peopleStore.Close()
} catch {
    Write-Host "      [NOTE] Could not auto-import to CurrentUser\TrustedPeople in current session: $($_.Exception.Message)" -ForegroundColor DarkYellow
    Write-Host "      To trust manually for sideloading, double-click '$cerPath' and install to 'Trusted People'." -ForegroundColor DarkYellow
}

# 4. Install / Update the Appx Package
Write-Host "[4/4] Installing / Registering MSIX package on Windows..." -ForegroundColor Yellow
Add-AppxPackage -Path "$msixPath" -ForceApplicationShutdown

Write-Host ""
Write-Host "==============================================================================" -ForegroundColor Green
Write-Host "[SUCCESS] ExamTyping Tutor MSIX package installed successfully!" -ForegroundColor Green
Write-Host "You can now launch 'ExamTyping Tutor' directly from your Windows Start Menu!" -ForegroundColor Cyan
Write-Host "==============================================================================" -ForegroundColor Green
