# IncliSafe Development Workflow Master Script
# Author: Claude AI
# Version: 1.0.0

$ErrorActionPreference = 'Stop'
$VerbosePreference = 'Continue'

# Configuration
$config = @{
    WorkspacePath = $PSScriptRoot
    ScriptsPath = $PSScriptRoot
    LogsPath = "..\logs"
    BackupPath = "..\backups"
}

# Import required scripts
. "$($config.ScriptsPath)\IncliSafe-Master.ps1"
. "$($config.ScriptsPath)\Fix-Errors.ps1"
. "$($config.ScriptsPath)\Update-Documentation.ps1"
. "$($config.ScriptsPath)\Test-Quality.ps1"

# Initialize development environment
function Initialize-Development {
    Write-Verbose "Initializing development environment..."
    
    # Create required directories
    @(
        $config.LogsPath,
        $config.BackupPath,
        "$($config.LogsPath)\errors",
        "$($config.LogsPath)\builds"
    ) | ForEach-Object {
        if (-not (Test-Path $_)) {
            New-Item -ItemType Directory -Path $_ -Force
        }
    }
    
    # Install required global tools
    @(
        "dotnet-format",
        "dotnet-reportgenerator-globaltool",
        "coverlet.console",
        "security-scan"
    ) | ForEach-Object {
        if (-not (Get-Command $_ -ErrorAction SilentlyContinue)) {
            Write-Verbose "Installing $_ tool..."
            dotnet tool install -g $_
        }
    }
}

# Start development session
function Start-DevelopmentSession {
    Write-Verbose "Starting development session..."
    
    # Update from repository
    git pull origin develop
    
    # Restore dependencies
    dotnet restore ..\IncliSafe.sln
    
    # Initial build
    dotnet build ..\IncliSafe.sln
}

# Monitor for changes and run quality checks
function Start-DevelopmentMonitor {
    Write-Verbose "Starting development monitor..."
    
    # Create FileSystemWatcher
    $watcher = New-Object System.IO.FileSystemWatcher
    $watcher.Path = ".."
    $watcher.IncludeSubdirectories = $true
    $watcher.EnableRaisingEvents = $true
    
    # Define event handlers
    $action = {
        $path = $Event.SourceEventArgs.FullPath
        $changeType = $Event.SourceEventArgs.ChangeType
        $timeStamp = (Get-Date).ToString("yyyy-MM-dd HH:mm:ss")
        
        Write-Host "[$timeStamp] $changeType detected in $path"
        
        # Run quality checks on changes
        if ($path -match "\.(cs|razor)$") {
            Write-Host "Running quality checks..."
            & "$($config.ScriptsPath)\Test-Quality.ps1"
        }
        
        # Update documentation on relevant changes
        if ($path -match "\.(cs|md|razor)$") {
            Write-Host "Updating documentation..."
            & "$($config.ScriptsPath)\Update-Documentation.ps1"
        }
    }
    
    # Register event handlers
    Register-ObjectEvent $watcher "Created" -Action $action
    Register-ObjectEvent $watcher "Changed" -Action $action
    Register-ObjectEvent $watcher "Deleted" -Action $action
    Register-ObjectEvent $watcher "Renamed" -Action $action
    
    Write-Host "Development monitor started. Press Ctrl+C to stop."
    
    try {
        while ($true) { Start-Sleep -Seconds 1 }
    }
    finally {
        $watcher.EnableRaisingEvents = $false
        $watcher.Dispose()
        Get-EventSubscriber | Unregister-Event
    }
}

# Main execution flow
try {
    Write-Host "Starting IncliSafe development environment..." -ForegroundColor Cyan
    
    # Initialize environment
    Initialize-Development
    
    # Start development session
    Start-DevelopmentSession
    
    # Run initial quality checks
    & "$($config.ScriptsPath)\Test-Quality.ps1"
    
    # Generate initial documentation
    & "$($config.ScriptsPath)\Update-Documentation.ps1"
    
    # Start monitoring
    Start-DevelopmentMonitor
    
    Write-Host "Development environment started successfully!" -ForegroundColor Green
}
catch {
    Write-Error "Error starting development environment: $_"
    exit 1
} 