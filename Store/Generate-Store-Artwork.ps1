# Store\Generate-Store-Artwork.ps1
# Generates official Microsoft Store 1:1 Box Art (1080x1080) and 2:3 Poster Art (720x1080)

Add-Type -AssemblyName PresentationCore, PresentationFramework, WindowsBase

$storeDir = if ($PSScriptRoot) { $PSScriptRoot } elseif ($MyInvocation.MyCommand.Path) { Split-Path -Parent $MyInvocation.MyCommand.Path } else { "e:\PlayGround\TypingTutor\Store" }
$assetsDir = Join-Path $storeDir "Assets"

if (-not (Test-Path $assetsDir)) {
    New-Item -ItemType Directory -Path $assetsDir -Force | Out-Null
}

function Save-VisualToPng {
    param(
        [System.Windows.Media.DrawingVisual]$visual,
        [int]$width,
        [int]$height,
        [string]$outputPath
    )

    $dpi = 96
    $rtb = New-Object System.Windows.Media.Imaging.RenderTargetBitmap($width, $height, $dpi, $dpi, [System.Windows.Media.PixelFormats]::Pbgra32)
    $rtb.Render($visual)

    $encoder = New-Object System.Windows.Media.Imaging.PngBitmapEncoder
    $frame = [System.Windows.Media.Imaging.BitmapFrame]::Create($rtb)
    $encoder.Frames.Add($frame)

    $fs = New-Object System.IO.FileStream($outputPath, [System.IO.FileMode]::Create)
    $encoder.Save($fs)
    $fs.Close()
    Write-Host "[OK] Generated: $outputPath ($width x $height)"
}

# -------------------------------------------------------------
# 1. 1:1 Box Art (1080 x 1080 px) - Required Main Store Logo
# -------------------------------------------------------------
function Generate-BoxArt1080 {
    param([string]$outPath)

    $width = 1080
    $height = 1080
    $visual = New-Object System.Windows.Media.DrawingVisual
    $dc = $visual.RenderOpen()

    # Background: Clean Modern Light Slate Canvas
    $bgBrush = New-Object System.Windows.Media.LinearGradientBrush(
        [System.Windows.Media.Color]::FromRgb(248, 250, 252),
        [System.Windows.Media.Color]::FromRgb(241, 245, 249),
        (New-Object System.Windows.Point(0, 0)),
        (New-Object System.Windows.Point(0, 1))
    )
    $dc.DrawRectangle($bgBrush, $null, (New-Object System.Windows.Rect(0, 0, $width, $height)))

    # Outer Frame Border
    $framePen = New-Object System.Windows.Media.Pen(
        (New-Object System.Windows.Media.SolidColorBrush([System.Windows.Media.Color]::FromRgb(226, 232, 240))), 3.0
    )
    $dc.DrawRoundedRectangle($null, $framePen, (New-Object System.Windows.Rect(20, 20, 1040, 1040)), 40, 40)

    # Central Hero Badge (Rounded Square with Royal Blue / Indigo Gradient)
    $badgeSize = 460
    $badgeX = [int](($width - $badgeSize) / 2)
    $badgeY = 160

    $badgeGrad = New-Object System.Windows.Media.LinearGradientBrush(
        [System.Windows.Media.Color]::FromRgb(2, 132, 199),
        [System.Windows.Media.Color]::FromRgb(99, 102, 241),
        (New-Object System.Windows.Point(0, 0)),
        (New-Object System.Windows.Point(1, 1))
    )
    $badgeRect = New-Object System.Windows.Rect($badgeX, $badgeY, $badgeSize, $badgeSize)
    $dc.DrawRoundedRectangle($badgeGrad, $null, $badgeRect, 90, 90)

    # Monitor / Screen Outline inside Badge
    $whiteBrush = New-Object System.Windows.Media.SolidColorBrush([System.Windows.Media.Color]::FromRgb(255, 255, 255))
    $whitePen = New-Object System.Windows.Media.Pen($whiteBrush, 14.0)
    $whitePen.StartLineCap = [System.Windows.Media.PenLineCap]::Round
    $whitePen.EndLineCap = [System.Windows.Media.PenLineCap]::Round

    $monX = $badgeX + 110
    $monY = $badgeY + 95
    $dc.DrawRoundedRectangle($null, $whitePen, (New-Object System.Windows.Rect($monX, $monY, 240, 160)), 20, 20)

    # Stand
    $p1 = New-Object System.Windows.Point(($badgeX + 230), ($monY + 160))
    $p2 = New-Object System.Windows.Point(($badgeX + 230), ($monY + 210))
    $dc.DrawLine($whitePen, $p1, $p2)

    $p3 = New-Object System.Windows.Point(($badgeX + 170), ($monY + 210))
    $p4 = New-Object System.Windows.Point(($badgeX + 290), ($monY + 210))
    $dc.DrawLine($whitePen, $p3, $p4)

    # Keyboard base line
    $kbY = $badgeY + 340
    $kp1 = New-Object System.Windows.Point(($badgeX + 90), $kbY)
    $kp2 = New-Object System.Windows.Point(($badgeX + 370), $kbY)
    $dc.DrawLine($whitePen, $kp1, $kp2)
    
    # Tactile key dots
    $keyBrush = New-Object System.Windows.Media.SolidColorBrush([System.Windows.Media.Color]::FromArgb(200, 255, 255, 255))
    for ($k = 0; $k -lt 5; $k++) {
        $kx = $badgeX + 130 + ($k * 50)
        $ky = $kbY - 35
        $dc.DrawRoundedRectangle($keyBrush, $null, (New-Object System.Windows.Rect($kx, $ky, 36, 22)), 6, 6)
    }

    # Title: ExamTyping
    $culture = [System.Globalization.CultureInfo]::InvariantCulture
    $titleTypeface = New-Object System.Windows.Media.Typeface(
        (New-Object System.Windows.Media.FontFamily("Segoe UI, Arial")),
        [System.Windows.FontStyles]::Normal,
        [System.Windows.FontWeights]::Black,
        [System.Windows.FontStretches]::Normal
    )
    $darkCharcoal = New-Object System.Windows.Media.SolidColorBrush([System.Windows.Media.Color]::FromRgb(15, 23, 42))

    $titleText = New-Object System.Windows.Media.FormattedText(
        "ExamTyping",
        $culture,
        [System.Windows.FlowDirection]::LeftToRight,
        $titleTypeface,
        96,
        $darkCharcoal,
        1.0
    )
    $titleX = ($width - $titleText.Width) / 2
    $dc.DrawText($titleText, (New-Object System.Windows.Point($titleX, 660)))

    # Subtitle: TUTOR - EXAM SIMULATOR
    $subTypeface = New-Object System.Windows.Media.Typeface(
        (New-Object System.Windows.Media.FontFamily("Segoe UI, Arial")),
        [System.Windows.FontStyles]::Normal,
        [System.Windows.FontWeights]::Bold,
        [System.Windows.FontStretches]::Normal
    )
    $blueAccent = New-Object System.Windows.Media.SolidColorBrush([System.Windows.Media.Color]::FromRgb(2, 132, 199))
    $subText = New-Object System.Windows.Media.FormattedText(
        "TUTOR  •  EXAM SIMULATOR",
        $culture,
        [System.Windows.FlowDirection]::LeftToRight,
        $subTypeface,
        34,
        $blueAccent,
        1.0
    )
    $subX = ($width - $subText.Width) / 2
    $dc.DrawText($subText, (New-Object System.Windows.Point($subX, 780)))

    # Tag: SSC • Railway • High Court • English and 10 Indian Languages
    $tagTypeface = New-Object System.Windows.Media.Typeface(
        (New-Object System.Windows.Media.FontFamily("Segoe UI, Arial")),
        [System.Windows.FontStyles]::Normal,
        [System.Windows.FontWeights]::SemiBold,
        [System.Windows.FontStretches]::Normal
    )
    $slateMuted = New-Object System.Windows.Media.SolidColorBrush([System.Windows.Media.Color]::FromRgb(100, 116, 139))
    $tagText = New-Object System.Windows.Media.FormattedText(
        "SSC • Railway • High Court • English and 10 Indian Languages",
        $culture,
        [System.Windows.FlowDirection]::LeftToRight,
        $tagTypeface,
        24,
        $slateMuted,
        1.0
    )
    $tagX = ($width - $tagText.Width) / 2
    $dc.DrawText($tagText, (New-Object System.Windows.Point($tagX, 850)))

    $dc.Close()
    Save-VisualToPng -visual $visual -width $width -height $height -outputPath $outPath
}

# -------------------------------------------------------------
# 2. 2:3 Poster Art (720 x 1080 px) - Recommended Store Poster
# -------------------------------------------------------------
function Generate-PosterArt720 {
    param([string]$outPath)

    $width = 720
    $height = 1080
    $visual = New-Object System.Windows.Media.DrawingVisual
    $dc = $visual.RenderOpen()

    # Background
    $bgBrush = New-Object System.Windows.Media.LinearGradientBrush(
        [System.Windows.Media.Color]::FromRgb(248, 250, 252),
        [System.Windows.Media.Color]::FromRgb(226, 232, 240),
        (New-Object System.Windows.Point(0, 0)),
        (New-Object System.Windows.Point(0, 1))
    )
    $dc.DrawRectangle($bgBrush, $null, (New-Object System.Windows.Rect(0, 0, $width, $height)))

    # Border
    $framePen = New-Object System.Windows.Media.Pen(
        (New-Object System.Windows.Media.SolidColorBrush([System.Windows.Media.Color]::FromRgb(203, 213, 225))), 2.0
    )
    $dc.DrawRoundedRectangle($null, $framePen, (New-Object System.Windows.Rect(16, 16, 688, 1048)), 32, 32)

    # Hero Badge (300x300)
    $badgeSize = 300
    $badgeX = [int](($width - $badgeSize) / 2)
    $badgeY = 110

    $badgeGrad = New-Object System.Windows.Media.LinearGradientBrush(
        [System.Windows.Media.Color]::FromRgb(2, 132, 199),
        [System.Windows.Media.Color]::FromRgb(99, 102, 241),
        (New-Object System.Windows.Point(0, 0)),
        (New-Object System.Windows.Point(1, 1))
    )
    $badgeRect = New-Object System.Windows.Rect($badgeX, $badgeY, $badgeSize, $badgeSize)
    $dc.DrawRoundedRectangle($badgeGrad, $null, $badgeRect, 60, 60)

    # Screen icon
    $whiteBrush = New-Object System.Windows.Media.SolidColorBrush([System.Windows.Media.Color]::FromRgb(255, 255, 255))
    $whitePen = New-Object System.Windows.Media.Pen($whiteBrush, 10.0)
    $whitePen.StartLineCap = [System.Windows.Media.PenLineCap]::Round
    $whitePen.EndLineCap = [System.Windows.Media.PenLineCap]::Round

    $monX = $badgeX + 70
    $monY = $badgeY + 65
    $dc.DrawRoundedRectangle($null, $whitePen, (New-Object System.Windows.Rect($monX, $monY, 160, 105)), 14, 14)

    $sp1 = New-Object System.Windows.Point(($badgeX + 150), ($monY + 105))
    $sp2 = New-Object System.Windows.Point(($badgeX + 150), ($monY + 140))
    $dc.DrawLine($whitePen, $sp1, $sp2)

    $sp3 = New-Object System.Windows.Point(($badgeX + 110), ($monY + 140))
    $sp4 = New-Object System.Windows.Point(($badgeX + 190), ($monY + 140))
    $dc.DrawLine($whitePen, $sp3, $sp4)

    $kbY = $badgeY + 225
    $kbp1 = New-Object System.Windows.Point(($badgeX + 60), $kbY)
    $kbp2 = New-Object System.Windows.Point(($badgeX + 240), $kbY)
    $dc.DrawLine($whitePen, $kbp1, $kbp2)

    # Title
    $culture = [System.Globalization.CultureInfo]::InvariantCulture
    $darkCharcoal = New-Object System.Windows.Media.SolidColorBrush([System.Windows.Media.Color]::FromRgb(15, 23, 42))
    $blueAccent = New-Object System.Windows.Media.SolidColorBrush([System.Windows.Media.Color]::FromRgb(2, 132, 199))
    $slateMuted = New-Object System.Windows.Media.SolidColorBrush([System.Windows.Media.Color]::FromRgb(71, 85, 105))

    $titleTypeface = New-Object System.Windows.Media.Typeface(
        (New-Object System.Windows.Media.FontFamily("Segoe UI, Arial")),
        [System.Windows.FontStyles]::Normal,
        [System.Windows.FontWeights]::Black,
        [System.Windows.FontStretches]::Normal
    )
    $titleText = New-Object System.Windows.Media.FormattedText(
        "ExamTyping",
        $culture,
        [System.Windows.FlowDirection]::LeftToRight,
        $titleTypeface,
        64,
        $darkCharcoal,
        1.0
    )
    $titleX = ($width - $titleText.Width) / 2
    $dc.DrawText($titleText, (New-Object System.Windows.Point($titleX, 450)))

    # Subtitle
    $subTypeface = New-Object System.Windows.Media.Typeface(
        (New-Object System.Windows.Media.FontFamily("Segoe UI, Arial")),
        [System.Windows.FontStyles]::Normal,
        [System.Windows.FontWeights]::Bold,
        [System.Windows.FontStretches]::Normal
    )
    $subText = New-Object System.Windows.Media.FormattedText(
        "TUTOR - EXAM SIMULATOR",
        $culture,
        [System.Windows.FlowDirection]::LeftToRight,
        $subTypeface,
        24,
        $blueAccent,
        1.0
    )
    $subX = ($width - $subText.Width) / 2
    $dc.DrawText($subText, (New-Object System.Windows.Point($subX, 530)))

    # Feature Pills
    $pillBrush = New-Object System.Windows.Media.SolidColorBrush([System.Windows.Media.Color]::FromRgb(255, 255, 255))
    $pillPen = New-Object System.Windows.Media.Pen((New-Object System.Windows.Media.SolidColorBrush([System.Windows.Media.Color]::FromRgb(203, 213, 225))), 1.5)

    $pills = @(
        "Official SSC - Railway - High Court Modes",
        "Hindi Remington GAIL and Inscript Layouts",
        "1,000+ Full-Length Practice Passages",
        "100% Offline with Complete Data Privacy"
    )

    $yPos = 590
    foreach ($pText in $pills) {
        $ft = New-Object System.Windows.Media.FormattedText(
            $pText,
            $culture,
            [System.Windows.FlowDirection]::LeftToRight,
            $subTypeface,
            16,
            $darkCharcoal,
            1.0
        )
        $pw = $ft.Width + 40
        $px = ($width - $pw) / 2
        $dc.DrawRoundedRectangle($pillBrush, $pillPen, (New-Object System.Windows.Rect($px, $yPos, $pw, 42)), 21, 21)
        $dc.DrawText($ft, (New-Object System.Windows.Point(($px + 20), ($yPos + 10))))
        $yPos += 58
    }

    # Bottom Footer
    $ctaTypeface = New-Object System.Windows.Media.Typeface(
        (New-Object System.Windows.Media.FontFamily("Segoe UI, Arial")),
        [System.Windows.FontStyles]::Normal,
        [System.Windows.FontWeights]::SemiBold,
        [System.Windows.FontStretches]::Normal
    )
    $ctaText = New-Object System.Windows.Media.FormattedText(
        "Master Speed and Accuracy for Government Tests",
        $culture,
        [System.Windows.FlowDirection]::LeftToRight,
        $ctaTypeface,
        18,
        $slateMuted,
        1.0
    )
    $ctaX = ($width - $ctaText.Width) / 2
    $dc.DrawText($ctaText, (New-Object System.Windows.Point($ctaX, 860)))

    $dc.Close()
    Save-VisualToPng -visual $visual -width $width -height $height -outputPath $outPath
}

Write-Host "Generating Microsoft Store Artwork..."
Generate-BoxArt1080 -outPath (Join-Path $assetsDir "BoxArt_1080x1080.png")
Generate-PosterArt720 -outPath (Join-Path $assetsDir "PosterArt_720x1080.png")
Write-Host "Successfully generated all Microsoft Store artwork!"
