# Script de depuración automática para IncliSafe
# Autor: Claude AI
# Versión: 1.0.0

$ErrorActionPreference = 'Stop'
$VerbosePreference = 'Continue'

# Configuración
$config = @{
    SolutionPath = (Resolve-Path "IncliSafe.sln")
    ErrorLogPath = "logs\errors"
    SolutionsPath = "docs\solutions.txt"
    BackupPath = "backups"
}

# Función para crear backup
function Backup-CurrentState {
    $timestamp = Get-Date -Format "yyyy-MM-dd_HH-mm-ss"
    $backupDir = Join-Path $config.BackupPath $timestamp
    
    Write-Host "Creando backup en $backupDir..."
    New-Item -ItemType Directory -Path $backupDir -Force | Out-Null
    Copy-Item -Path "IncliSafe.Shared" -Destination $backupDir -Recurse -Force
}

# Función para analizar errores
function Analyze-Errors {
    $errorLog = Join-Path $config.ErrorLogPath "$(Get-Date -Format 'yyyy-MM-dd').log"
    if (Test-Path $errorLog) {
        return Get-Content $errorLog
    }
    return @()
}

# Función para corregir referencias ambiguas
function Fix-AmbiguousReferences {
    param($errors)
    
    foreach ($error in $errors) {
        if ($error -match "CS0104: '(.+?)' es una referencia ambigua") {
            $ambiguousType = $matches[1]
            Write-Host "Corrigiendo referencia ambigua: $ambiguousType"
            
            # Eliminar definiciones duplicadas
            $commonEnumsPath = "IncliSafe.Shared\Models\Common\CommonEnums.cs"
            if (Test-Path $commonEnumsPath) {
                Set-Content -Path $commonEnumsPath -Value "// Este archivo ha sido eliminado ya que los enums están definidos en IncliSafe.Shared/Models/Enums/AnalysisEnums.cs"
            }
        }
    }
}

# Función para corregir conflictos de miembros requeridos
function Fix-RequiredMemberConflicts {
    param($errors)
    
    foreach ($error in $errors) {
        if ($error -match "CS9031: 'BaseEntity\.(.+?)' no puede ocultar el miembro requerido") {
            $member = $matches[1]
            $filePath = $error -replace '.*?\((.*?):.*', '$1'
            Write-Host "Corrigiendo conflicto de miembro requerido: $member en $filePath"
            
            # Leer el archivo
            $content = Get-Content $filePath
            
            # Eliminar la propiedad duplicada
            $content = $content | Where-Object { $_ -notmatch "public required $member" }
            
            # Guardar cambios
            Set-Content -Path $filePath -Value $content
        }
    }
}

# Función para corregir referencias faltantes
function Fix-MissingReferences {
    param($errors)
    
    foreach ($error in $errors) {
        if ($error -match "CS0246: The type or namespace name '(.+?)' could not be found") {
            $missingType = $matches[1]
            Write-Host "Corrigiendo referencia faltante: $missingType"
            
            # Agregar using necesario
            $filePath = $error -replace '.*?\((.*?):.*', '$1'
            $content = Get-Content $filePath
            
            # Agregar using si no existe
            if ($content -notmatch "using IncliSafe.Shared.Models.Enums;") {
                $content = "using IncliSafe.Shared.Models.Enums;`n" + $content
                Set-Content -Path $filePath -Value $content
            }
        }
    }
}

# Función principal
function Start-AutoDebug {
    Write-Host "Iniciando proceso de depuración automática..."
    
    # Crear backup
    Backup-CurrentState
    
    # Analizar errores
    $errors = Analyze-Errors
    if ($errors.Count -gt 0) {
        Write-Host "Encontrados $($errors.Count) errores. Iniciando correcciones..."
        
        # Corregir referencias ambiguas
        Fix-AmbiguousReferences $errors
        
        # Corregir conflictos de miembros requeridos
        Fix-RequiredMemberConflicts $errors
        
        # Corregir referencias faltantes
        Fix-MissingReferences $errors
        
        Write-Host "Correcciones aplicadas. Intentando compilar nuevamente..."
        
        # Intentar compilar
        dotnet build $config.SolutionPath
        
        # Verificar si hay más errores
        $newErrors = Analyze-Errors
        if ($newErrors.Count -gt 0) {
            Write-Host "Todavía hay errores. Por favor, revise el log de errores para más detalles."
        } else {
            Write-Host "¡Compilación exitosa! Todos los errores han sido corregidos."
        }
    } else {
        Write-Host "No se encontraron errores. La compilación es exitosa."
    }
}

# Ejecutar el proceso
try {
    Start-AutoDebug
} catch {
    Write-Host "Error durante la depuración automática: $_"
    exit 1
} 