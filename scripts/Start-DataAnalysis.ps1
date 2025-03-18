# Real-time Data Analysis Script
# Author: Claude AI
# Version: 1.0.0

$ErrorActionPreference = 'Stop'
$VerbosePreference = 'Continue'

# Configuration
$config = @{
    DataPath = "..\data"
    AnalysisPath = "..\analysis"
    AlertsPath = "..\alerts"
    ThresholdsPath = "..\config\thresholds.json"
    SamplingWindow = 100  # samples
    UpdateInterval = 1000  # milliseconds
}

# Initialize analysis environment
function Initialize-AnalysisEnvironment {
    Write-Verbose "Initializing analysis environment..."
    
    # Create required directories
    @(
        $config.DataPath,
        $config.AnalysisPath,
        $config.AlertsPath
    ) | ForEach-Object {
        if (-not (Test-Path $_)) {
            New-Item -ItemType Directory -Path $_ -Force
        }
    }
    
    # Create default thresholds if not exists
    if (-not (Test-Path $config.ThresholdsPath)) {
        @{
            Acceleration = @{
                Warning = @{
                    X = 0.5  # g
                    Y = 0.5  # g
                    Z = 0.5  # g
                }
                Critical = @{
                    X = 1.0  # g
                    Y = 1.0  # g
                    Z = 1.0  # g
                }
            }
            Angle = @{
                Warning = @{
                    X = 15.0  # degrees
                    Y = 15.0  # degrees
                }
                Critical = @{
                    X = 30.0  # degrees
                    Y = 30.0  # degrees
                }
            }
            Vibration = @{
                Warning = 0.5  # g RMS
                Critical = 1.0  # g RMS
            }
        } | ConvertTo-Json -Depth 10 | Set-Content $config.ThresholdsPath
    }
}

# Calculate statistics for a data window
function Get-WindowStatistics {
    param (
        [double[]]$data
    )
    
    if ($data.Count -eq 0) {
        return $null
    }
    
    $stats = @{
        Mean = ($data | Measure-Object -Average).Average
        StdDev = [Math]::Sqrt(($data | ForEach-Object { [Math]::Pow($_ - ($data | Measure-Object -Average).Average, 2) } | Measure-Object -Average).Average)
        Min = ($data | Measure-Object -Minimum).Minimum
        Max = ($data | Measure-Object -Maximum).Maximum
        RMS = [Math]::Sqrt(($data | ForEach-Object { [Math]::Pow($_, 2) } | Measure-Object -Average).Average)
    }
    
    return $stats
}

# Process real-time data
function Start-DataProcessing {
    Write-Verbose "Starting real-time data processing..."
    
    # Load thresholds
    $thresholds = Get-Content $config.ThresholdsPath | ConvertFrom-Json
    
    # Initialize data windows
    $windowX = New-Object System.Collections.Queue
    $windowY = New-Object System.Collections.Queue
    $windowZ = New-Object System.Collections.Queue
    
    try {
        while ($true) {
            # Get latest data from MCP server
            $response = Invoke-RestMethod -Uri "http://localhost:5000/api/telemetry/latest"
            
            # Update data windows
            $windowX.Enqueue($response.x)
            $windowY.Enqueue($response.y)
            $windowZ.Enqueue($response.z)
            
            if ($windowX.Count -gt $config.SamplingWindow) {
                $windowX.Dequeue()
                $windowY.Dequeue()
                $windowZ.Dequeue()
            }
            
            # Calculate statistics
            $statsX = Get-WindowStatistics $windowX.ToArray()
            $statsY = Get-WindowStatistics $windowY.ToArray()
            $statsZ = Get-WindowStatistics $windowZ.ToArray()
            
            # Calculate angles
            $angleX = [Math]::Atan2($statsX.Mean, [Math]::Sqrt([Math]::Pow($statsY.Mean, 2) + [Math]::Pow($statsZ.Mean, 2))) * 180 / [Math]::PI
            $angleY = [Math]::Atan2($statsY.Mean, [Math]::Sqrt([Math]::Pow($statsX.Mean, 2) + [Math]::Pow($statsZ.Mean, 2))) * 180 / [Math]::PI
            
            # Calculate vibration
            $vibration = [Math]::Sqrt([Math]::Pow($statsX.RMS, 2) + [Math]::Pow($statsY.RMS, 2) + [Math]::Pow($statsZ.RMS, 2))
            
            # Check thresholds and generate alerts
            $timestamp = Get-Date -Format "yyyy-MM-dd HH:mm:ss.fff"
            $alerts = @()
            
            # Acceleration alerts
            if ([Math]::Abs($statsX.Mean) -gt $thresholds.Acceleration.Critical.X) {
                $alerts += "CRITICAL: X acceleration exceeded threshold: $($statsX.Mean) g"
            }
            elseif ([Math]::Abs($statsX.Mean) -gt $thresholds.Acceleration.Warning.X) {
                $alerts += "WARNING: X acceleration near threshold: $($statsX.Mean) g"
            }
            
            # Angle alerts
            if ([Math]::Abs($angleX) -gt $thresholds.Angle.Critical.X) {
                $alerts += "CRITICAL: X angle exceeded threshold: $angleX degrees"
            }
            elseif ([Math]::Abs($angleX) -gt $thresholds.Angle.Warning.X) {
                $alerts += "WARNING: X angle near threshold: $angleX degrees"
            }
            
            # Vibration alerts
            if ($vibration -gt $thresholds.Vibration.Critical) {
                $alerts += "CRITICAL: Excessive vibration detected: $vibration g RMS"
            }
            elseif ($vibration -gt $thresholds.Vibration.Warning) {
                $alerts += "WARNING: High vibration detected: $vibration g RMS"
            }
            
            # Log alerts
            if ($alerts.Count -gt 0) {
                $alertLog = "$timestamp`n" + ($alerts -join "`n")
                Add-Content -Path (Join-Path $config.AlertsPath "alerts.log") -Value $alertLog
                
                # Send alerts to SignalR hub
                Invoke-RestMethod -Uri "http://localhost:5000/api/alerts" -Method Post -Body @{
                    timestamp = $timestamp
                    alerts = $alerts
                }
            }
            
            # Save analysis results
            $analysis = @{
                Timestamp = $timestamp
                Acceleration = @{
                    X = $statsX
                    Y = $statsY
                    Z = $statsZ
                }
                Angles = @{
                    X = $angleX
                    Y = $angleY
                }
                Vibration = $vibration
                Alerts = $alerts
            }
            
            $analysisPath = Join-Path $config.AnalysisPath "analysis-$(Get-Date -Format 'yyyy-MM-dd').json"
            $analysis | ConvertTo-Json -Depth 10 | Add-Content $analysisPath
            
            Start-Sleep -Milliseconds $config.UpdateInterval
        }
    }
    catch {
        Write-Error "Error in data processing: $_"
        throw
    }
}

# Main execution flow
try {
    Write-Host "Starting IncliSafe Data Analysis..." -ForegroundColor Cyan
    
    Initialize-AnalysisEnvironment
    Start-DataProcessing
}
catch {
    Write-Error "Error in data analysis: $_"
    exit 1
} 