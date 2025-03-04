# Script para gestionar el trabajo diario
param(
    [string]$WorkspacePath = "C:\Users\Hermoso\Desktop\IncliSafeV2",
    [ValidateSet('Start', 'Check', 'End')]
    [string]$Command = 'Check'
)

# Crear directorio de logs si no existe
$date = Get-Date -Format "yyyy-MM-dd"
$logPath = Join-Path $WorkspacePath "logs\$date"
if (-not (Test-Path $logPath)) {
    New-Item -ItemType Directory -Path $logPath -Force | Out-Null
}

# Función para registrar en el log
function Write-Log {
    param(
        [string]$Message,
        [string]$Type = "INFO"
    )
    
    $timestamp = Get-Date -Format "HH:mm:ss"
    $logFile = Join-Path $logPath "daily.log"
    "$timestamp [$Type] $Message" | Add-Content $logFile
    Write-Host "[$Type] $Message"
}

# Función para iniciar el día
function Start-Work {
    Write-Log "=== Iniciando día de trabajo ===" "START"
    
    try {
        # 1. Pull del repositorio
        Write-Log "Actualizando código..." "GIT"
        git pull origin develop 2>&1 | Tee-Object -FilePath (Join-Path $logPath "git.log")
        
        # 2. Compilación inicial
        Write-Log "Compilando proyecto..." "BUILD"
        $buildResult = dotnet build 2>&1
        $buildResult | Out-File (Join-Path $logPath "build.log")
        
        # 3. Registrar errores iniciales
        $errors = $buildResult | Where-Object { $_ -match "error CS\d+:" }
        if ($errors) {
            Write-Log "Se encontraron $($errors.Count) errores" "ERROR"
            $errors | Out-File (Join-Path $logPath "errors.log")
        }
        
        Write-Log "Día de trabajo iniciado correctamente" "SUCCESS"
    }
    catch {
        Write-Log $_.Exception.Message "ERROR"
        exit 1
    }
}

# Función para verificar progreso
function Check-Progress {
    Write-Log "=== Verificando progreso ===" "CHECK"
    
    try {
        # 1. Compilación rápida
        Write-Log "Compilando proyecto..." "BUILD"
        $buildResult = dotnet build 2>&1
        $buildResult | Out-File (Join-Path $logPath "build-check.log")
        
        # 2. Verificar errores
        $errors = $buildResult | Where-Object { $_ -match "error CS\d+:" }
        if ($errors) {
            Write-Log "Se encontraron $($errors.Count) errores" "ERROR"
            $errors | Out-File (Join-Path $logPath "errors-check.log")
        }
        
        Write-Log "Verificación completada" "SUCCESS"
    }
    catch {
        Write-Log $_.Exception.Message "ERROR"
    }
}

# Función para finalizar el día
function End-Work {
    Write-Log "=== Finalizando día de trabajo ===" "END"
    
    try {
        # 1. Compilación final
        Write-Log "Compilación final..." "BUILD"
        $buildResult = dotnet build 2>&1
        $buildResult | Out-File (Join-Path $logPath "build-final.log")
        
        # 2. Verificar errores
        $errors = $buildResult | Where-Object { $_ -match "error CS\d+:" }
        $hasErrors = $errors.Count -gt 0
        
        if ($hasErrors) {
            Write-Log "Se encontraron $($errors.Count) errores" "ERROR"
            $errors | Out-File (Join-Path $logPath "errors-final.log")
        }
        
        # 3. Crear resumen de cambios
        $changes = @()
        if ($hasErrors) {
            $changes += "- Errores pendientes: $($errors.Count)"
        }
        
        # 4. Commit diario
        Write-Log "Realizando commit diario..." "GIT"
        git add .
        
        $commitMessage = @"
daily($date): resumen del día

$($changes -join "`n")
"@
        
        git commit -m $commitMessage 2>&1 | Tee-Object -FilePath (Join-Path $logPath "git-commit.log")
        
        # 5. Push
        Write-Log "Push al repositorio..." "GIT"
        git push origin develop 2>&1 | Tee-Object -FilePath (Join-Path $logPath "git-push.log")
        
        Write-Log "Día de trabajo finalizado correctamente" "SUCCESS"
        
        # 6. Mostrar resumen
        Write-Log "=== Resumen del Día ===" "SUMMARY"
        Write-Log "- Fecha: $date" "SUMMARY"
        Write-Log "- Errores: $($errors.Count)" "SUMMARY"
        Write-Log "- Commit: $commitMessage" "SUMMARY"
    }
    catch {
        Write-Log $_.Exception.Message "ERROR"
        exit 1
    }
}

# Ejecutar comando especificado
switch ($Command) {
    'Start' { Start-Work }
    'Check' { Check-Progress }
    'End' { End-Work }
} 