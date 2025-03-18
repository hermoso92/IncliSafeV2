# MCP Server Management Script
# Author: Claude AI
# Version: 1.0.0

param(
    [string]$Port = "5000",
    [string]$ConfigPath = "config/mcp-config.json"
)

$ErrorActionPreference = 'Stop'
$VerbosePreference = 'Continue'
$root = Split-Path -Parent (Split-Path -Parent $MyInvocation.MyCommand.Path)

# Configuration
$config = @{
    McpPort = $Port
    LogPath = "..\logs\mcp"
    DevicesPath = "..\config\devices.json"
    CertPath = "..\certificates"
    BufferSize = 1024
    SamplingRate = 100  # Hz
    MaxConnections = 100
    Timeout = 30000
    LogLevel = "Information"
    Database = @{
        ConnectionString = "Server=localhost;Database=IncliSafe;Trusted_Connection=True;"
        MaxPoolSize = 100
        CommandTimeout = 30
    }
}

function Write-Log {
    param($Message)
    Write-Host "$(Get-Date -Format 'yyyy-MM-dd HH:mm:ss'): $Message"
}

# Initialize MCP environment
function Initialize-McpEnvironment {
    Write-Verbose "Initializing MCP environment..."
    
    # Create required directories
    @(
        $config.LogPath,
        $config.CertPath
    ) | ForEach-Object {
        if (-not (Test-Path $_)) {
            New-Item -ItemType Directory -Path $_ -Force
        }
    }
    
    # Create default device configuration if not exists
    if (-not (Test-Path $config.DevicesPath)) {
        @{
            Devices = @(
                @{
                    Id = "DOBACK001"
                    Type = "Inclinometer"
                    IpAddress = "192.168.1.100"
                    Port = 5001
                    SamplingRate = 100
                    Calibration = @{
                        OffsetX = 0.0
                        OffsetY = 0.0
                        OffsetZ = 0.0
                        ScaleX = 1.0
                        ScaleY = 1.0
                        ScaleZ = 1.0
                    }
                }
            )
        } | ConvertTo-Json -Depth 10 | Set-Content $config.DevicesPath
    }
}

# Create self-signed certificate for secure communication
function New-McpCertificate {
    Write-Verbose "Creating MCP certificate..."
    
    $certPath = Join-Path $config.CertPath "mcp-server.pfx"
    if (-not (Test-Path $certPath)) {
        $cert = New-SelfSignedCertificate `
            -DnsName "IncliSafe-MCP" `
            -CertStoreLocation "Cert:\LocalMachine\My" `
            -NotAfter (Get-Date).AddYears(1) `
            -KeySpec KeyExchange
        
        $pwd = ConvertTo-SecureString -String "IncliSafe2024!" -Force -AsPlainText
        Export-PfxCertificate -Cert $cert -FilePath $certPath -Password $pwd
    }
}

function Initialize-McpServer {
    Write-Log "Inicializando servidor MCP..."
    
    # Verificar configuración
    if (-not (Test-Path $ConfigPath)) {
        Write-Log "Creando configuración por defecto..."
        $config = @{
            Port = $Port
            MaxConnections = 100
            BufferSize = 1024
            Timeout = 30000
            LogLevel = "Information"
            Database = @{
                ConnectionString = "Server=localhost;Database=IncliSafe;Trusted_Connection=True;"
                MaxPoolSize = 100
                CommandTimeout = 30
            }
        }
        
        $configDir = Split-Path -Parent $ConfigPath
        if (-not (Test-Path $configDir)) {
            New-Item -ItemType Directory -Path $configDir
        }
        
        $config | ConvertTo-Json -Depth 10 | Set-Content $ConfigPath
    }
    
    # Cargar configuración
    $config = Get-Content $ConfigPath | ConvertFrom-Json
    
    return $config
}

function Start-McpListener {
    param($Config)
    
    Write-Log "Iniciando listener MCP en puerto $($Config.Port)..."
    
    try {
        $endpoint = New-Object System.Net.IPEndPoint([System.Net.IPAddress]::Any, $Config.Port)
        $listener = New-Object System.Net.Sockets.TcpListener $endpoint
        $listener.Start()
        
        Write-Log "Listener MCP iniciado correctamente"
        
        while ($true) {
            if ($listener.Pending()) {
                $client = $listener.AcceptTcpClient()
                $clientEndPoint = $client.Client.RemoteEndPoint
                Write-Log "Nueva conexión desde $clientEndPoint"
                
                # Procesar cliente en un job separado
                Start-Job -ScriptBlock {
                    param($Client, $Config)
                    
                    try {
                        $stream = $Client.GetStream()
                        $reader = New-Object System.IO.StreamReader($stream)
                        $writer = New-Object System.IO.StreamWriter($stream)
                        $writer.AutoFlush = $true
                        
                        while ($Client.Connected) {
                            if ($stream.DataAvailable) {
                                $message = $reader.ReadLine()
                                if ($message) {
                                    # Procesar mensaje MCP
                                    $response = "ACK:$message"
                                    $writer.WriteLine($response)
                                }
                            }
                            Start-Sleep -Milliseconds 100
                        }
                    }
                    finally {
                        $Client.Close()
                    }
                } -ArgumentList $client, $Config
            }
            Start-Sleep -Milliseconds 100
        }
    }
    finally {
        if ($listener) {
            $listener.Stop()
        }
    }
}

# Monitor device connections
function Watch-McpDevices {
    Write-Verbose "Monitoring MCP devices..."
    
    $devices = Get-Content $config.DevicesPath | ConvertFrom-Json
    
    while ($true) {
        foreach ($device in $devices.Devices) {
            $result = Test-NetConnection -ComputerName $device.IpAddress -Port $device.Port -WarningAction SilentlyContinue
            
            if ($result.TcpTestSucceeded) {
                Write-Host "Device $($device.Id) is online" -ForegroundColor Green
            }
            else {
                Write-Host "Device $($device.Id) is offline" -ForegroundColor Red
                
                # Log connection failure
                $timestamp = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
                "$timestamp - Connection failed to device $($device.Id)" | 
                    Add-Content (Join-Path $config.LogPath "connection-errors.log")
            }
        }
        
        Start-Sleep -Seconds 30
    }
}

# Main execution flow
try {
    Write-Host "Starting IncliSafe MCP Server..." -ForegroundColor Cyan
    
    Initialize-McpEnvironment
    New-McpCertificate
    
    # Start device monitoring in background
    Start-Job -ScriptBlock ${function:Watch-McpDevices} -Name "DeviceMonitor"
    
    # Start MCP server
    $config = Initialize-McpServer
    Start-McpListener -Config $config
}
catch {
    Write-Log "Error en servidor MCP: $_"
    exit 1
}
finally {
    Stop-Job -Name "DeviceMonitor"
    Remove-Job -Name "DeviceMonitor"
} 