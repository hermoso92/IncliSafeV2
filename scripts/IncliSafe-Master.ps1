# IncliSafe Master Automation Script
# Author: Claude AI
# Version: 1.0.2

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$VerbosePreference = 'Continue'

# Configuration
$scriptPath = Split-Path -Parent $MyInvocation.MyCommand.Path
$rootPath = Split-Path -Parent $scriptPath

$config = @{
    SolutionPath = Join-Path $rootPath "IncliSafe.sln"
    LogPath = Join-Path $rootPath "logs"
    DocsPath = Join-Path $rootPath "docs"
    ErrorLogPath = Join-Path $rootPath "logs\errors"
    BackupPath = Join-Path $rootPath "backups"
    GitBranch = "develop"
}

function Write-StatusMessage {
    param(
        [string]$Message,
        [string]$Type = "Info"
    )
    
    $color = switch ($Type) {
        "Error" { "Red" }
        "Warning" { "Yellow" }
        "Success" { "Green" }
        default { "White" }
    }
    
    Write-Host "[$((Get-Date).ToString('yyyy-MM-dd HH:mm:ss'))] $Message" -ForegroundColor $color
}

# Create required directories
function Initialize-Environment {
    Write-StatusMessage "Initializing environment..." -Type "Info"
    try {
        @(
            $config.LogPath,
            $config.ErrorLogPath,
            $config.BackupPath,
            (Join-Path $config.DocsPath "daily-reports")
        ) | ForEach-Object {
            if (-not (Test-Path $_)) {
                New-Item -ItemType Directory -Path $_ -Force | Out-Null
                Write-Verbose "Created directory: $_"
            }
        }
        Write-StatusMessage "Environment initialized successfully" -Type "Success"
    }
    catch {
        Write-StatusMessage "Failed to initialize environment: $_" -Type "Error"
        throw
    }
}

# Update all NuGet packages to latest stable versions
function Update-Dependencies {
    Write-StatusMessage "Updating dependencies..." -Type "Info"
    try {
        # Restore NuGet packages first
        dotnet restore $config.SolutionPath
        
        # Update client packages
        $clientProj = Join-Path $rootPath "IncliSafe.Client\IncliSafe.Client.csproj"
        if (Test-Path $clientProj) {
            dotnet add $clientProj package MudBlazor --version 6.16.0
            dotnet add $clientProj package FluentValidation --version 11.11.0
            dotnet add $clientProj package Blazored.LocalStorage --version 4.4.0
        }
        
        # Update API packages
        $apiProj = Join-Path $rootPath "IncliSafeApi\IncliSafeApi.csproj"
        if (Test-Path $apiProj) {
            dotnet add $apiProj package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.3
            dotnet add $apiProj package AutoMapper --version 12.0.1
        }
        
        Write-StatusMessage "Dependencies updated successfully" -Type "Success"
    }
    catch {
        Write-StatusMessage "Failed to update dependencies: $_" -Type "Error"
        throw
    }
}

# Run all tests and generate coverage report
function Test-Solution {
    Write-Verbose "Running tests..."
    try {
        dotnet test (Join-Path $rootPath "IncliSafe.Tests") /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura
        
        if (Get-Command reportgenerator -ErrorAction SilentlyContinue) {
            reportgenerator -reports:(Join-Path $rootPath "IncliSafe.Tests\coverage.cobertura.xml") -targetdir:(Join-Path $config.DocsPath "coverage")
        }
    }
    catch {
        Write-Error "Failed to run tests: $_"
        throw
    }
}

# Build solution and capture errors
function Build-Solution {
    Write-StatusMessage "Building solution..." -Type "Info"
    try {
        $buildLog = Join-Path $config.LogPath "build.log"
        $errorLog = Join-Path $config.ErrorLogPath "$(Get-Date -Format 'yyyy-MM-dd').log"
        
        # Clean the solution first
        dotnet clean $config.SolutionPath --configuration Release
        
        # Then build
        $buildOutput = dotnet build $config.SolutionPath --configuration Release 2>&1
        $buildOutput | Out-File -FilePath $buildLog -Force
        
        if ($LASTEXITCODE -ne 0) {
            $errors = $buildOutput | Where-Object { $_ -match "error" }
            $errors | Out-File -FilePath $errorLog -Force
            Write-StatusMessage "Build failed. See $errorLog for details." -Type "Error"
            Write-StatusMessage "Errors found:" -Type "Error"
            $errors | ForEach-Object { Write-StatusMessage $_ -Type "Error" }
            throw "Build failed with $($errors.Count) errors."
        }
        
        Write-StatusMessage "Build completed successfully" -Type "Success"
    }
    catch {
        Write-StatusMessage "Failed to build solution: $_" -Type "Error"
        throw
    }
}

# Generate documentation
function Update-Documentation {
    Write-StatusMessage "Updating documentation..." -Type "Info"
    try {
        $date = Get-Date -Format "yyyy-MM-dd"
        $report = @"
# Daily Report - $date

## Build Status
- Last Build: $(Get-Date)
- Status: $(if ($LASTEXITCODE -eq 0) { "Success" } else { "Failed" })

## Changes
$(git log --pretty=format:"- %s" -n 10)

## Errors
$(Get-Content (Join-Path $config.ErrorLogPath "$date.log") -ErrorAction SilentlyContinue)
"@
        
        Set-Content -Path (Join-Path $config.DocsPath "daily-reports\$date.md") -Value $report -Force
        Write-StatusMessage "Documentation updated successfully" -Type "Success"
    }
    catch {
        Write-StatusMessage "Failed to update documentation: $_" -Type "Error"
        throw
    }
}

# Main execution flow
try {
    Write-StatusMessage "Starting IncliSafe automation process..." -Type "Info"
    
    Initialize-Environment
    Update-Dependencies
    Test-Solution
    Build-Solution
    Update-Documentation
    
    Write-StatusMessage "IncliSafe automation completed successfully!" -Type "Success"
    exit 0
}
catch {
    Write-StatusMessage "Error during automation: $_" -Type "Error"
    exit 1
} 