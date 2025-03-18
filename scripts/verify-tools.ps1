# Script para verificar herramientas instaladas
Write-Host "`nVerificando herramientas necesarias..." -ForegroundColor Cyan

function Test-Command {
    param($Command)
    
    try {
        Get-Command $Command -ErrorAction Stop | Out-Null
        return $true
    }
    catch {
        return $false
    }
}

function Write-Status {
    param($Tool, $IsInstalled)
    
    $status = if ($IsInstalled) { "✅" } else { "❌" }
    Write-Host "$status $Tool"
}

# Verificar .NET SDK
$dotnetVersion = dotnet --version
Write-Status ".NET SDK" ($dotnetVersion -like "8.*")

# Verificar Git
$gitInstalled = Test-Command "git"
Write-Status "Git" $gitInstalled

# Verificar Node.js
$nodeInstalled = Test-Command "node"
Write-Status "Node.js" $nodeInstalled

# Verificar SQL Server
$sqlInstalled = Get-Service -Name "MSSQLSERVER" -ErrorAction SilentlyContinue
Write-Status "SQL Server" ($sqlInstalled -ne $null)

# Verificar Visual Studio / VS Code
$vsInstalled = Test-Path "C:\Program Files\Microsoft Visual Studio\2022"
$vscodeInstalled = Test-Command "code"
Write-Status "Visual Studio 2022" $vsInstalled
Write-Status "VS Code" $vscodeInstalled

# Verificar PowerShell version
$psVersion = $PSVersionTable.PSVersion
Write-Status "PowerShell 7+" ($psVersion.Major -ge 7)

# Verificar directorios del proyecto
$directories = @(
    "IncliSafe.Client",
    "IncliSafe.Server",
    "IncliSafe.Shared",
    "IncliSafe.Tests",
    "docs",
    "scripts"
)

Write-Host "`nVerificando estructura del proyecto:"
foreach ($dir in $directories) {
    $exists = Test-Path $dir
    Write-Status $dir $exists
}

# Verificar archivos de configuración
$configFiles = @(
    "global.json",
    "Directory.Build.props",
    ".editorconfig",
    "errores.txt",
    "docs/soluciones.txt"
)

Write-Host "`nVerificando archivos de configuración:"
foreach ($file in $configFiles) {
    $exists = Test-Path $file
    Write-Status $file $exists
    
    if (-not $exists) {
        Write-Host "  Creando $file..."
        New-Item -ItemType File -Path $file -Force
    }
}

Write-Host "`n" 