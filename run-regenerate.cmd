@echo off
chcp 65001 > nul
setlocal enabledelayedexpansion

echo [%date% %time%] Iniciando proceso de regeneracion de IncliSafe...

REM Verificar que PowerShell este disponible
where powershell >nul 2>nul
if %ERRORLEVEL% neq 0 (
    echo [%date% %time%] ERROR: PowerShell no esta disponible en el sistema
    pause
    exit /b 1
)

REM Cambiar al directorio del script
cd /d "%~dp0"

REM Verificar que el script de PowerShell existe
if not exist "scripts\IncliSafe-Master.ps1" (
    echo [%date% %time%] ERROR: No se encuentra el archivo scripts\IncliSafe-Master.ps1
    pause
    exit /b 1
)

REM Crear directorio de logs si no existe
if not exist "logs\errors" mkdir "logs\errors" 2>nul

echo [%date% %time%] Ejecutando PowerShell script...

powershell.exe -NoProfile -ExecutionPolicy Bypass -Command "$OutputEncoding = [Console]::OutputEncoding = [Text.Encoding]::UTF8; & '.\scripts\IncliSafe-Master.ps1' -Verbose"

set EXIT_CODE=%ERRORLEVEL%

if %EXIT_CODE% neq 0 (
    echo [%date% %time%] ERROR: El proceso ha fallado con codigo %EXIT_CODE%
    if exist "logs\errors\%date:~0,10%.log" (
        echo Ultimos errores encontrados:
        type "logs\errors\%date:~0,10%.log"
    ) else (
        echo No se encontro el archivo de log de errores
    )
) else (
    echo [%date% %time%] Proceso completado exitosamente
)

pause 