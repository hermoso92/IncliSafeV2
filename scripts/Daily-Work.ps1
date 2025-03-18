# Script para gestionar el trabajo diario
param(
    [string]$WorkspacePath = "C:\Users\Hermoso\Desktop\IncliSafeV2",
    [ValidateSet('Start', 'Check', 'End')]
    [string]$Command = 'Check'
)

# Configurar codificación
$OutputEncoding = [System.Text.Encoding]::UTF8
[Console]::OutputEncoding = [System.Text.Encoding]::UTF8

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
    "$timestamp [$Type] $Message" | Out-File -FilePath $logFile -Append -Encoding UTF8
    Write-Host "[$Type] $Message"
}

# Función para configurar Git
function Initialize-Git {
    Write-Log "Configurando Git..." "GIT"
    
    # Configurar finales de línea
    git config --global core.autocrlf true
    
    # Verificar rama actual
    $currentBranch = git rev-parse --abbrev-ref HEAD
    
    # Si no estamos en main o develop, crear develop desde main
    if ($currentBranch -ne "develop") {
        Write-Log "Configurando rama develop..." "GIT"
        
        # Verificar si existe rama main
        $mainExists = git show-ref --verify --quiet refs/heads/main
        if ($LASTEXITCODE -eq 0) {
            # Crear develop desde main si no existe
            $developExists = git show-ref --verify --quiet refs/heads/develop
            if ($LASTEXITCODE -ne 0) {
                git checkout main
                git checkout -b develop
            } else {
                git checkout develop
            }
        } else {
            # Si no existe main, crear develop desde la rama actual
            $developExists = git show-ref --verify --quiet refs/heads/develop
            if ($LASTEXITCODE -ne 0) {
                git checkout -b develop
            } else {
                git checkout develop
            }
        }
    }
}

# Función para iniciar el día
function Start-Work {
    Write-Log "=== Iniciando día de trabajo ===" "START"
    
    try {
        # 1. Configurar Git
        Initialize-Git
        
        # 2. Pull del repositorio
        Write-Log "Actualizando código..." "GIT"
        $pullResult = git pull origin develop 2>&1
        if ($LASTEXITCODE -ne 0) {
            Write-Log "No se pudo hacer pull. Creando rama develop local..." "WARNING"
            git checkout -b develop
        }
        $pullResult | Out-File (Join-Path $logPath "git.log") -Encoding UTF8
        
        # 3. Compilación inicial
        Write-Log "Compilando proyecto..." "BUILD"
        $buildResult = dotnet build 2>&1
        $buildResult | Out-File (Join-Path $logPath "build.log") -Encoding UTF8
        
        # 4. Registrar errores iniciales
        $errors = $buildResult | Where-Object { $_ -match "error CS\d+:" }
        if ($errors) {
            Write-Log "Se encontraron $($errors.Count) errores" "ERROR"
            $errors | Out-File (Join-Path $logPath "errors.log") -Encoding UTF8
            
            # Crear archivo de errores para seguimiento
            $errorSummary = @"
# Errores Pendientes ($date)

## Errores de Compilación
$($errors | ForEach-Object { "- $_" } | Out-String)

## Plan de Acción
1. Revisar cada error
2. Priorizar correcciones
3. Verificar dependencias
"@
            $errorSummary | Out-File (Join-Path $WorkspacePath "docs\pending-errors.md") -Encoding UTF8
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
        $buildResult | Out-File (Join-Path $logPath "build-check.log") -Encoding UTF8
        
        # 2. Verificar errores
        $errors = $buildResult | Where-Object { $_ -match "error CS\d+:" }
        if ($errors) {
            Write-Log "Se encontraron $($errors.Count) errores" "ERROR"
            $errors | Out-File (Join-Path $logPath "errors-check.log") -Encoding UTF8
            
            # Actualizar archivo de errores pendientes
            $errorSummary = Get-Content (Join-Path $WorkspacePath "docs\pending-errors.md") -Encoding UTF8
            $newErrors = $errors | Where-Object { $errorSummary -notcontains $_ }
            if ($newErrors) {
                $errorSummary += "`n## Nuevos Errores ($date)`n"
                $errorSummary += $newErrors | ForEach-Object { "- $_" } | Out-String
                $errorSummary | Out-File (Join-Path $WorkspacePath "docs\pending-errors.md") -Encoding UTF8
            }
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
        $buildResult | Out-File (Join-Path $logPath "build-final.log") -Encoding UTF8
        
        # 2. Verificar errores
        $errors = $buildResult | Where-Object { $_ -match "error CS\d+:" }
        $hasErrors = $errors.Count -gt 0
        
        if ($hasErrors) {
            Write-Log "Se encontraron $($errors.Count) errores" "ERROR"
            $errors | Out-File (Join-Path $logPath "errors-final.log") -Encoding UTF8
        }
        
        # 3. Crear resumen de cambios
        $changes = @()
        if ($hasErrors) {
            $changes += "- Errores pendientes: $($errors.Count)"
            
            # Agrupar errores por tipo
            $errorTypes = $errors | ForEach-Object {
                if ($_ -match "error CS(\d+):") {
                    $matches[1]
                }
            } | Group-Object
            
            $changes += "- Tipos de errores:"
            foreach ($type in $errorTypes) {
                $changes += "  - CS$($type.Name): $($type.Count) ocurrencias"
            }
        }
        
        # Agregar archivos modificados
        $modifiedFiles = git status --porcelain | Where-Object { $_ -match '^[AM]' }
        if ($modifiedFiles) {
            $changes += "- Archivos modificados:"
            $changes += $modifiedFiles | ForEach-Object { "  - $_" }
        }
        
        # 4. Commit diario
        Write-Log "Realizando commit diario..." "GIT"
        git add .
        
        $commitMessage = @"
daily($date): resumen del día

$($changes -join "`n")
"@
        
        git commit -m $commitMessage 2>&1 | Tee-Object -FilePath (Join-Path $logPath "git-commit.log")
        
        # 5. Push (verificando rama)
        Write-Log "Push al repositorio..." "GIT"
        $branch = git rev-parse --abbrev-ref HEAD
        $pushResult = git push origin $branch 2>&1
        if ($LASTEXITCODE -ne 0) {
            Write-Log "Error al hacer push. Creando rama remota..." "WARNING"
            git push -u origin $branch
        }
        $pushResult | Out-File (Join-Path $logPath "git-push.log") -Encoding UTF8
        
        Write-Log "Día de trabajo finalizado correctamente" "SUCCESS"
        
        # 6. Mostrar resumen
        Write-Log "=== Resumen del Día ===" "SUMMARY"
        Write-Log "- Fecha: $date" "SUMMARY"
        Write-Log "- Errores: $($errors.Count)" "SUMMARY"
        Write-Log "- Branch: $branch" "SUMMARY"
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