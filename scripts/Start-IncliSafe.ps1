# IncliSafe Master Control Script
# Author: Claude AI
# Version: 1.0.0

param(
    [switch]$SkipTests,
    [switch]$SkipBuild,
    [switch]$AutoFix
)

$ErrorActionPreference = 'Stop'
$VerbosePreference = 'Continue'
$root = Split-Path -Parent (Split-Path -Parent $MyInvocation.MyCommand.Path)

# Configuration
$config = @{
    WorkspacePath = $PSScriptRoot
    LogPath = "..\logs"
    ConfigPath = "..\config"
}

# Initialize logging
function Initialize-Logging {
    Write-Verbose "Initializing logging..."
    
    if (-not (Test-Path $config.LogPath)) {
        New-Item -ItemType Directory -Path $config.LogPath -Force
    }
    
    Start-Transcript -Path (Join-Path $config.LogPath "inclisafe-$(Get-Date -Format 'yyyy-MM-dd').log") -Append
}

function Write-Step {
    param($Message)
    Write-Host "`n=== $Message ===" -ForegroundColor Cyan
}

# Verificar herramientas necesarias
Write-Step "Verificando herramientas"
& "$PSScriptRoot\verify-tools.ps1"

# Actualizar dependencias
Write-Step "Actualizando dependencias"
dotnet restore

if (-not $SkipBuild) {
    # Compilar solución
    Write-Step "Compilando solución"
    dotnet build --no-restore > errores.txt 2>&1
    
    if ($LASTEXITCODE -ne 0) {
        Write-Warning "La compilación falló. Verificando errores..."
        Get-Content errores.txt
        
        if ($AutoFix) {
            Write-Step "Iniciando proceso de auto-corrección"
            & "$PSScriptRoot\Auto-Fix.ps1"
        }
    }
}

if (-not $SkipTests) {
    # Ejecutar tests
    Write-Step "Ejecutando tests"
    dotnet test --no-build
}

# Iniciar servicios
Write-Step "Iniciando servicios"

# Iniciar SQL Server si no está corriendo
$sqlService = Get-Service -Name "MSSQLSERVER" -ErrorAction SilentlyContinue
if ($sqlService -and $sqlService.Status -ne 'Running') {
    Write-Host "Iniciando SQL Server..."
    Start-Service MSSQLSERVER
}

# Iniciar API en segundo plano
Write-Step "Iniciando API"
Start-Process -FilePath "dotnet" -ArgumentList "run --project IncliSafe.Server" -WindowStyle Hidden

# Iniciar Cliente Blazor
Write-Step "Iniciando Cliente Blazor"
Start-Process -FilePath "dotnet" -ArgumentList "run --project IncliSafe.Client" -WindowStyle Hidden

Write-Host "`nIncliSafe está en ejecución!" -ForegroundColor Green
Write-Host "API: https://localhost:7001"
Write-Host "Cliente: https://localhost:7000"
Write-Host "`nPresiona Ctrl+C para detener todos los servicios..."

try {
    # Mantener el script en ejecución
    while ($true) {
        Start-Sleep -Seconds 1
    }
}
finally {
    # Cleanup al cerrar
    Write-Step "Deteniendo servicios"
    Get-Process -Name "dotnet" | Where-Object {$_.CommandLine -like "*IncliSafe*"} | Stop-Process
}
finally {
    Stop-Transcript
} 