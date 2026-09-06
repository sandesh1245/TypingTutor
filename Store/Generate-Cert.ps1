$ErrorActionPreference = "Stop"
$certDir = "e:\PlayGround\TypingTutor\Store\Cert"
$pfxPath = Join-Path $certDir "ExamTypingTest.pfx"
$cerPath = Join-Path $certDir "ExamTypingTest.cer"
$certSubject = "CN=3DD1BBA5-3865-4441-ADF3-EA240BCCC42C"
$pfxPassword = "ExamTyping2026"

if (Test-Path $pfxPath) { Remove-Item $pfxPath -Force }
if (Test-Path $cerPath) { Remove-Item $cerPath -Force }

$rsa = [System.Security.Cryptography.RSA]::Create(2048)
$dn = New-Object System.Security.Cryptography.X509Certificates.X500DistinguishedName($certSubject)
$req = New-Object System.Security.Cryptography.X509Certificates.CertificateRequest(
    $dn,
    $rsa,
    [System.Security.Cryptography.HashAlgorithmName]::SHA256,
    [System.Security.Cryptography.RSASignaturePadding]::Pkcs1
)

$basicConstraints = New-Object System.Security.Cryptography.X509Certificates.X509BasicConstraintsExtension($false, $false, 0, $true)
$req.CertificateExtensions.Add($basicConstraints)

$keyUsage = New-Object System.Security.Cryptography.X509Certificates.X509KeyUsageExtension(
    [System.Security.Cryptography.X509Certificates.X509KeyUsageFlags]::DigitalSignature,
    $true
)
$req.CertificateExtensions.Add($keyUsage)

$oidCollection = New-Object System.Security.Cryptography.OidCollection
$codeSigningOid = New-Object System.Security.Cryptography.Oid("1.3.6.1.5.5.7.3.3")
$oidCollection.Add($codeSigningOid) | Out-Null
$enhancedKeyUsage = New-Object System.Security.Cryptography.X509Certificates.X509EnhancedKeyUsageExtension($oidCollection, $false)
$req.CertificateExtensions.Add($enhancedKeyUsage)

$notBefore = [DateTimeOffset]::UtcNow.AddDays(-1)
$notAfter = [DateTimeOffset]::UtcNow.AddYears(5)
$cert = $req.CreateSelfSigned($notBefore, $notAfter)

$pfxBytes = $cert.Export([System.Security.Cryptography.X509Certificates.X509ContentType]::Pfx, $pfxPassword)
[System.IO.File]::WriteAllBytes($pfxPath, $pfxBytes)

$cerBytes = $cert.Export([System.Security.Cryptography.X509Certificates.X509ContentType]::Cert)
[System.IO.File]::WriteAllBytes($cerPath, $cerBytes)

Write-Host "SUCCESS: Certificate generated for $certSubject" -ForegroundColor Green
