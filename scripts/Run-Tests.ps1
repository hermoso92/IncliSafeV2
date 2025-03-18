# Script para ejecutar pruebas del proyecto
param(
    [string]$WorkspacePath = "C:\Users\Hermoso\Desktop\IncliSafeV2",
    [switch]$Coverage,
    [switch]$SkipIntegration
)

# Importar configuración
$configPath = Join-Path $WorkspacePath "config\project-settings.json"
$config = Get-Content $configPath | ConvertFrom-Json

# Función para ejecutar pruebas unitarias
function Invoke-UnitTests {
    Write-Host "Ejecutando pruebas unitarias..."
    
    $testProjects = Get-ChildItem -Path $WorkspacePath -Recurse -Filter "*Tests.csproj"
    $results = @()
    
    foreach ($project in $testProjects) {
        if ($project.Name -notlike "*Integration*") {
            Write-Host "Probando $($project.Name)..."
            
            if ($Coverage) {
                # Ejecutar con cobertura
                $result = dotnet test $project.FullName `
                    --collect:"XPlat Code Coverage" `
                    --results-directory "TestResults"
            }
            else {
                # Ejecutar sin cobertura
                $result = dotnet test $project.FullName
            }
            
            $results += @{
                Project = $project.Name
                Success = $LASTEXITCODE -eq 0
                Output = $result
            }
        }
    }
    
    return $results
}

# Función para ejecutar pruebas de integración
function Invoke-IntegrationTests {
    Write-Host "Ejecutando pruebas de integración..."
    
    $testProjects = Get-ChildItem -Path $WorkspacePath -Recurse -Filter "*Integration.Tests.csproj"
    $results = @()
    
    foreach ($project in $testProjects) {
        Write-Host "Probando $($project.Name)..."
        
        if ($Coverage) {
            # Ejecutar con cobertura
            $result = dotnet test $project.FullName `
                --collect:"XPlat Code Coverage" `
                --results-directory "TestResults"
        }
        else {
            # Ejecutar sin cobertura
            $result = dotnet test $project.FullName
        }
        
        $results += @{
            Project = $project.Name
            Success = $LASTEXITCODE -eq 0
            Output = $result
        }
    }
    
    return $results
}

# Función para generar reporte de cobertura
function New-CoverageReport {
    Write-Host "Generando reporte de cobertura..."
    
    # Instalar herramienta de reporte si no existe
    if (-not (Get-Command reportgenerator -ErrorAction SilentlyContinue)) {
        dotnet tool install -g dotnet-reportgenerator-globaltool
    }
    
    # Generar reporte
    $testResults = Join-Path $WorkspacePath "TestResults"
    $coverageReport = Join-Path $WorkspacePath "CoverageReport"
    
    reportgenerator `
        -reports:"$testResults\**\coverage.cobertura.xml" `
        -targetdir:$coverageReport `
        -reporttypes:Html
        
    return $coverageReport
}

# Función para actualizar registro de pruebas
function Update-TestLog {
    param(
        [array]$UnitTestResults,
        [array]$IntegrationTestResults,
        [string]$CoverageReportPath
    )
    
    $testLogPath = Join-Path $WorkspacePath "docs\test-log.md"
    $date = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
    
    # Crear entrada en el log
    $entry = @"

## Ejecución de Pruebas ($date)

### Pruebas Unitarias
$(foreach ($result in $UnitTestResults) {
    "- $($result.Project): $($result.Success ? '✓' : '✗')"
})

### Pruebas de Integración
$(foreach ($result in $IntegrationTestResults) {
    "- $($result.Project): $($result.Success ? '✓' : '✗')"
})

$(if ($CoverageReportPath) {
    "### Cobertura de Código
- Reporte generado en: $CoverageReportPath"
})
"@
    
    Add-Content $testLogPath $entry
}

# Función principal
function Invoke-Tests {
    try {
        # 1. Ejecutar pruebas unitarias
        $unitResults = Invoke-UnitTests
        
        # 2. Ejecutar pruebas de integración si no se omiten
        $integrationResults = @()
        if (-not $SkipIntegration) {
            $integrationResults = Invoke-IntegrationTests
        }
        
        # 3. Generar reporte de cobertura si se solicita
        $coverageReport = $null
        if ($Coverage) {
            $coverageReport = New-CoverageReport
        }
        
        # 4. Actualizar registro de pruebas
        Update-TestLog -UnitTestResults $unitResults `
            -IntegrationTestResults $integrationResults `
            -CoverageReportPath $coverageReport
        
        # 5. Verificar resultados
        $allSuccess = ($unitResults | Where-Object { -not $_.Success }).Count -eq 0 -and
                     ($integrationResults | Where-Object { -not $_.Success }).Count -eq 0
        
        if ($allSuccess) {
            Write-Host "Todas las pruebas pasaron correctamente."
            if ($coverageReport) {
                Write-Host "Reporte de cobertura generado en: $coverageReport"
            }
            exit 0
        }
        else {
            Write-Error "Algunas pruebas fallaron. Revisa el registro para más detalles."
            exit 1
        }
    }
    catch {
        Write-Error "Error al ejecutar las pruebas: $_"
        exit 1
    }
}

# Ejecutar pruebas
Invoke-Tests 