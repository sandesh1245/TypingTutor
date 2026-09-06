# Store\Generate-App-Icon.ps1
# Generates AppIcon.ico, AppIcon_256x256.png, AppIcon_300x300.png, and AppIcon_512x512.png

Add-Type -AssemblyName PresentationCore, PresentationFramework, WindowsBase, System.Drawing

$storeDir = if ($PSScriptRoot) { $PSScriptRoot } elseif ($MyInvocation.MyCommand.Path) { Split-Path -Parent $MyInvocation.MyCommand.Path } else { "e:\PlayGround\TypingTutor\Store" }
$assetsDir = Join-Path $storeDir "Assets"

if (-not (Test-Path $assetsDir)) {
    New-Item -ItemType Directory -Path $assetsDir -Force | Out-Null
}

function Render-AppIconBitmap {
    param([int]$size)

    $visual = New-Object System.Windows.Media.DrawingVisual
    $dc = $visual.RenderOpen()

    # Background Gradient: Sleek Vibrant Indigo to Sky Blue
    $grad = New-Object System.Windows.Media.LinearGradientBrush(
        [System.Windows.Media.Color]::FromRgb(2, 132, 199),   # Sky blue
        [System.Windows.Media.Color]::FromRgb(99, 102, 241),  # Indigo
        (New-Object System.Windows.Point(0, 0)),
        (New-Object System.Windows.Point(1, 1))
    )

    $cornerRad = [int]($size * 0.22)
    $dc.DrawRoundedRectangle($grad, $null, (New-Object System.Windows.Rect(0, 0, $size, $size)), $cornerRad, $cornerRad)

    # Monitor / Screen Outline inside Icon
    $whiteBrush = New-Object System.Windows.Media.SolidColorBrush([System.Windows.Media.Color]::FromRgb(255, 255, 255))
    $penThick = [Math]::Max(2.0, [double]($size * 0.035))
    $whitePen = New-Object System.Windows.Media.Pen($whiteBrush, $penThick)
    $whitePen.StartLineCap = [System.Windows.Media.PenLineCap]::Round
    $whitePen.EndLineCap = [System.Windows.Media.PenLineCap]::Round

    $monW = [int]($size * 0.55)
    $monH = [int]($size * 0.36)
    $monX = [int](($size - $monW) / 2)
    $monY = [int]($size * 0.18)
    $monRad = [int]($size * 0.04)

    $dc.DrawRoundedRectangle($null, $whitePen, (New-Object System.Windows.Rect($monX, $monY, $monW, $monH)), $monRad, $monRad)

    # Stand
    $midX = [int]($size / 2)
    $standTop = $monY + $monH
    $standBottom = $standTop + [int]($size * 0.10)
    $dc.DrawLine($whitePen, (New-Object System.Windows.Point($midX, $standTop)), (New-Object System.Windows.Point($midX, $standBottom)))

    $baseW = [int]($size * 0.28)
    $baseX1 = [int]($midX - ($baseW / 2))
    $baseX2 = [int]($midX + ($baseW / 2))
    $dc.DrawLine($whitePen, (New-Object System.Windows.Point($baseX1, $standBottom)), (New-Object System.Windows.Point($baseX2, $standBottom)))

    # Keyboard base bar
    $kbY = [int]($size * 0.73)
    $kbW = [int]($size * 0.65)
    $kbX1 = [int](($size - $kbW) / 2)
    $kbX2 = $kbX1 + $kbW
    $dc.DrawLine($whitePen, (New-Object System.Windows.Point($kbX1, $kbY)), (New-Object System.Windows.Point($kbX2, $kbY)))

    # Tactile key dots / bars
    $keyCount = 5
    $keyW = [int]($kbW / ($keyCount + 1))
    $keyH = [Math]::Max(2, [int]($size * 0.045))
    $keyY = $kbY - [int]($size * 0.075)
    $keyBrush = New-Object System.Windows.Media.SolidColorBrush([System.Windows.Media.Color]::FromArgb(210, 255, 255, 255))

    for ($i = 0; $i -lt $keyCount; $i++) {
        $kx = $kbX1 + [int](($i + 0.6) * $keyW)
        $dc.DrawRoundedRectangle($keyBrush, $null, (New-Object System.Windows.Rect($kx, $keyY, [int]($keyW * 0.75), $keyH)), 3, 3)
    }

    $dc.Close()

    $dpi = 96
    $rtb = New-Object System.Windows.Media.Imaging.RenderTargetBitmap($size, $size, $dpi, $dpi, [System.Windows.Media.PixelFormats]::Pbgra32)
    $rtb.Render($visual)
    return $rtb
}

function Save-RtbToPng {
    param(
        [System.Windows.Media.Imaging.RenderTargetBitmap]$rtb,
        [string]$filePath
    )

    $encoder = New-Object System.Windows.Media.Imaging.PngBitmapEncoder
    $frame = [System.Windows.Media.Imaging.BitmapFrame]::Create($rtb)
    $encoder.Frames.Add($frame)

    $fs = New-Object System.IO.FileStream($filePath, [System.IO.FileMode]::Create)
    $encoder.Save($fs)
    $fs.Close()
    Write-Host "[OK] Saved: $filePath"
}

# Generate PNGs
$rtb256 = Render-AppIconBitmap -size 256
Save-RtbToPng -rtb $rtb256 -filePath (Join-Path $assetsDir "AppIcon_256x256.png")

$rtb300 = Render-AppIconBitmap -size 300
Save-RtbToPng -rtb $rtb300 -filePath (Join-Path $assetsDir "AppIcon_300x300.png")

$rtb512 = Render-AppIconBitmap -size 512
Save-RtbToPng -rtb $rtb512 -filePath (Join-Path $assetsDir "AppIcon_512x512.png")

# Also save multi-resolution .ico
$icoPath = Join-Path $assetsDir "app.ico"
$sizes = @(16, 32, 48, 64, 128, 256)
$bitmaps = @()

foreach ($s in $sizes) {
    $rtb = Render-AppIconBitmap -size $s
    $encoder = New-Object System.Windows.Media.Imaging.PngBitmapEncoder
    $encoder.Frames.Add([System.Windows.Media.Imaging.BitmapFrame]::Create($rtb))
    $ms = New-Object System.IO.MemoryStream
    $encoder.Save($ms)
    $ms.Position = 0
    $bmp = [System.Drawing.Bitmap]::FromStream($ms)
    $bitmaps += $bmp
}

# Build ICO binary format with multiple images
$icoStream = New-Object System.IO.FileStream($icoPath, [System.IO.FileMode]::Create)
$bw = New-Object System.IO.BinaryWriter($icoStream)

# ICO Header
$bw.Write([uint16]0) # Reserved
$bw.Write([uint16]1) # Type 1 = ICO
$bw.Write([uint16]$bitmaps.Count) # Count

$offset = 6 + ($bitmaps.Count * 16)
$pngBytesList = @()

for ($i = 0; $i -lt $bitmaps.Count; $i++) {
    $b = $bitmaps[$i]
    $ms = New-Object System.IO.MemoryStream
    $b.Save($ms, [System.Drawing.Imaging.ImageFormat]::Png)
    $bytes = $ms.ToArray()
    $pngBytesList += ,$bytes

    $wByte = if ($b.Width -ge 256) { [byte]0 } else { [byte]$b.Width }
    $hByte = if ($b.Height -ge 256) { [byte]0 } else { [byte]$b.Height }

    $bw.Write($wByte)
    $bw.Write($hByte)
    $bw.Write([byte]0) # Color palette count
    $bw.Write([byte]0) # Reserved
    $bw.Write([uint16]1) # Color planes
    $bw.Write([uint16]32) # Bits per pixel
    $bw.Write([uint32]$bytes.Length) # Image size in bytes
    $bw.Write([uint32]$offset) # File offset

    $offset += $bytes.Length
}

# Write image bytes
foreach ($bytes in $pngBytesList) {
    $bw.Write($bytes)
}

$bw.Close()
$icoStream.Close()
Write-Host "[OK] Multi-resolution Windows ICO generated at: $icoPath"
