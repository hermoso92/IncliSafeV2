# Script de Fin de Jornada - IncliSafeV2
Write-Host "🏁 Finalizando jornada de trabajo en IncliSafeV2..." -ForegroundColor Green

# 1. Verificar cambios no commiteados
Write-Host "🔍 Verificando cambios pendientes..." -ForegroundColor Yellow
$changes = git status
if ($changes -match "nothing to commit") {
    Write-Host "✅ No hay cambios pendientes" -ForegroundColor Green
} else {
    Write-Host "⚠️ Hay cambios pendientes:" -ForegroundColor Red
    Write-Host $changes
}

# 2. Compilar la solución
Write-Host "🔨 Compilando solución..." -ForegroundColor Yellow
dotnet build

# 3. Actualizar historial de soluciones
Write-Host "📝 Actualizando historial de soluciones..." -ForegroundColor Yellow
$date = Get-Date -Format "yyyy-MM-dd"
$commitMessage = git log -1 --pretty=%B
Add-Content -Path "soluciones.txt" -Value "`n## $date`n$commitMessage"

# 4. Actualizar registro de errores
Write-Host "⚠️ Actualizando registro de errores..." -ForegroundColor Yellow
$buildOutput = dotnet build
if ($buildOutput -match "error") {
    Add-Content -Path "errores.txt" -Value "`n## $date`n$buildOutput"
}

# 5. Hacer commit de los cambios
Write-Host "💾 Guardando cambios..." -ForegroundColor Yellow
git add .
git commit -m "chore: Actualización diaria - $date"

# 6. Subir cambios al repositorio
Write-Host "📤 Subiendo cambios al repositorio..." -ForegroundColor Yellow
git push origin main

# 7. Generar resumen de la jornada
Write-Host "📊 Generando resumen de la jornada..." -ForegroundColor Yellow
$summary = @"
# Resumen de Jornada - $date

## Cambios Realizados
$(git log --since="24 hours ago" --pretty=format:"- %s")

## Errores Pendientes
$(Get-Content errores.txt | Select-Object -Last 10)

## Próximos Pasos
1. Revisar errores pendientes
2. Implementar soluciones propuestas
3. Continuar con las tareas en curso
"@

# Guardar resumen
$summary | Out-File -FilePath "daily-summary-$date.md"

Write-Host "✅ Fin de jornada completado" -ForegroundColor Green
Write-Host "📝 Resumen guardado en daily-summary-$date.md" 