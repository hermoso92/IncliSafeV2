# Code Quality and Testing Script
# Author: Claude AI
# Version: 1.0.0

param(
    [switch]$FixIssues
)

$ErrorActionPreference = 'Stop'
$VerbosePreference = 'Continue'
$root = Split-Path -Parent (Split-Path -Parent $MyInvocation.MyCommand.Path)

# Configuration
$config = @{
    SolutionPath = "..\IncliSafe.sln"
    TestsPath = "..\IncliSafe.Tests"
    ReportsPath = "..\reports"
    CoveragePath = "..\reports\coverage"
    QualityPath = "..\reports\quality"
}

# Initialize reporting directories
function Initialize-Reports {
    Write-Verbose "Initializing report directories..."
    
    @(
        $config.ReportsPath,
        $config.CoveragePath,
        $config.QualityPath
    ) | ForEach-Object {
        if (-not (Test-Path $_)) {
            New-Item -ItemType Directory -Path $_ -Force
        }
    }
}

# Run all tests with coverage
function Test-Solution {
    Write-Verbose "Running tests with coverage..."
    
    # Install required tools if not present
    if (-not (Get-Command reportgenerator -ErrorAction SilentlyContinue)) {
        dotnet tool install -g dotnet-reportgenerator-globaltool
    }
    
    if (-not (Get-Command coverlet -ErrorAction SilentlyContinue)) {
        dotnet tool install -g coverlet.console
    }
    
    # Run tests with coverage
    $testResult = dotnet test $config.TestsPath `
        /p:CollectCoverage=true `
        /p:CoverletOutputFormat=cobertura `
        /p:CoverletOutput="$($config.CoveragePath)\coverage.xml" `
        /p:Exclude="[*]*.Migrations.*"
    
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Tests failed. See test output for details."
        return $false
    }
    
    # Generate coverage report
    reportgenerator `
        -reports:"$($config.CoveragePath)\coverage.xml" `
        -targetdir:"$($config.CoveragePath)\html" `
        -reporttypes:Html
    
    return $true
}

# Run code analysis
function Test-CodeQuality {
    Write-Step "Analizando calidad del código"
    
    # Ejecutar análisis de código
    dotnet format --verify-no-changes --report quality-report.json
    
    if ($LASTEXITCODE -ne 0 -and $FixIssues) {
        Write-Host "Corrigiendo problemas de formato..." -ForegroundColor Yellow
        dotnet format
    }
    
    # Ejecutar SonarAnalyzer
    dotnet build /p:TreatWarningsAsErrors=true
    
    # Verificar reglas de estilo
    Write-Step "Verificando reglas de estilo"
    
    $rules = @(
        @{
            Name = "Uso de var"
            Pattern = '\bvar\b'
            Message = "Usar tipos explícitos en lugar de var"
        },
        @{
            Name = "Métodos async sin Async"
            Pattern = 'public\s+Task\s+\w+[^Async]\('
            Message = "Los métodos async deben terminar en Async"
        },
        @{
            Name = "Uso de dynamic"
            Pattern = '\bdynamic\b'
            Message = "Evitar uso de dynamic"
        }
    )
    
    $files = Get-ChildItem -Path $root -Include *.cs,*.razor -Recurse
    
    foreach ($file in $files) {
        $content = Get-Content $file
        
        foreach ($rule in $rules) {
            $violations = $content | Select-String -Pattern $rule.Pattern
            
            if ($violations) {
                Write-Host "`nViolación de regla '$($rule.Name)' en $($file.Name):" -ForegroundColor Yellow
                Write-Host $rule.Message
                
                foreach ($violation in $violations) {
                    Write-Host "Línea $($violation.LineNumber): $($violation.Line.Trim())"
                }
            }
        }
    }
}

function Test-Performance {
    Write-Step "Analizando rendimiento"
    
    # Verificar uso de memoria
    Write-Host "Analizando uso de memoria..."
    dotnet-counters monitor --process-id $pid --refresh-interval 1 --duration 10
    
    # Verificar tiempo de respuesta
    Write-Host "`nAnalizando tiempo de respuesta..."
    $endpoints = @(
        "https://localhost:7001/api/vehicles",
        "https://localhost:7001/api/analysis"
    )
    
    foreach ($endpoint in $endpoints) {
        try {
            $response = Measure-Command { 
                Invoke-WebRequest -Uri $endpoint -UseBasicParsing
            }
            
            Write-Host "$endpoint : $($response.TotalMilliseconds)ms"
        }
        catch {
            Write-Warning "No se pudo probar $endpoint"
        }
    }
}

# Check for common security issues
function Test-Security {
    Write-Step "Analizando seguridad"
    
    # Verificar secretos expuestos
    Write-Host "Buscando secretos expuestos..."
    $patterns = @(
        'password\s*=\s*"[^"]*"',
        'connectionString\s*=\s*"[^"]*"',
        'apiKey\s*=\s*"[^"]*"'
    )
    
    $files = Get-ChildItem -Path $root -Include *.cs,*.json,*.config -Recurse
    
    foreach ($file in $files) {
        if (Test-Path $file) {
            $content = Get-Content $file
            
            foreach ($pattern in $patterns) {
                $matches = $content | Select-String -Pattern $pattern
                
                if ($null -ne $matches) {
                    Write-Warning "Posible secreto expuesto en $($file.Name):"
                    foreach ($match in $matches) {
                        if ($null -ne $match) {
                            Write-Host "Línea $($match.LineNumber): $($match.Line.Trim())"
                        }
                    }
                }
            }
        }
    }
    
    # Verificar vulnerabilidades en dependencias
    Write-Host "`nVerificando vulnerabilidades en dependencias..."
    dotnet list package --vulnerable-packages
}

# Generate quality report
function New-QualityReport {
    Write-Verbose "Generating quality report..."
    
    $date = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
    $report = @"
# Code Quality Report
Generated: $date

## Test Results
- Location: $($config.CoveragePath)\html\index.html
- Status: $($LASTEXITCODE -eq 0 ? "✅ Passed" : "❌ Failed")

## Code Quality
- Format Check: $($formatResult -eq $null ? "✅ Passed" : "⚠️ Issues Found")
- Build Warnings: $(Get-Content "$($config.QualityPath)\build.log" | Where-Object { $_ -match "warning" } | Measure-Object).Count

## Security Scan
- Report: $($config.QualityPath)\security-report.json
- Critical Issues: $(Get-Content "$($config.QualityPath)\security-report.json" | ConvertFrom-Json | Where-Object { $_.Severity -eq "Critical" } | Measure-Object).Count

## Recommendations
$(if ($LASTEXITCODE -ne 0) { "- Fix failing tests`n" })
$(if ($formatResult) { "- Run code formatting`n" })
$(if ($scanResult) { "- Address security issues`n" })
"@
    
    Set-Content -Path "$($config.QualityPath)\quality-report.md" -Value $report
}

# Main execution flow
try {
    Initialize-Reports
    
    $testsPassed = Test-Solution
    $qualityPassed = Test-CodeQuality
    $securityPassed = Test-Security
    
    New-QualityReport
    
    if ($testsPassed -and $qualityPassed -and $securityPassed) {
        Write-Host "All quality checks passed!" -ForegroundColor Green
        exit 0
    }
    else {
        Write-Error "Quality checks failed. See reports for details."
        exit 1
    }
}
catch {
    Write-Error "Error during quality checks: $_"
    exit 1
} 