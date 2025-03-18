# Auto-Commit.ps1
# Script para realizar commits automáticos cuando hay cambios

$projectPath = Split-Path -Parent (Split-Path -Parent $PSCommandPath)
Set-Location $projectPath

function Get-PendingChanges {
    $status = git status --porcelain
    return $status.Length -gt 0
}

function Get-ChangesSummary {
    $added = (git diff --cached --numstat | Measure-Object -Property 1 -Sum).Sum
    $deleted = (git diff --cached --numstat | Measure-Object -Property 2 -Sum).Sum
    $files = (git status --porcelain | Measure-Object).Count
    return "Archivos modificados: $files | Líneas añadidas: $added | Líneas eliminadas: $deleted"
}

function New-CommitMessage {
    $date = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
    $changes = Get-ChangesSummary
    return "Auto-commit: $date`n`n$changes"
}

function Invoke-AutoCommit {
    try {
        # Verificar si hay cambios pendientes
        if (Get-PendingChanges) {
            Write-Host "Detectados cambios. Preparando commit..."
            
            # Stage todos los cambios
            git add .
            
            # Crear mensaje de commit
            $message = New-CommitMessage
            
            # Realizar commit
            git commit -m $message
            
            # Push al repositorio remoto
            git push origin main
            
            Write-Host "Commit y push realizados exitosamente"
            Write-Host "Mensaje: $message"
        }
        else {
            Write-Host "No hay cambios pendientes para commit"
        }
    }
    catch {
        Write-Error "Error durante el auto-commit: $_"
    }
}

# Bucle principal
Write-Host "Iniciando monitoreo de cambios..."
while ($true) {
    Invoke-AutoCommit
    # Esperar 5 minutos antes del siguiente check
    Start-Sleep -Seconds 300
} 