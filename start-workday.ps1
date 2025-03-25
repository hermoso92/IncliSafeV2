# Script de Inicio de Jornada - IncliSafeV2
Write-Host "Iniciando jornada de trabajo en IncliSafeV2..." -ForegroundColor Green

# 1. Actualizar el repositorio
Write-Host "Actualizando repositorio..." -ForegroundColor Yellow
git pull origin main

# 2. Restaurar paquetes
Write-Host "Restaurando paquetes NuGet..." -ForegroundColor Yellow
dotnet restore

# 3. Compilar la solución
Write-Host "Compilando solución..." -ForegroundColor Yellow
dotnet build

Write-Host "Inicio de jornada completado" -ForegroundColor Green 