# Script para automatizar el proceso de actualización de estado y compilación
param(
    [string]$WorkspacePath = "C:\Users\Hermoso\Desktop\IncliSafeV2",
    [switch]$ForceUpdate
)

# Función para actualizar el archivo de estado
function Update-StateTracking {
    param(
        [string]$Status,
        [array]$Changes
    )
    
    $date = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
    $stateFile = Join-Path $WorkspacePath "docs\state-tracking.md"
    
    # Actualizar estado
    $content = Get-Content $stateFile
    $newContent = @()
    $updated = $false
    
    foreach ($line in $content) {
        if ($line -match "^Última actualización:") {
            $newContent += "Última actualización: $date"
            $updated = $true
        }
        else {
            $newContent += $line
        }
    }
    
    if (-not $updated) {
        $newContent = @("Última actualización: $date") + $newContent
    }
    
    # Agregar cambios nuevos
    if ($Changes) {
        $newContent += "`n### Cambios ($date)"
        foreach ($change in $Changes) {
            $newContent += "- $change"
        }
    }
    
    $newContent | Set-Content $stateFile
}

# Función para compilar el proyecto
function Build-Solution {
    $buildLog = Join-Path $WorkspacePath "build-log.txt"
    $buildResult = dotnet build $WorkspacePath 2>&1
    $buildResult | Out-File $buildLog
    
    # Analizar errores
    $errors = $buildResult | Where-Object { $_ -match "error CS\d+:" }
    $warnings = $buildResult | Where-Object { $_ -match "warning CS\d+:" }
    
    return @{
        Success = ($LASTEXITCODE -eq 0)
        Errors = $errors.Count
        Warnings = $warnings.Count
        Log = $buildLog
    }
}

# Función para actualizar el registro de errores
function Update-ErrorLog {
    param(
        [array]$Errors,
        [array]$Warnings
    )
    
    $errorLog = Join-Path $WorkspacePath "docs\error-solutions-log.md"
    $date = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
    
    $content = @"
### Compilación ($date)
#### Errores ($($Errors.Count))
$($Errors -join "`n")

#### Advertencias ($($Warnings.Count))
$($Warnings -join "`n")
"@
    
    Add-Content $errorLog $content
}

# Función principal
function Update-Project {
    # 1. Compilar solución
    Write-Host "Compilando solución..."
    $buildResult = Build-Solution
    
    # 2. Actualizar estado
    $status = if ($buildResult.Success) { "✓ Compilación exitosa" } else { "⚠ Errores de compilación" }
    $changes = @(
        "Compilación: $($buildResult.Errors) errores, $($buildResult.Warnings) advertencias"
    )
    
    Write-Host "Actualizando estado..."
    Update-StateTracking -Status $status -Changes $changes
    
    # 3. Actualizar registro de errores si hay problemas
    if ($buildResult.Errors -gt 0 -or $buildResult.Warnings -gt 0) {
        Write-Host "Registrando errores..."
        Update-ErrorLog -Errors $buildResult.Errors -Warnings $buildResult.Warnings
    }
    
    # 4. Mostrar resumen
    Write-Host "`nResumen:"
    Write-Host "- Estado: $status"
    Write-Host "- Errores: $($buildResult.Errors)"
    Write-Host "- Advertencias: $($buildResult.Warnings)"
    Write-Host "- Log: $($buildResult.Log)"
}

# Ejecutar actualización
try {
    Update-Project
}
catch {
    Write-Error "Error al actualizar el proyecto: $_"
    exit 1
} 