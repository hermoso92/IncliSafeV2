# Script para gestionar el flujo de trabajo diario
param(
    [string]$WorkspacePath = "C:\Users\Hermoso\Desktop\IncliSafeV2",
    [string]$FeatureType = "",
    [string]$FeatureDescription = ""
)

# Importar configuración
$configPath = Join-Path $WorkspacePath "config\project-settings.json"
$config = Get-Content $configPath | ConvertFrom-Json

function Start-Day {
    try {
        Write-Host "=== Iniciando jornada de desarrollo ==="
        
        # 1. Actualizar código
        Write-Host "`n1. Actualizando código..."
        git pull origin develop
        
        # 2. Actualizar estado del proyecto
        Write-Host "`n2. Actualizando estado del proyecto..."
        & "$WorkspacePath\scripts\Update-ProjectState.ps1"
        
        # 3. Mostrar estado actual
        Write-Host "`n3. Estado actual del proyecto:"
        Get-Content (Join-Path $WorkspacePath "docs\state-tracking.md") | Select-Object -First 20
        
        # 4. Crear rama si se especifica
        if ($FeatureType -and $FeatureDescription) {
            $branchName = "feature/$FeatureType/$FeatureDescription"
            Write-Host "`n4. Creando rama $branchName..."
            git checkout -b $branchName
        }
        
        Write-Host "`n=== Jornada iniciada correctamente ==="
    }
    catch {
        Write-Error "Error al iniciar la jornada: $_"
        exit 1
    }
}

function Save-Progress {
    param(
        [string]$CommitMessage
    )
    
    try {
        Write-Host "=== Guardando progreso ==="
        
        # 1. Ejecutar pruebas
        Write-Host "`n1. Ejecutando pruebas..."
        & "$WorkspacePath\scripts\Run-Tests.ps1" -Coverage
        
        # 2. Actualizar documentación
        Write-Host "`n2. Actualizando documentación..."
        & "$WorkspacePath\scripts\Update-Documentation.ps1"
        
        # 3. Actualizar estado
        Write-Host "`n3. Actualizando estado..."
        & "$WorkspacePath\scripts\Update-ProjectState.ps1"
        
        # 4. Commit si hay mensaje
        if ($CommitMessage) {
            Write-Host "`n4. Realizando commit..."
            git add .
            git commit -m $CommitMessage
        }
        
        Write-Host "`n=== Progreso guardado correctamente ==="
    }
    catch {
        Write-Error "Error al guardar progreso: $_"
        exit 1
    }
}

function End-Day {
    param(
        [string]$CommitMessage = "chore(daily): end of day commit"
    )
    
    try {
        Write-Host "=== Finalizando jornada ==="
        
        # 1. Ejecutar pruebas completas
        Write-Host "`n1. Ejecutando suite completa de pruebas..."
        & "$WorkspacePath\scripts\Run-Tests.ps1" -Coverage
        
        # 2. Actualizar documentación
        Write-Host "`n2. Actualizando documentación final..."
        & "$WorkspacePath\scripts\Update-Documentation.ps1"
        
        # 3. Commit final
        Write-Host "`n3. Realizando commit final..."
        git add .
        git commit -m $CommitMessage
        
        # 4. Push
        Write-Host "`n4. Push a repositorio remoto..."
        $branch = git branch --show-current
        git push origin $branch
        
        Write-Host "`n=== Jornada finalizada correctamente ==="
        
        # 5. Mostrar resumen
        Write-Host "`nResumen del día:"
        Write-Host "- Branch: $branch"
        Write-Host "- Último commit: $CommitMessage"
        Write-Host "- Pruebas: Ejecutadas"
        Write-Host "- Documentación: Actualizada"
    }
    catch {
        Write-Error "Error al finalizar la jornada: $_"
        exit 1
    }
}

# Exportar funciones
Export-ModuleMember -Function Start-Day, Save-Progress, End-Day 