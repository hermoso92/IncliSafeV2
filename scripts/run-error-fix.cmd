@echo off
setlocal enabledelayedexpansion

echo IncliSafe Error Fix Script
echo ========================
echo.

REM Check if PowerShell is available
powershell -Command "$null" 2>nul
if %ERRORLEVEL% NEQ 0 (
    echo Error: PowerShell is not available
    echo Please install PowerShell and try again
    exit /b 1
)

REM Create logs directory if it doesn't exist
if not exist "logs" mkdir logs
if not exist "logs\errors" mkdir logs\errors

REM Run the PowerShell script
echo Running error fix script...
powershell -ExecutionPolicy Bypass -File "%~dp0IncliSafe-ErrorFix.ps1"
if %ERRORLEVEL% NEQ 0 (
    echo.
    echo Error: Failed to fix errors
    echo Check logs\errors for details
    exit /b 1
)

echo.
echo Error fix completed successfully!
echo Check logs\errors for details
exit /b 0 