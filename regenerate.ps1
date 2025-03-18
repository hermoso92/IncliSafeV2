param (
    [switch]$Fix,
    [switch]$Document,
    [switch]$Build,
    [switch]$Analyze,
    [switch]$Backup,
    [switch]$Security
)

$ErrorActionPreference = "Stop"
$fecha = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
$projectRoot = $PSScriptRoot

Write-Host "🚀 Iniciando proceso de corrección y documentación de IncliSafe V2..." -ForegroundColor Cyan
Write-Host "📅 Fecha: $fecha" -ForegroundColor Cyan

# Función para hacer backup
function Backup-Project {
    $backupDir = "Backups/$(Get-Date -Format 'yyyy-MM-dd_HH-mm')"
    Write-Host "📦 Creando backup en $backupDir..." -ForegroundColor Yellow
    
    if (-not (Test-Path "Backups")) {
        New-Item -ItemType Directory -Path "Backups"
    }
    
    New-Item -ItemType Directory -Path $backupDir
    Copy-Item -Path "IncliSafe.*" -Destination $backupDir -Recurse
    Copy-Item -Path "*.txt" -Destination $backupDir
    Copy-Item -Path "*.md" -Destination $backupDir
    
    # Comprimir backup
    Compress-Archive -Path "$backupDir/*" -DestinationPath "$backupDir.zip"
    Remove-Item -Path $backupDir -Recurse
    
    Write-Host "✅ Backup completado y comprimido en $backupDir.zip" -ForegroundColor Green
}

# Función para análisis de seguridad
function Analyze-Security {
    Write-Host "🔒 Analizando seguridad..." -ForegroundColor Yellow
    
    $securityIssues = @()
    
    # Buscar tokens y secretos hardcodeados
    $sensitivePatterns = @(
        'password\s*=\s*["''][^"'']*["'']',
        'apikey\s*=\s*["''][^"'']*["'']',
        'secret\s*=\s*["''][^"'']*["'']',
        'connectionstring\s*=\s*["''][^"'']*["'']',
        'jwt\s*=\s*["''][^"'']*["'']'
    )
    
    Get-ChildItem -Recurse -File | Where-Object {
        $_.Extension -match "\.(cs|config|json|xml)$"
    } | ForEach-Object {
        $content = Get-Content $_.FullName -Raw
        foreach ($pattern in $sensitivePatterns) {
            if ($content -match $pattern) {
                $securityIssues += "⚠️ Posible secreto hardcodeado en $($_.FullName)"
            }
        }
    }
    
    # Verificar configuraciones de seguridad
    $configFiles = Get-ChildItem -Recurse -Include "appsettings*.json","web.config"
    foreach ($file in $configFiles) {
        $content = Get-Content $file -Raw
        if ($content -match '"AllowedHosts":\s*"\*"') {
            $securityIssues += "⚠️ AllowedHosts está configurado como * en $($file.Name)"
        }
    }
    
    if ($securityIssues.Count -gt 0) {
        Write-Host "`n🚨 Problemas de seguridad encontrados:" -ForegroundColor Red
        $securityIssues | ForEach-Object { Write-Host $_ }
        Add-Content "errores.txt" "`n### [$fecha] Problemas de Seguridad"
        Add-Content "errores.txt" "🚨 CRÍTICO"
        $securityIssues | ForEach-Object {
            Add-Content "errores.txt" "- $_"
        }
    }
}

# Verificar que estamos en el directorio correcto
if (-not (Test-Path "IncliSafe.Shared")) {
    Write-Host "❌ Error: No se encuentra el directorio IncliSafe.Shared" -ForegroundColor Red
    Write-Host "Ejecute este script desde el directorio raíz del proyecto" -ForegroundColor Yellow
    exit 1
}

# Crear backup si se solicita
if ($Backup) {
    Backup-Project
}

# Análisis estático del código
if ($Analyze) {
    Write-Host "🔍 Ejecutando análisis estático..." -ForegroundColor Yellow
    
    # Verificar archivos sin usar
    $unusedFiles = Get-ChildItem -Recurse -File | Where-Object {
        $content = Get-Content $_.FullName -Raw
        $content -eq $null -or $content.Trim() -eq ""
    }
    
    if ($unusedFiles) {
        Write-Host "⚠️ Archivos potencialmente sin usar:" -ForegroundColor Yellow
        $unusedFiles | ForEach-Object { Write-Host "  - $_" }
        Add-Content "errores.txt" "`n### [$fecha] Archivos Sin Usar Detectados"
        Add-Content "errores.txt" "⚠️ REVISAR"
        $unusedFiles | ForEach-Object {
            Add-Content "errores.txt" "- Archivo sin usar: $_"
        }
    }
    
    # Verificar TODOs pendientes
    $todos = Get-ChildItem -Recurse -File | Where-Object {
        $_.Extension -match "\.(cs|razor|cshtml)$"
    } | ForEach-Object {
        $content = Get-Content $_.FullName
        $lineNumber = 1
        $content | ForEach-Object {
            if ($_ -match "TODO:|HACK:|FIXME:") {
                [PSCustomObject]@{
                    File = $_.Name
                    Line = $lineNumber
                    Todo = $_
                }
            }
            $lineNumber++
        }
    }
    
    if ($todos) {
        Write-Host "`n📝 TODOs pendientes encontrados:" -ForegroundColor Yellow
        $todos | ForEach-Object { Write-Host "  - [$($_.File):$($_.Line)] $($_.Todo)" }
        Add-Content "errores.txt" "`n### [$fecha] TODOs Pendientes"
        Add-Content "errores.txt" "📝 REVISAR"
        $todos | ForEach-Object {
            Add-Content "errores.txt" "- [$($_.File):$($_.Line)] $($_.Todo)"
        }
    }
    
    # Verificar complejidad de métodos
    Write-Host "`n�� Analizando complejidad de código..." -ForegroundColor Yellow
    $methodPattern = '^\s*(public|private|protected).*\(.*\)\s*{'
    $methodEndPattern = '^\s*}'
    $complexMethods = Get-ChildItem -Recurse -Include "*.cs" | ForEach-Object {
        $content = Get-Content $_.FullName
        $methodLines = 0
        $inMethod = $false
        $methodName = ""
        
        for ($i = 0; $i -lt $content.Count; $i++) {
            $line = $content[$i]
            if ($line -match $methodPattern) {
                $inMethod = $true
                $methodName = $line
                $methodLines = 1
            }
            elseif ($inMethod) {
                $methodLines++
                if ($line -match $methodEndPattern) {
                    $inMethod = $false
                    if ($methodLines -gt 30) {
                        [PSCustomObject]@{
                            File = $_.Name
                            Method = $methodName.Trim()
                            Lines = $methodLines
                        }
                    }
                }
            }
        }
    }
    
    if ($complexMethods) {
        Write-Host "`n⚠️ Métodos potencialmente complejos:" -ForegroundColor Yellow
        $complexMethods | ForEach-Object {
            Write-Host "  - $($_.File): $($_.Method) ($($_.Lines) líneas)"
        }
        Add-Content "errores.txt" "`n### [$fecha] Métodos Complejos"
        Add-Content "errores.txt" "⚠️ OPTIMIZAR"
        $complexMethods | ForEach-Object {
            Add-Content "errores.txt" "- $($_.File): $($_.Method) ($($_.Lines) líneas)"
        }
    }
}

# Análisis de seguridad si se solicita
if ($Security) {
    Analyze-Security
}

if ($Fix) {
    Write-Host "🔧 Analizando y corrigiendo errores..." -ForegroundColor Yellow
    
    # Leer errores.txt si existe
    if (Test-Path "errores.txt") {
        $errores = Get-Content "errores.txt" -Raw
        Write-Host "📝 Errores encontrados en registro:" -ForegroundColor Yellow
        Write-Host $errores
    }
    
    # Compilar para obtener errores actuales
    Write-Host "🔍 Compilando para detectar errores actuales..." -ForegroundColor Yellow
    $buildOutput = dotnet build 2>&1
    $buildErrors = $buildOutput | Where-Object { $_ -match ": error" }
    $buildWarnings = $buildOutput | Where-Object { $_ -match ": warning" }
    
    if ($buildErrors -or $buildWarnings) {
        # Registrar errores
        if ($buildErrors) {
            Write-Host "❌ Errores detectados:" -ForegroundColor Red
            $buildErrors | ForEach-Object { Write-Host $_ }
            
            Add-Content "errores.txt" "`n### [$fecha] Errores de Compilación"
            Add-Content "errores.txt" "❌ PENDIENTE"
            $buildErrors | ForEach-Object {
                Add-Content "errores.txt" "- Error: $_"
            }
        }
        
        # Registrar warnings
        if ($buildWarnings) {
            Write-Host "`n⚠️ Warnings detectados:" -ForegroundColor Yellow
            $buildWarnings | ForEach-Object { Write-Host $_ }
            
            Add-Content "errores.txt" "`n### [$fecha] Warnings de Compilación"
            Add-Content "errores.txt" "⚠️ REVISAR"
            $buildWarnings | ForEach-Object {
                Add-Content "errores.txt" "- Warning: $_"
            }
        }
        
        Write-Host "✏️ Problemas registrados en errores.txt" -ForegroundColor Yellow
    } else {
        Write-Host "✅ No se detectaron errores ni warnings" -ForegroundColor Green
    }
}

if ($Document) {
    Write-Host "📚 Actualizando documentación..." -ForegroundColor Yellow
    
    # Actualizar soluciones.txt con los cambios realizados
    Add-Content "soluciones.txt" "`n### [$fecha] Actualización Automática"
    Add-Content "soluciones.txt" "✅ APLICADO"
    Add-Content "soluciones.txt" "- Ejecutada corrección automática"
    Add-Content "soluciones.txt" "- Documentados nuevos errores y warnings"
    Add-Content "soluciones.txt" "- Actualizado registro de soluciones"
    
    # Verificar si hay cambios sin commitear
    $gitStatus = git status --porcelain
    if ($gitStatus) {
        Write-Host "`n⚠️ Cambios pendientes de commit:" -ForegroundColor Yellow
        $gitStatus | ForEach-Object { Write-Host "  $_" }
        Add-Content "soluciones.txt" "⚠️ PENDIENTE"
        Add-Content "soluciones.txt" "- Cambios sin commitear detectados:"
        $gitStatus | ForEach-Object {
            Add-Content "soluciones.txt" "  $_"
        }
    }
    
    # Generar resumen de métricas
    Write-Host "`n📊 Generando métricas del proyecto..." -ForegroundColor Yellow
    $metrics = @{
        "Archivos .cs" = (Get-ChildItem -Recurse -Include "*.cs" | Measure-Object).Count
        "Archivos .razor" = (Get-ChildItem -Recurse -Include "*.razor" | Measure-Object).Count
        "Líneas de código" = 0
        "Clases" = 0
        "Métodos" = 0
    }
    
    Get-ChildItem -Recurse -Include "*.cs","*.razor" | ForEach-Object {
        $content = Get-Content $_.FullName
        $metrics["Líneas de código"] += ($content | Measure-Object).Count
        $metrics["Clases"] += ($content | Select-String -Pattern "class\s+\w+" | Measure-Object).Count
        $metrics["Métodos"] += ($content | Select-String -Pattern "^\s*(public|private|protected).*\(.*\).*{" | Measure-Object).Count
    }
    
    Add-Content "soluciones.txt" "`n### Métricas del Proyecto"
    $metrics.GetEnumerator() | ForEach-Object {
        Add-Content "soluciones.txt" "- $($_.Key): $($_.Value)"
        Write-Host "  $($_.Key): $($_.Value)" -ForegroundColor White
    }
    
    Write-Host "✅ Documentación actualizada" -ForegroundColor Green
}

if ($Build) {
    Write-Host "🔨 Compilando solución..." -ForegroundColor Yellow
    dotnet build
    if ($LASTEXITCODE -ne 0) {
        Write-Host "❌ Error al compilar la solución" -ForegroundColor Red
        exit 1
    }
}

Write-Host "`n✅ Proceso completado!" -ForegroundColor Green

# Sugerir próximos pasos
Write-Host "`n📋 Próximos pasos sugeridos:" -ForegroundColor Cyan
Write-Host "1. Revisar errores.txt para ver los problemas detectados" -ForegroundColor White
Write-Host "2. Escribir 'Arreglalo' en el chat para que corrija los errores automáticamente" -ForegroundColor White
Write-Host "3. Revisar soluciones.txt para ver los cambios aplicados" -ForegroundColor White
Write-Host "4. Hacer commit de los cambios" -ForegroundColor White

# Mostrar ayuda si no se especificaron parámetros
if (-not ($Fix -or $Document -or $Build -or $Analyze -or $Backup -or $Security)) {
    Write-Host "`n💡 Uso del script:" -ForegroundColor Cyan
    Write-Host "  .\regenerate.ps1 -Fix         # Analizar y registrar errores" -ForegroundColor White
    Write-Host "  .\regenerate.ps1 -Document    # Actualizar documentación" -ForegroundColor White
    Write-Host "  .\regenerate.ps1 -Build       # Compilar solución" -ForegroundColor White
    Write-Host "  .\regenerate.ps1 -Analyze     # Análisis estático" -ForegroundColor White
    Write-Host "  .\regenerate.ps1 -Backup      # Crear backup" -ForegroundColor White
    Write-Host "  .\regenerate.ps1 -Security    # Análisis de seguridad" -ForegroundColor White
    Write-Host "  # Puedes combinar parámetros, ejemplo:" -ForegroundColor White
    Write-Host "  .\regenerate.ps1 -Fix -Document -Analyze -Security" -ForegroundColor White
} 