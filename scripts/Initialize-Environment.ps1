# Environment Initialization Script
# Author: Claude AI
# Version: 1.0.0

# Función para verificar y solicitar permisos de administrador si es necesario
function Request-AdminPrivileges {
    $currentPrincipal = New-Object Security.Principal.WindowsPrincipal([Security.Principal.WindowsIdentity]::GetCurrent())
    $isAdmin = $currentPrincipal.IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)
    
    if (-not $isAdmin) {
        Write-Host "Este script requiere privilegios de administrador. Solicitando elevación..." -ForegroundColor Yellow
        Start-Process powershell -Verb RunAs -ArgumentList "-NoProfile -ExecutionPolicy Bypass -File `"$PSCommandPath`""
        exit
    }
}

# Función para configurar la política de ejecución de PowerShell
function Set-ExecutionPolicy {
    Write-Host "Configurando política de ejecución de PowerShell..." -ForegroundColor Cyan
    Set-ExecutionPolicy RemoteSigned -Scope CurrentUser -Force
}

# Función para instalar herramientas necesarias
function Install-RequiredTools {
    Write-Host "Instalando herramientas necesarias..." -ForegroundColor Cyan
    
    # Verificar e instalar chocolatey si no está instalado
    if (-not (Get-Command choco -ErrorAction SilentlyContinue)) {
        Write-Host "Instalando Chocolatey..." -ForegroundColor Yellow
        Set-ExecutionPolicy Bypass -Scope Process -Force
        [System.Net.ServicePointManager]::SecurityProtocol = [System.Net.ServicePointManager]::SecurityProtocol -bor 3072
        Invoke-Expression ((New-Object System.Net.WebClient).DownloadString('https://chocolatey.org/install.ps1'))
    }
    
    # Lista de herramientas necesarias
    $tools = @(
        "dotnet-sdk",
        "git",
        "nodejs",
        "sql-server-management-studio",
        "vscode"
    )
    
    foreach ($tool in $tools) {
        Write-Host "Instalando $tool..." -ForegroundColor Yellow
        choco install $tool -y
    }
}

# Función para instalar herramientas .NET
function Install-DotNetTools {
    Write-Host "Instalando herramientas .NET..." -ForegroundColor Cyan
    
    $tools = @(
        "dotnet-ef",
        "dotnet-format",
        "dotnet-reportgenerator-globaltool",
        "coverlet.console",
        "security-scan",
        "docfx"
    )
    
    foreach ($tool in $tools) {
        Write-Host "Instalando $tool..." -ForegroundColor Yellow
        dotnet tool install -g $tool
    }
}

# Función para configurar SQL Server
function Initialize-SqlServer {
    Write-Host "Configurando SQL Server..." -ForegroundColor Cyan
    
    # Habilitar TCP/IP
    $smo = 'Microsoft.SqlServer.Management.Smo.'
    $wmi = New-Object ($smo + 'Wmi.ManagedComputer')
    $uri = "ManagedComputer[@Name='$env:COMPUTERNAME']/ServerInstance[@Name='MSSQLSERVER']/ServerProtocol[@Name='Tcp']"
    $tcp = $wmi.GetSmoObject($uri)
    $tcp.IsEnabled = $true
    $tcp.Alter()
    
    # Reiniciar servicio
    Restart-Service -Name 'MSSQLSERVER' -Force
    
    # Crear base de datos IncliSafe
    sqlcmd -Q "IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'IncliSafe') CREATE DATABASE IncliSafe"
}

# Función para configurar Git
function Initialize-Git {
    Write-Host "Configurando Git..." -ForegroundColor Cyan
    
    # Configurar nombre y email si no están configurados
    if (-not (git config --global user.name)) {
        $name = Read-Host "Ingrese su nombre para la configuración de Git"
        git config --global user.name $name
    }
    
    if (-not (git config --global user.email)) {
        $email = Read-Host "Ingrese su email para la configuración de Git"
        git config --global user.email $email
    }
    
    # Configurar Git LFS
    git lfs install
    
    # Configurar hooks de pre-commit
    $hookPath = ".git/hooks/pre-commit"
    @"
#!/bin/sh
# Pre-commit hook para IncliSafe

# Ejecutar pruebas
echo "Ejecutando pruebas..."
dotnet test

# Verificar formato del código
echo "Verificando formato del código..."
dotnet format --verify-no-changes

# Análisis de seguridad
echo "Ejecutando análisis de seguridad..."
security-scan
"@ | Set-Content $hookPath
}

# Función para configurar VSCode
function Initialize-VSCode {
    Write-Host "Configurando Visual Studio Code..." -ForegroundColor Cyan
    
    # Instalar extensiones necesarias
    $extensions = @(
        "ms-dotnettools.csharp",
        "ms-dotnettools.blazor-debugging",
        "ms-mssql.mssql",
        "formulahendry.dotnet-test-explorer",
        "streetsidesoftware.code-spell-checker",
        "editorconfig.editorconfig",
        "davidanson.vscode-markdownlint",
        "ms-dotnettools.dotnet-interactive-vscode"
    )
    
    foreach ($extension in $extensions) {
        Write-Host "Instalando extensión VSCode: $extension" -ForegroundColor Yellow
        code --install-extension $extension
    }
}

# Función para crear certificado de desarrollo
function New-DevelopmentCertificate {
    Write-Host "Creando certificado de desarrollo..." -ForegroundColor Cyan
    
    dotnet dev-certs https --clean
    dotnet dev-certs https --trust
}

# Función principal
try {
    Write-Host "Iniciando configuración del entorno de desarrollo IncliSafe..." -ForegroundColor Green
    
    Request-AdminPrivileges
    Set-ExecutionPolicy
    Install-RequiredTools
    Install-DotNetTools
    Initialize-SqlServer
    Initialize-Git
    Initialize-VSCode
    New-DevelopmentCertificate
    
    Write-Host "`nConfiguración completada exitosamente!" -ForegroundColor Green
    Write-Host "Por favor, reinicie su terminal para aplicar todos los cambios." -ForegroundColor Yellow
}
catch {
    Write-Error "Error durante la configuración: $_"
    exit 1
} 