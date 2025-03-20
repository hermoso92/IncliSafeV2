# Script de respaldo y documentación diaria para IncliSafe
# Este script debe ejecutarse al final de cada jornada

# Configuración de rutas
$workspaceRoot = "C:\Users\Cosigein SL\Documents\GitHub\IncliSafeV2"
$backupFolder = Join-Path $workspaceRoot "backups"
$date = Get-Date -Format "yyyy-MM-dd"
$backupPath = Join-Path $backupFolder $date

# Crear directorio de respaldo si no existe
if (-not (Test-Path $backupFolder)) {
    New-Item -ItemType Directory -Path $backupFolder | Out-Null
}

# Crear directorio para el día actual
if (-not (Test-Path $backupPath)) {
    New-Item -ItemType Directory -Path $backupPath | Out-Null
}

# 1. Guardar estado actual del repositorio
Write-Host "Guardando estado del repositorio..." -ForegroundColor Green
git add .
git commit -m "Finalización de jornada $date - Guardado automático"
git push

# 2. Crear respaldo de archivos críticos
Write-Host "Creando respaldo de archivos críticos..." -ForegroundColor Green
$criticalFiles = @(
    "IncliSafe.Shared/Models/Analysis/Core/Analysis.Core.cs",
    "IncliSafe.Shared/Models/Analysis/Extensions/AnalysisExtensions.cs",
    "IncliSafe.Shared/Models/DTOs/Analysis.DTOs.cs",
    "IncliSafe.Shared/Models/Entities/Analysis.cs",
    "IncliSafe.Shared/Models/Enums/AnalysisEnums.cs"
)

foreach ($file in $criticalFiles) {
    $sourcePath = Join-Path $workspaceRoot $file
    if (Test-Path $sourcePath) {
        $targetPath = Join-Path $backupPath (Split-Path $file -Leaf)
        Copy-Item $sourcePath $targetPath
        Write-Host "Respaldo creado: $file" -ForegroundColor Yellow
    }
}

# 3. Generar reporte de estado
$reportPath = Join-Path $backupPath "daily-report.md"
$reportContent = @"
# Reporte Diario - $date

## Estado del Proyecto
- Último commit: $(git log -1 --pretty=format:"%h - %s")
- Rama actual: $(git branch --show-current)

## Archivos Modificados
$(git status --porcelain)

## Errores Pendientes
$(if (Test-Path "errores.txt") { Get-Content "errores.txt" } else { "No hay errores pendientes" })

## Soluciones Aplicadas
$(if (Test-Path "soluciones.txt") { Get-Content "soluciones.txt" } else { "No hay soluciones registradas" })

## Tareas Pendientes
1. Revisar y corregir errores de compilación en AnalysisExtensions.cs
2. Completar la implementación de los DTOs faltantes
3. Implementar la lógica de cálculo de scores en AnalysisResult
4. Revisar y optimizar el mapeo entre entidades y DTOs

## Notas Adicionales
- Se requiere revisar la consistencia entre los modelos Core y DTOs
- Pendiente implementar validaciones en los DTOs
- Considerar agregar más documentación en los métodos de extensión
"@

$reportContent | Out-File -FilePath $reportPath -Encoding UTF8

# 4. Crear archivo de estado para la siguiente sesión
$nextSessionPath = Join-Path $backupPath "next-session.md"
$nextSessionContent = @"
# Estado para la Siguiente Sesión

## Prioridades
1. Corregir errores de compilación en AnalysisExtensions.cs
2. Completar la implementación de los DTOs faltantes
3. Implementar la lógica de cálculo de scores
4. Optimizar el mapeo entre entidades y DTOs

## Archivos Críticos Modificados
$(git diff --name-only HEAD~1 HEAD)

## Errores Pendientes
$(if (Test-Path "errores.txt") { Get-Content "errores.txt" } else { "No hay errores pendientes" })

## Notas para Continuar
- Revisar el reporte diario en: $reportPath
- Verificar los cambios en los archivos críticos
- Comenzar por la corrección de errores de compilación
"@

$nextSessionContent | Out-File -FilePath $nextSessionPath -Encoding UTF8

# 5. Crear archivo de resumen ejecutivo
$summaryPath = Join-Path $backupPath "summary.md"
$summaryContent = @"
# Resumen Ejecutivo - $date

## Progreso del Día
- Se implementaron las extensiones para mapeo entre entidades y DTOs
- Se identificaron y documentaron errores de compilación
- Se estableció la estructura base para el manejo de análisis

## Desafíos Encontrados
1. Inconsistencias entre modelos Core y DTOs
2. Errores de compilación en AnalysisExtensions.cs
3. Falta de implementación en algunos DTOs

## Próximos Pasos
1. Corregir errores de compilación
2. Completar implementación de DTOs
3. Implementar lógica de cálculo de scores
4. Optimizar mapeo entre entidades y DTOs

## Archivos de Referencia
- Reporte diario: $reportPath
- Estado para siguiente sesión: $nextSessionPath
- Respaldo de archivos críticos: $backupPath
"@

$summaryContent | Out-File -FilePath $summaryPath -Encoding UTF8

Write-Host "`nProceso de respaldo completado exitosamente." -ForegroundColor Green
Write-Host "Ubicación de los archivos de respaldo: $backupPath" -ForegroundColor Yellow
Write-Host "`nArchivos generados:" -ForegroundColor Cyan
Write-Host "- Reporte diario: $reportPath"
Write-Host "- Estado para siguiente sesión: $nextSessionPath"
Write-Host "- Resumen ejecutivo: $summaryPath" 