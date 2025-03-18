# Error Analysis and Fix Script
# Author: Claude AI
# Version: 1.0.0

$ErrorActionPreference = 'Stop'
$VerbosePreference = 'Continue'

# Configuration
$config = @{
    SolutionPath = (Resolve-Path "..\IncliSafe.sln")
    ErrorLogPath = "..\logs\errors"
    SolutionsPath = "..\docs\solutions.txt"
}

# Common error patterns and their fixes
$errorPatterns = @{
    "CS0246" = @{
        Description = "The type or namespace name could not be found"
        Fix = {
            param($errorMessage)
            # Extract missing type
            if ($errorMessage -match "CS0246: The type or namespace name '(.+?)' could not be found") {
                $missingType = $matches[1]
                Write-Verbose "Attempting to fix missing type: $missingType"
                
                # Common namespace mappings
                $namespaces = @{
                    "ILogger" = "Microsoft.Extensions.Logging"
                    "DbContext" = "Microsoft.EntityFrameworkCore"
                    "JsonSerializer" = "System.Text.Json"
                }
                
                if ($namespaces.ContainsKey($missingType)) {
                    return "using $($namespaces[$missingType]);"
                }
            }
        }
    }
    "CS0103" = @{
        Description = "The name does not exist in the current context"
        Fix = {
            param($errorMessage)
            if ($errorMessage -match "CS0103: The name '(.+?)' does not exist") {
                $missingName = $matches[1]
                Write-Verbose "Checking for missing field: $missingName"
                
                # Check if it's a common property name
                $commonProps = @("Id", "Name", "Description", "CreatedAt", "UpdatedAt")
                if ($commonProps -contains $missingName) {
                    return "public string $missingName { get; set; }"
                }
            }
        }
    }
}

# Parse build errors
function Get-BuildErrors {
    Write-Verbose "Analyzing build errors..."
    
    $errorLog = "$($config.ErrorLogPath)\$(Get-Date -Format 'yyyy-MM-dd').log"
    if (Test-Path $errorLog) {
        return Get-Content $errorLog
    }
    
    return @()
}

# Check if a solution exists for the error
function Get-ExistingSolution {
    param($errorMessage)
    
    if (Test-Path $config.SolutionsPath) {
        $solutions = Get-Content $config.SolutionsPath
        foreach ($solution in $solutions) {
            if ($solution -match "ERROR: (.+)") {
                $storedError = $matches[1]
                if ($errorMessage -like "*$storedError*") {
                    return ($solution -split "SOLUTION: ")[1]
                }
            }
        }
    }
    
    return $null
}

# Apply fixes to the code
function Apply-ErrorFix {
    param($errorMessage)
    
    # Check for existing solution
    $existingSolution = Get-ExistingSolution $errorMessage
    if ($existingSolution) {
        Write-Verbose "Found existing solution: $existingSolution"
        return $existingSolution
    }
    
    # Try to match error pattern
    foreach ($pattern in $errorPatterns.Keys) {
        if ($errorMessage -match $pattern) {
            $fix = $errorPatterns[$pattern].Fix
            $solution = & $fix $errorMessage
            
            if ($solution) {
                # Store the solution
                $solutionEntry = "ERROR: $errorMessage`nSOLUTION: $solution"
                Add-Content -Path $config.SolutionsPath -Value $solutionEntry
                
                return $solution
            }
        }
    }
    
    return $null
}

# Main execution flow
try {
    $errors = Get-BuildErrors
    
    if ($errors.Count -eq 0) {
        Write-Host "No errors found!" -ForegroundColor Green
        exit 0
    }
    
    Write-Verbose "Found $($errors.Count) errors to analyze"
    
    foreach ($errorMessage in $errors) {
        Write-Verbose "Processing error: $errorMessage"
        
        $fix = Apply-ErrorFix $errorMessage
        if ($fix) {
            Write-Host "Found fix for error: $errorMessage" -ForegroundColor Green
            Write-Host "Fix: $fix" -ForegroundColor Cyan
        }
        else {
            Write-Warning "No automatic fix available for: $errorMessage"
        }
    }
}
catch {
    Write-Error "Error during error analysis: $_"
    exit 1
} 