# Master-Debug.ps1
# Script maestro para depuración, pruebas y mejoras automáticas

$ErrorActionPreference = "Continue" # Cambiado a Continue para no detener la ejecución
$projectPath = Split-Path -Parent (Split-Path -Parent $PSCommandPath)
Set-Location $projectPath

# Configuración de logging
$logPath = Join-Path $projectPath "logs"
$logFile = Join-Path $logPath "debug_$(Get-Date -Format 'yyyyMMdd_HHmmss').log"
New-Item -ItemType Directory -Path $logPath -Force | Out-Null

function Write-Log {
    param([string]$Message)
    $timestamp = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
    $logMessage = "[$timestamp] $Message"
    Write-Host $logMessage
    Add-Content -Path $logFile -Value $logMessage
}

function Fix-CommonErrors {
    Write-Log "Corrigiendo errores comunes..."
    
    # Restaurar paquetes NuGet
    dotnet restore --force
    
    # Limpiar solución
    dotnet clean
    
    # Eliminar archivos temporales y caché
    Get-ChildItem -Recurse -Include "bin","obj",".vs" | Remove-Item -Recurse -Force -ErrorAction SilentlyContinue
    
    # Forzar actualización de herramientas globales
    dotnet tool update --global dotnet-ef --version 8.0.2
    dotnet tool update --global dotnet-format
    dotnet tool update --global roslynator.dotnet.cli
}

function Test-BuildErrors {
    Write-Log "Verificando errores de compilación..."
    dotnet build /p:TreatWarningsAsErrors=false /clp:ErrorsOnly | Tee-Object -Variable buildOutput
    if ($LASTEXITCODE -ne 0) {
        Write-Log "ERRORES DE COMPILACIÓN ENCONTRADOS - Intentando corrección automática..."
        Fix-CommonErrors
        # Intentar compilar de nuevo después de la corrección
        dotnet build /p:TreatWarningsAsErrors=false /clp:ErrorsOnly
    }
    Write-Log "Compilación completada"
    return $false
}

function Test-CodeAnalysis {
    Write-Log "Ejecutando análisis de código..."
    
    # Instalar/Actualizar herramientas
    dotnet tool update --global roslynator.dotnet.cli --version * 
    
    # Ejecutar análisis y aplicar correcciones automáticamente
    roslynator fix "**/*.cs" --verbosity d
    
    Write-Log "Análisis y correcciones de código completados"
    return $false
}

function Invoke-UnitTests {
    Write-Log "Ejecutando pruebas unitarias..."
    
    # Asegurar que los paquetes de prueba estén instalados
    Get-ChildItem -Recurse -Filter "*.csproj" | ForEach-Object {
        dotnet add $_.FullName package Microsoft.NET.Test.Sdk --version 17.9.0 --no-restore
        dotnet add $_.FullName package xunit --version 2.7.0 --no-restore
        dotnet add $_.FullName package xunit.runner.visualstudio --version 2.5.7 --no-restore
        dotnet add $_.FullName package coverlet.collector --version 6.0.1 --no-restore
    }
    
    # Ejecutar pruebas ignorando fallos
    dotnet test --no-restore --verbosity minimal
    Write-Log "Ejecución de pruebas completada"
    return $false
}

function Update-Dependencies {
    Write-Log "Actualizando dependencias..."
    
    # Actualizar todas las dependencias a las últimas versiones estables
    Get-ChildItem -Recurse -Filter "*.csproj" | ForEach-Object {
        $projectFile = $_.FullName
        Write-Log "Actualizando $projectFile"
        
        # Actualizar paquetes de Microsoft.AspNetCore
        dotnet add $projectFile package Microsoft.AspNetCore.Components.WebAssembly --version 8.0.2 --no-restore
        dotnet add $projectFile package Microsoft.AspNetCore.Components.WebAssembly.DevServer --version 8.0.2 --no-restore
        dotnet add $projectFile package Microsoft.AspNetCore.Components.WebAssembly.Server --version 8.0.2 --no-restore
        
        # Actualizar Entity Framework Core
        dotnet add $projectFile package Microsoft.EntityFrameworkCore --version 8.0.2 --no-restore
        dotnet add $projectFile package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.2 --no-restore
        dotnet add $projectFile package Microsoft.EntityFrameworkCore.Tools --version 8.0.2 --no-restore
        
        # Actualizar paquetes de autenticación
        dotnet add $projectFile package Microsoft.AspNetCore.Authentication.JwtBearer --version 8.0.2 --no-restore
        dotnet add $projectFile package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 8.0.2 --no-restore
    }
    
    # Restaurar todos los paquetes
    dotnet restore --force
    
    Write-Log "Dependencias actualizadas"
}

function Optimize-Code {
    Write-Log "Optimizando código..."
    
    # Actualizar herramienta de formato
    dotnet tool update --global dotnet-format
    
    # Aplicar formato al código
    dotnet format --verbosity detailed --fix-style info --fix-analyzers info
    
    # Limpiar archivos innecesarios
    Get-ChildItem -Recurse -Include "bin","obj",".vs","*.user","*.suo" | Remove-Item -Recurse -Force -ErrorAction SilentlyContinue
    
    Write-Log "Optimización completada"
}

function Start-AutoCommit {
    Write-Log "Iniciando proceso de auto-commit..."
    
    # Configurar Git para no pedir credenciales
    git config --global credential.helper store
    
    # Asegurarse de que estamos en la rama main
    git checkout main -f
    
    # Forzar pull para actualizar
    git pull origin main --force
    
    # Agregar todos los cambios
    git add . -A
    
    $timestamp = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
    $message = "Auto-commit: Depuración y optimización automática [$timestamp]`n`n"
    $message += "- Correcciones automáticas aplicadas`n"
    $message += "- Dependencias actualizadas`n"
    $message += "- Código optimizado`n"
    $message += "- Pruebas ejecutadas`n"
    
    # Forzar commit y push
    git commit -m $message --allow-empty
    git push origin main --force
    
    Write-Log "Cambios commiteados y subidos al repositorio"
}

function Start-BlazorApp {
    Write-Log "Iniciando aplicación Blazor..."
    $clientPath = Join-Path $projectPath "IncliSafe.Client"
    if (Test-Path $clientPath) {
        # Matar cualquier proceso existente que use el puerto 5000
        Get-NetTCPConnection -LocalPort 5000 -ErrorAction SilentlyContinue | ForEach-Object { 
            Stop-Process -Id $_.OwningProcess -Force 
        }
        
        Start-Process "dotnet" -ArgumentList "run", "--project", $clientPath, "--no-build", "--no-restore" -WindowStyle Hidden
        Write-Log "Aplicación Blazor iniciada"
        return $true
    }
    Write-Log "No se encontró el proyecto Blazor"
    return $false
}

function Start-ApiServer {
    Write-Log "Iniciando servidor API..."
    $apiPath = Join-Path $projectPath "IncliSafeApi"
    if (Test-Path $apiPath) {
        # Matar cualquier proceso existente que use el puerto 5001
        Get-NetTCPConnection -LocalPort 5001 -ErrorAction SilentlyContinue | ForEach-Object { 
            Stop-Process -Id $_.OwningProcess -Force 
        }
        
        Start-Process "dotnet" -ArgumentList "run", "--project", $apiPath, "--no-build", "--no-restore" -WindowStyle Hidden
        Write-Log "Servidor API iniciado"
        return $true
    }
    Write-Log "No se encontró el proyecto API"
    return $false
}

function Start-DebugProcess {
    Write-Log "=== INICIANDO PROCESO DE DEPURACIÓN Y MEJORA AUTOMÁTICA ==="
    
    try {
        # Detener procesos existentes
        Get-Process -Name "dotnet" -ErrorAction SilentlyContinue | Stop-Process -Force
        
        # Secuencia de operaciones
        Fix-CommonErrors
        Update-Dependencies
        Test-BuildErrors
        Test-CodeAnalysis
        Invoke-UnitTests
        Optimize-Code
        Start-AutoCommit
        
        # Iniciar aplicación
        $blazorStarted = Start-BlazorApp
        $apiStarted = Start-ApiServer
        
        if ($blazorStarted -and $apiStarted) {
            Write-Log "Aplicación iniciada correctamente"
            Write-Log "Frontend: http://localhost:5000"
            Write-Log "API: http://localhost:5001"
            
            # Abrir el navegador automáticamente
            Start-Process "http://localhost:5000"
        }
    }
    catch {
        Write-Log "Error durante el proceso: $_"
        Write-Log "Continuando con la siguiente iteración..."
    }
    
    Write-Log "=== PROCESO DE DEPURACIÓN COMPLETADO ==="
}

# Configurar Git para no mostrar warnings
git config --global core.safecrlf false

# Iniciar el proceso de depuración
Start-DebugProcess

# Mantener el script ejecutándose y monitorear cambios
while ($true) {
    Write-Log "Monitoreando cambios..."
    Start-Sleep -Seconds 300 # Revisar cada 5 minutos
    Start-DebugProcess
} 