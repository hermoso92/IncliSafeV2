# AutoDeploy Script for IncliSafe
# Author: Claude AI
# Version: 1.0.0

# Habilitar la ejecución sin confirmaciones
$ErrorActionPreference = 'Continue'
$ConfirmPreference = 'None'
$ProgressPreference = 'SilentlyContinue'

# Configuración global
$config = @{
    SolutionPath = (Resolve-Path ".\IncliSafe.sln")
    ApiPath = ".\IncliSafeApi"
    ClientPath = ".\IncliSafe.Client"
    SharedPath = ".\IncliSafe.Shared"
    LogPath = ".\logs"
    BackupPath = ".\backups"
    ConfigPath = ".\config"
}

# Función para escribir logs
function Write-Log {
    param(
        [string]$Message,
        [string]$Type = "INFO"
    )
    
    $timestamp = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
    $logMessage = "[$timestamp] [$Type] $Message"
    Write-Host $logMessage
    Add-Content -Path "$($config.LogPath)\autodeploy.log" -Value $logMessage
}

# Función para crear backup
function Backup-Project {
    try {
        Write-Log "Creating backup..." "BACKUP"
        
        # Crear nombre de archivo con timestamp
        $timestamp = Get-Date -Format "yyyyMMdd_HHmmss"
        $backupFile = Join-Path $config.BackupPath "backup_$timestamp.zip"
        
        # Asegurarse de que el directorio de backup existe
        if (-not (Test-Path $config.BackupPath)) {
            New-Item -ItemType Directory -Path $config.BackupPath -Force | Out-Null
        }
        
        # Crear lista de exclusiones
        $excludes = @(
            '*.zip',
            '*/bin/*',
            '*/obj/*',
            '*/.vs/*',
            '*/node_modules/*',
            '*/logs/*',
            '*/backups/*',
            '*/.git/*',
            '*/TestResults/*'
        )
        
        # Obtener archivos a comprimir
        $filesToBackup = Get-ChildItem -Path . -Exclude $excludes |
            Where-Object { -not ($_.FullName -like "*\bin\*" -or 
                               $_.FullName -like "*\obj\*" -or 
                               $_.FullName -like "*\.vs\*" -or 
                               $_.FullName -like "*\node_modules\*" -or 
                               $_.FullName -like "*\logs\*" -or 
                               $_.FullName -like "*\backups\*" -or 
                               $_.FullName -like "*\.git\*" -or 
                               $_.FullName -like "*\TestResults\*") }
        
        # Comprimir archivos en lotes pequeños
        $tempPath = Join-Path $config.BackupPath "temp"
        New-Item -ItemType Directory -Path $tempPath -Force | Out-Null
        
        try {
            $batchSize = 100
            $batch = 1
            $totalFiles = $filesToBackup.Count
            
            for ($i = 0; $i -lt $totalFiles; $i += $batchSize) {
                $batchFiles = $filesToBackup | Select-Object -Skip $i -First $batchSize
                $batchZip = Join-Path $tempPath "batch_$batch.zip"
                
                Compress-Archive -Path $batchFiles -DestinationPath $batchZip -Force
                $batch++
            }
            
            # Combinar todos los archivos ZIP en uno solo
            Get-ChildItem -Path $tempPath -Filter "*.zip" | 
                ForEach-Object { Expand-Archive -Path $_.FullName -DestinationPath $tempPath -Force }
            
            Remove-Item -Path "$tempPath\*.zip"
            
            Compress-Archive -Path "$tempPath\*" -DestinationPath $backupFile -Force
        }
        finally {
            # Limpiar archivos temporales
            if (Test-Path $tempPath) {
                Remove-Item -Path $tempPath -Recurse -Force
            }
        }
        
        Write-Log "Backup created at: $backupFile" "BACKUP"
        return $true
    }
    catch {
        Write-Log "Error creating backup: $_" "ERROR"
        Write-Log "Skipping backup and continuing with deployment..." "WARNING"
        return $false
    }
}

# Función para verificar herramientas
function Test-RequiredTools {
    Write-Log "Verificando herramientas necesarias..." "INIT"
    
    $tools = @{
        "dotnet" = {
            try {
                $version = (dotnet --version)
                if ($version -match "8.0") {
                    return $true
                }
                Write-Log "Se requiere .NET SDK 8.0. Versión actual: $version" "ERROR"
                return $false
            }
            catch {
                Write-Log ".NET SDK no está instalado o no está en el PATH" "ERROR"
                Write-Log "Por favor, instala .NET SDK 8.0 desde: https://dotnet.microsoft.com/download/dotnet/8.0" "INFO"
                return $false
            }
        }
        "git" = {
            try {
                $version = (git --version)
                return $true
            }
            catch {
                Write-Log "Git no está instalado o no está en el PATH" "ERROR"
                Write-Log "Por favor, instala Git desde: https://git-scm.com/downloads" "INFO"
                return $false
            }
        }
        "node" = {
            try {
                $version = (node --version)
                return $true
            }
            catch {
                Write-Log "Node.js no está instalado o no está en el PATH" "ERROR"
                Write-Log "Por favor, instala Node.js desde: https://nodejs.org/" "INFO"
                return $false
            }
        }
    }
    
    $allToolsPresent = $true
    foreach ($tool in $tools.Keys) {
        Write-Log "Verificando $tool..." "INIT"
        if (-not (& $tools[$tool])) {
            $allToolsPresent = $false
        }
    }
    
    return $allToolsPresent
}

# Función para inicializar el entorno
function Initialize-Environment {
    try {
        Write-Log "Initializing environment..." "INIT"
        
        # Crear directorios necesarios
        @(
            $config.LogPath,
            $config.BackupPath,
            $config.ConfigPath,
            "$($config.LogPath)\errors",
            "$($config.LogPath)\builds"
        ) | ForEach-Object {
            if (-not (Test-Path $_)) {
                New-Item -ItemType Directory -Path $_ -Force | Out-Null
            }
        }
        
        # Configurar política de ejecución para el proceso actual
        Set-ExecutionPolicy Bypass -Scope Process -Force
        
        # Verificar herramientas necesarias
        if (-not (Test-RequiredTools)) {
            throw "Missing required tools. Please install missing tools and try again."
        }
        
        # Instalar herramientas .NET si no están presentes
        $dotnetTools = @(
            "dotnet-ef",
            "dotnet-format"
        )
        
        foreach ($tool in $dotnetTools) {
            if (-not (Get-Command $tool -ErrorAction SilentlyContinue)) {
                Write-Log "Installing $tool..." "INIT"
                dotnet tool install -g $tool --no-cache
            }
        }
        
        Write-Log "Environment initialized successfully" "INIT"
        return $true
    }
    catch {
        Write-Log "Error initializing environment: $_" "ERROR"
        return $false
    }
}

# Función para actualizar dependencias
function Update-Dependencies {
    try {
        Write-Log "Updating dependencies..." "DEPS"
        
        # Actualizar paquetes NuGet
        Write-Log "Restoring NuGet packages..." "DEPS"
        dotnet restore $config.SolutionPath --force --no-cache
        
        # Verificar si hay advertencias de versiones incompatibles
        $projects = Get-ChildItem -Path . -Filter "*.csproj" -Recurse
        foreach ($project in $projects) {
            Write-Log "Checking dependencies for $($project.Name)..." "DEPS"
            
            # Actualizar paquetes críticos
            $packages = @(
                @{ Name = "Microsoft.EntityFrameworkCore.SqlServer"; Version = "8.0.0" },
                @{ Name = "Microsoft.AspNetCore.Components.WebAssembly"; Version = "8.0.0" },
                @{ Name = "MudBlazor"; Version = "6.16.0" },
                @{ Name = "AutoMapper"; Version = "13.0.1" }
            )
            
            foreach ($package in $packages) {
                try {
                    dotnet add $project.FullName package $package.Name -v $package.Version --no-restore
                    Write-Log "Updated $($package.Name) to version $($package.Version) in $($project.Name)" "DEPS"
                }
                catch {
                    Write-Log "Warning: Could not update $($package.Name) in $($project.Name): $_" "WARNING"
                }
            }
        }
        
        # Restaurar nuevamente para asegurar consistencia
        dotnet restore $config.SolutionPath --force
        
        Write-Log "Dependencies updated successfully" "DEPS"
        return $true
    }
    catch {
        Write-Log "Error updating dependencies: $_" "ERROR"
        return $false
    }
}

# Función para compilar la solución
function Build-Solution {
    Write-Log "Building solution..." "BUILD"
    
    $buildLog = "$($config.LogPath)\builds\build_$(Get-Date -Format 'yyyyMMdd_HHmmss').log"
    
    # Limpiar solución
    dotnet clean $config.SolutionPath --configuration Release | Out-File $buildLog
    
    # Compilar solución
    dotnet build $config.SolutionPath --configuration Release --no-restore | Out-File $buildLog -Append
    
    if ($LASTEXITCODE -eq 0) {
        Write-Log "Build completed successfully" "BUILD"
        return $true
    }
    else {
        Write-Log "Build failed. Check log: $buildLog" "ERROR"
        return $false
    }
}

# Función para ejecutar pruebas
function Test-Solution {
    Write-Log "Running tests..." "TEST"
    
    $testLog = "$($config.LogPath)\tests_$(Get-Date -Format 'yyyyMMdd_HHmmss').log"
    
    dotnet test $config.SolutionPath --configuration Release --no-build | Out-File $testLog
    
    if ($LASTEXITCODE -eq 0) {
        Write-Log "Tests completed successfully" "TEST"
        return $true
    }
    else {
        Write-Log "Tests failed. Check log: $testLog" "ERROR"
        return $false
    }
}

# Función para iniciar servicios
function Start-Services {
    Write-Log "Starting services..." "SERVICE"
    
    # Iniciar API
    $apiProcess = Start-Process "dotnet" -ArgumentList "run --project $($config.ApiPath)" -PassThru -NoNewWindow
    Write-Log "API started (PID: $($apiProcess.Id))" "SERVICE"
    
    # Iniciar Cliente
    $clientProcess = Start-Process "dotnet" -ArgumentList "run --project $($config.ClientPath)" -PassThru -NoNewWindow
    Write-Log "Client started (PID: $($clientProcess.Id))" "SERVICE"
    
    return @{
        API = $apiProcess
        Client = $clientProcess
    }
}

# Función para monitorear servicios
function Watch-Services {
    param (
        [hashtable]$processes
    )
    
    Write-Log "Monitoring services..." "MONITOR"
    
    while ($true) {
        foreach ($service in $processes.Keys) {
            $process = $processes[$service]
            if ($process.HasExited) {
                Write-Log "$service process exited unexpectedly. Restarting..." "WARNING"
                
                # Reiniciar servicio
                if ($service -eq "API") {
                    $processes.API = Start-Process "dotnet" -ArgumentList "run --project $($config.ApiPath)" -PassThru -WindowStyle Hidden
                }
                else {
                    $processes.Client = Start-Process "dotnet" -ArgumentList "run --project $($config.ClientPath)" -PassThru -WindowStyle Hidden
                }
            }
        }
        
        Start-Sleep -Seconds 5
    }
}

# Bloque principal de ejecución
try {
    Write-Log "Starting AutoDeploy process..." "START"
    
    # Crear backup
    if (-not (Backup-Project)) {
        Write-Log "Backup failed, but continuing with deployment..." "WARNING"
    }
    
    # Inicializar entorno
    if (-not (Initialize-Environment)) {
        throw "Environment initialization failed"
    }
    
    # Actualizar dependencias
    if (-not (Update-Dependencies)) {
        Write-Log "Warning: Some dependencies could not be updated" "WARNING"
    }
    
    # Compilar solución
    Write-Log "Building solution..." "BUILD"
    dotnet build --configuration Release
    
    # Ejecutar pruebas
    Write-Log "Running tests..." "TEST"
    dotnet test
    
    # Iniciar servicios
    Write-Log "Starting services..." "SERVICE"
    $processes = Start-Services
    
    # Monitorear servicios
    Write-Log "Monitoring services..." "MONITOR"
    Watch-Services -processes $processes
}
catch {
    Write-Log "Error during AutoDeploy: $_" "ERROR"
    exit 1
} 