# Store\Generate-Assets.ps1
# Generates official Windows Store PNG assets using WPF DrawingVisual vector rendering.

Add-Type -AssemblyName PresentationCore, PresentationFramework, WindowsBase

$storeDir = if ($PSScriptRoot) { $PSScriptRoot } elseif ($MyInvocation.MyCommand.Path) { Split-Path -Parent $MyInvocation.MyCommand.Path } else { Join-Path (Get-Location) "Store" }
$assetsDir = Join-Path $storeDir "Assets"

if (-not (Test-Path $assetsDir)) {
    New-Item -ItemType Directory -Path $assetsDir -Force | Out-Null
}

function Save-DrawingVisualAsPng {
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
    Write-Host "Generated: $outputPath ($width x $height)"
}

function Draw-TileIcon {
    param(
        [int]$width,
        [int]$height,
        [string]$outPath,
        [bool]$isWide = $false,
        [bool]$isSplash = $false
    )

    $visual = New-Object System.Windows.Media.DrawingVisual
    $dc = $visual.RenderOpen()

    # Colors
    $darkBgColor = [System.Windows.Media.Color]::FromRgb(15, 23, 42) # #0f172a
    $cardBgColor = [System.Windows.Media.Color]::FromRgb(30, 41, 59) # #1e293b
    $gradStart = [System.Windows.Media.Color]::FromRgb(99, 102, 241)  # #6366f1
    $gradEnd = [System.Windows.Media.Color]::FromRgb(168, 85, 247)   # #a855f7
    $cyanAccent = [System.Windows.Media.Color]::FromRgb(56, 189, 248) # #38bdf8
    $whiteColor = [System.Windows.Media.Color]::FromRgb(255, 255, 255)
    $mutedWhite = [System.Windows.Media.Color]::FromArgb(200, 241, 245, 249)

    $bgBrush = New-Object System.Windows.Media.SolidColorBrush($darkBgColor)
    $dc.DrawRectangle($bgBrush, $null, (New-Object System.Windows.Rect(0, 0, $width, $height)))

    if ($isSplash) {
        # Splash Screen: 620 x 300
        $badgeSize = 100
        $badgeX = ($width / 2) - ($badgeSize / 2)
        $badgeY = 55

        # Gradient Badge
        $linGrad = New-Object System.Windows.Media.LinearGradientBrush(
            $gradStart, $gradEnd,
            (New-Object System.Windows.Point(0, 0)),
            (New-Object System.Windows.Point(1, 1))
        )
        $badgeRect = New-Object System.Windows.Rect($badgeX, $badgeY, $badgeSize, $badgeSize)
        $dc.DrawRoundedRectangle($linGrad, $null, $badgeRect, 24, 24)

        # Inner Keyboard / Monitor icon
        $pen = New-Object System.Windows.Media.Pen(
            (New-Object System.Windows.Media.SolidColorBrush($whiteColor)), 4.0
        )
        $pen.StartLineCap = [System.Windows.Media.PenLineCap]::Round
        $pen.EndLineCap = [System.Windows.Media.PenLineCap]::Round

        # Screen outline
        $sx = $badgeX + 22
        $sy = $badgeY + 22
        $screenRect = New-Object System.Windows.Rect($sx, $sy, 56, 38)
        $dc.DrawRoundedRectangle($null, $pen, $screenRect, 6, 6)
        
        # Keyboard base
        $p1 = New-Object System.Windows.Point(($badgeX + 30), ($badgeY + 68))
        $p2 = New-Object System.Windows.Point(($badgeX + 70), ($badgeY + 68))
        $dc.DrawLine($pen, $p1, $p2)

        $p3 = New-Object System.Windows.Point(($badgeX + 50), ($badgeY + 60))
        $p4 = New-Object System.Windows.Point(($badgeX + 50), ($badgeY + 68))
        $dc.DrawLine($pen, $p3, $p4)

        # Title text: "ExamTyping Tutor"
        $culture = [System.Globalization.CultureInfo]::InvariantCulture
        $typeFace = New-Object System.Windows.Media.Typeface(
            (New-Object System.Windows.Media.FontFamily("Segoe UI, Arial")),
            [System.Windows.FontStyles]::Normal,
            [System.Windows.FontWeights]::Bold,
            [System.Windows.FontStretches]::Normal
        )
        $titleText = New-Object System.Windows.Media.FormattedText(
            "ExamTyping Tutor",
            $culture,
            [System.Windows.FlowDirection]::LeftToRight,
            $typeFace,
            30,
            (New-Object System.Windows.Media.SolidColorBrush($whiteColor)),
            1.0
        )
        $titleX = ($width - $titleText.Width) / 2
        $dc.DrawText($titleText, (New-Object System.Windows.Point($titleX, 175)))

        # Subtitle: "Official Government & Multi-Language Typing Suite"
        $subTypeFace = New-Object System.Windows.Media.Typeface(
            (New-Object System.Windows.Media.FontFamily("Segoe UI, Arial")),
            [System.Windows.FontStyles]::Normal,
            [System.Windows.FontWeights]::SemiBold,
            [System.Windows.FontStretches]::Normal
        )
        $subText = New-Object System.Windows.Media.FormattedText(
            "Official Government & Multi-Language Typing Exam Suite",
            $culture,
            [System.Windows.FlowDirection]::LeftToRight,
            $subTypeFace,
            14,
            (New-Object System.Windows.Media.SolidColorBrush($cyanAccent)),
            1.0
        )
        $subX = ($width - $subText.Width) / 2
        $dc.DrawText($subText, (New-Object System.Windows.Point($subX, 220)))

    } elseif ($isWide) {
        # Wide Tile: 310 x 150
        $badgeSize = 80
        $badgeX = 35
        $badgeY = 35

        # Gradient Badge
        $linGrad = New-Object System.Windows.Media.LinearGradientBrush(
            $gradStart, $gradEnd,
            (New-Object System.Windows.Point(0, 0)),
            (New-Object System.Windows.Point(1, 1))
        )
        $badgeRect = New-Object System.Windows.Rect($badgeX, $badgeY, $badgeSize, $badgeSize)
        $dc.DrawRoundedRectangle($linGrad, $null, $badgeRect, 20, 20)

        # Icon inside badge
        $pen = New-Object System.Windows.Media.Pen(
            (New-Object System.Windows.Media.SolidColorBrush($whiteColor)), 3.5
        )
        $wsx = $badgeX + 18
        $wsy = $badgeY + 18
        $screenRect = New-Object System.Windows.Rect($wsx, $wsy, 44, 30)
        $dc.DrawRoundedRectangle($null, $pen, $screenRect, 4, 4)

        $wp1 = New-Object System.Windows.Point(($badgeX + 24), ($badgeY + 56))
        $wp2 = New-Object System.Windows.Point(($badgeX + 56), ($badgeY + 56))
        $dc.DrawLine($pen, $wp1, $wp2)

        $wp3 = New-Object System.Windows.Point(($badgeX + 40), ($badgeY + 48))
        $wp4 = New-Object System.Windows.Point(($badgeX + 40), ($badgeY + 56))
        $dc.DrawLine($pen, $wp3, $wp4)

        # Title text
        $culture = [System.Globalization.CultureInfo]::InvariantCulture
        $typeFace = New-Object System.Windows.Media.Typeface(
            (New-Object System.Windows.Media.FontFamily("Segoe UI, Arial")),
            [System.Windows.FontStyles]::Normal,
            [System.Windows.FontWeights]::Bold,
            [System.Windows.FontStretches]::Normal
        )
        $titleText = New-Object System.Windows.Media.FormattedText(
            "ExamTyping",
            $culture,
            [System.Windows.FlowDirection]::LeftToRight,
            $typeFace,
            24,
            (New-Object System.Windows.Media.SolidColorBrush($whiteColor)),
            1.0
        )
        $dc.DrawText($titleText, (New-Object System.Windows.Point(135, 42)))

        $subTypeFace = New-Object System.Windows.Media.Typeface(
            (New-Object System.Windows.Media.FontFamily("Segoe UI, Arial")),
            [System.Windows.FontStyles]::Normal,
            [System.Windows.FontWeights]::Normal,
            [System.Windows.FontStretches]::Normal
        )
        $subText = New-Object System.Windows.Media.FormattedText(
            "Tutor Pro Edition",
            $culture,
            [System.Windows.FlowDirection]::LeftToRight,
            $subTypeFace,
            14,
            (New-Object System.Windows.Media.SolidColorBrush($cyanAccent)),
            1.0
        )
        $dc.DrawText($subText, (New-Object System.Windows.Point(135, 75)))

    } else {
        # Square tiles: 150x150, 44x44, 50x50
        $pad = [math]::Max(2, [int]($width * 0.12))
        $badgeSize = $width - ($pad * 2)
        $cornerRad = [math]::Max(4, [int]($badgeSize * 0.22))

        # Gradient Badge
        $linGrad = New-Object System.Windows.Media.LinearGradientBrush(
            $gradStart, $gradEnd,
            (New-Object System.Windows.Point(0, 0)),
            (New-Object System.Windows.Point(1, 1))
        )
        $badgeRect = New-Object System.Windows.Rect($pad, $pad, $badgeSize, $badgeSize)
        $dc.DrawRoundedRectangle($linGrad, $null, $badgeRect, $cornerRad, $cornerRad)

        # Monogram "TT" or Keyboard Graphic
        $culture = [System.Globalization.CultureInfo]::InvariantCulture
        $fontSize = [int]($badgeSize * 0.52)
        $typeFace = New-Object System.Windows.Media.Typeface(
            (New-Object System.Windows.Media.FontFamily("Segoe UI, Arial")),
            [System.Windows.FontStyles]::Normal,
            [System.Windows.FontWeights]::ExtraBold,
            [System.Windows.FontStretches]::Normal
        )
        $text = New-Object System.Windows.Media.FormattedText(
            "ET",
            $culture,
            [System.Windows.FlowDirection]::LeftToRight,
            $typeFace,
            $fontSize,
            (New-Object System.Windows.Media.SolidColorBrush($whiteColor)),
            1.0
        )
        $tx = ($width - $text.Width) / 2
        $ty = ($height - $text.Height) / 2
        $dc.DrawText($text, (New-Object System.Windows.Point($tx, $ty)))
    }

    $dc.Close()
    Save-DrawingVisualAsPng -visual $visual -width $width -height $height -outputPath $outPath
}

Write-Host "--- Generating Windows Store Visual Assets ---"

# Base required assets
Draw-TileIcon -width 50  -height 50  -outPath (Join-Path $assetsDir "StoreLogo.png")
Draw-TileIcon -width 44  -height 44  -outPath (Join-Path $assetsDir "Square44x44Logo.png")
Draw-TileIcon -width 150 -height 150 -outPath (Join-Path $assetsDir "Square150x150Logo.png")
Draw-TileIcon -width 310 -height 150 -outPath (Join-Path $assetsDir "Wide310x150Logo.png") -isWide $true
Draw-TileIcon -width 620 -height 300 -outPath (Join-Path $assetsDir "SplashScreen.png") -isSplash $true

Write-Host "Successfully generated all Store visual assets in $assetsDir!"
