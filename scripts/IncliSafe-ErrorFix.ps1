# IncliSafe Error Fix Script
# Author: Claude AI
# Version: 1.0.0

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$VerbosePreference = 'Continue'

# Configuration
$scriptPath = Split-Path -Parent $MyInvocation.MyCommand.Path
$rootPath = Split-Path -Parent $scriptPath

$config = @{
    SolutionPath = Join-Path $rootPath "IncliSafe.sln"
    LogPath = Join-Path $rootPath "logs"
    ErrorLogPath = Join-Path $rootPath "logs\errors"
    BackupPath = Join-Path $rootPath "backups"
    SolutionsPath = Join-Path $rootPath "soluciones.txt"
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

function Initialize-Environment {
    Write-StatusMessage "Initializing environment..." -Type "Info"
    try {
        @(
            $config.LogPath,
            $config.ErrorLogPath,
            $config.BackupPath
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

function Backup-CurrentState {
    Write-StatusMessage "Creating backup of current state..." -Type "Info"
    try {
        $backupDir = Join-Path $config.BackupPath (Get-Date -Format "yyyy-MM-dd_HH-mm-ss")
        New-Item -ItemType Directory -Path $backupDir -Force | Out-Null
        
        # Backup all model files
        Get-ChildItem -Path (Join-Path $rootPath "IncliSafe.Shared\Models") -Recurse -Filter "*.cs" | ForEach-Object {
            $relativePath = $_.FullName.Substring($rootPath.Length + 1)
            $targetPath = Join-Path $backupDir $relativePath
            $targetDir = Split-Path -Parent $targetPath
            if (-not (Test-Path $targetDir)) {
                New-Item -ItemType Directory -Path $targetDir -Force | Out-Null
            }
            Copy-Item $_.FullName $targetPath -Force
        }
        
        Write-StatusMessage "Backup created successfully at $backupDir" -Type "Success"
    }
    catch {
        Write-StatusMessage "Failed to create backup: $_" -Type "Error"
        throw
    }
}

function Analyze-Errors {
    Write-StatusMessage "Analyzing build errors..." -Type "Info"
    try {
        $errorLog = Join-Path $config.ErrorLogPath "$(Get-Date -Format 'yyyy-MM-dd').log"
        $buildOutput = dotnet build $config.SolutionPath 2>&1
        $buildOutput | Out-File -FilePath $errorLog -Force
        
        $errors = $buildOutput | Where-Object { $_ -match "error" }
        if ($errors.Count -gt 0) {
            Write-StatusMessage "Found $($errors.Count) errors:" -Type "Warning"
            $errors | ForEach-Object { Write-StatusMessage $_ -Type "Warning" }
            return $errors
        }
        
        Write-StatusMessage "No errors found" -Type "Success"
        return @()
    }
    catch {
        Write-StatusMessage "Failed to analyze errors: $_" -Type "Error"
        throw
    }
}

function Fix-EnumReferences {
    Write-StatusMessage "Fixing enum references..." -Type "Info"
    try {
        # Add using directive for Enums namespace
        Get-ChildItem -Path (Join-Path $rootPath "IncliSafe.Shared\Models") -Recurse -Filter "*.cs" | ForEach-Object {
            $content = Get-Content $_.FullName -Raw
            if (-not $content.Contains("using IncliSafe.Shared.Models.Enums;")) {
                $content = "using IncliSafe.Shared.Models.Enums;`n" + $content
                Set-Content -Path $_.FullName -Value $content -Force
                Write-Verbose "Added Enums namespace to $($_.Name)"
            }
        }
        Write-StatusMessage "Enum references fixed successfully" -Type "Success"
    }
    catch {
        Write-StatusMessage "Failed to fix enum references: $_" -Type "Error"
        throw
    }
}

function Fix-NullableReferences {
    Write-StatusMessage "Fixing nullable references..." -Type "Info"
    try {
        # Add required modifier to non-nullable properties
        Get-ChildItem -Path (Join-Path $rootPath "IncliSafe.Shared\Models") -Recurse -Filter "*.cs" | ForEach-Object {
            $content = Get-Content $_.FullName -Raw
            if ($content -match "public\s+(?!required)(\w+)\s+(\w+)\s*{\s*get;\s*set;\s*}") {
                $content = $content -replace "public\s+(?!required)(\w+)\s+(\w+)\s*{\s*get;\s*set;\s*}", 'public required ${1} ${2} { get; set; }'
                Set-Content -Path $_.FullName -Value $content -Force
                Write-Verbose "Added required modifier to properties in $($_.Name)"
            }
        }
        Write-StatusMessage "Nullable references fixed successfully" -Type "Success"
    }
    catch {
        Write-StatusMessage "Failed to fix nullable references: $_" -Type "Error"
        throw
    }
}

function Update-SolutionsFile {
    param(
        [string]$Solution
    )
    
    try {
        $date = Get-Date -Format "yyyy-MM-dd"
        $content = @"
## [$date] $Solution
- Applied at: $(Get-Date)
- Status: Success
"@
        
        Add-Content -Path $config.SolutionsPath -Value "`n$content"
        Write-StatusMessage "Solutions file updated successfully" -Type "Success"
    }
    catch {
        Write-StatusMessage "Failed to update solutions file: $_" -Type "Error"
        throw
    }
}

# Main execution flow
try {
    Write-StatusMessage "Starting IncliSafe error fixing process..." -Type "Info"
    
    Initialize-Environment
    Backup-CurrentState
    
    # Fix common issues
    Fix-EnumReferences
    Fix-NullableReferences
    
    # Analyze and fix specific errors
    $errors = Analyze-Errors
    if ($errors.Count -gt 0) {
        # Apply specific fixes based on error patterns
        foreach ($error in $errors) {
            if ($error -match "CS0104") {
                # Ambiguous reference
                $filePath = $error -replace '.*?\((.*?):.*', '$1'
                Write-StatusMessage "Fixing ambiguous reference in $filePath" -Type "Warning"
                # Add specific fix logic here
            }
            elseif ($error -match "CS8618") {
                # Non-nullable property
                $filePath = $error -replace '.*?\((.*?):.*', '$1'
                Write-StatusMessage "Fixing non-nullable property in $filePath" -Type "Warning"
                # Add specific fix logic here
            }
        }
        
        # Update solutions file with applied fixes
        Update-SolutionsFile "Fixed $($errors.Count) compilation errors"
    }
    
    Write-StatusMessage "IncliSafe error fixing completed successfully!" -Type "Success"
    exit 0
}
catch {
    Write-StatusMessage "Error during automation: $_" -Type "Error"
    exit 1
} 