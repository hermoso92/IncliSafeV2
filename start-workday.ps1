# Script de Inicio de Jornada - IncliSafeV2
Write-Host "🚀 Iniciando jornada de trabajo en IncliSafeV2..." -ForegroundColor Green

# 1. Actualizar el repositorio
Write-Host "📥 Actualizando repositorio..." -ForegroundColor Yellow
git pull origin main

# 2. Restaurar paquetes
Write-Host "📦 Restaurando paquetes NuGet..." -ForegroundColor Yellow
dotnet restore

# 3. Compilar la solución
Write-Host "🔨 Compilando solución..." -ForegroundColor Yellow
dotnet build

# 4. Verificar errores
Write-Host "🔍 Verificando errores..." -ForegroundColor Yellow
$errors = Get-Content errores.txt
if ($errors) {
    Write-Host "⚠️ Errores pendientes encontrados:" -ForegroundColor Red
    $errors | ForEach-Object { Write-Host $_ }
} else {
    Write-Host "✅ No hay errores pendientes" -ForegroundColor Green
}

# 5. Verificar soluciones
Write-Host "📋 Revisando soluciones implementadas..." -ForegroundColor Yellow
$solutions = Get-Content soluciones.txt
if ($solutions) {
    Write-Host "📝 Últimas soluciones implementadas:" -ForegroundColor Green
    $solutions | Select-Object -Last 10 | ForEach-Object { Write-Host $_ }
}

# 6. Verificar estado de la base de datos
Write-Host "🗄️ Verificando estado de la base de datos..." -ForegroundColor Yellow
# Aquí irían los comandos para verificar la base de datos

# 7. Iniciar servicios necesarios
Write-Host "🚀 Iniciando servicios..." -ForegroundColor Yellow
# Aquí irían los comandos para iniciar servicios necesarios

Write-Host "✅ Inicio de jornada completado" -ForegroundColor Green
Write-Host "📝 Próximos pasos:"
Write-Host "1. Revisar errores pendientes en errores.txt"
Write-Host "2. Continuar con las tareas pendientes"
Write-Host "3. Documentar nuevos cambios" 