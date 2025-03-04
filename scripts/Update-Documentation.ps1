# Script para actualizar la documentación del proyecto
param(
    [string]$WorkspacePath = "C:\Users\Hermoso\Desktop\IncliSafeV2",
    [switch]$ForceUpdate
)

# Importar configuración
$configPath = Join-Path $WorkspacePath "config\project-settings.json"
$config = Get-Content $configPath | ConvertFrom-Json

# Función para actualizar README
function Update-Readme {
    $readmePath = Join-Path $WorkspacePath "README.md"
    $date = Get-Date -Format "yyyy-MM-dd"
    
    # Obtener última versión del proyecto
    $version = $config.project.version
    
    # Actualizar versión y fecha
    $content = Get-Content $readmePath
    $content = $content -replace "^# IncliSafe v\d+\.\d+\.\d+", "# IncliSafe v$version"
    $content = $content -replace "Última actualización: \d{4}-\d{2}-\d{2}", "Última actualización: $date"
    
    $content | Set-Content $readmePath
}

# Función para actualizar documentación de desarrollo
function Update-DevelopmentDocs {
    $processPath = Join-Path $WorkspacePath "docs\development-process.md"
    $date = Get-Date -Format "yyyy-MM-dd"
    
    # Obtener estadísticas del proyecto
    $files = Get-ChildItem -Path $WorkspacePath -Recurse -File
    $totalFiles = $files.Count
    $totalLines = ($files | Get-Content | Measure-Object -Line).Lines
    
    # Actualizar estadísticas
    $content = Get-Content $processPath
    $content = $content -replace "Última actualización: \d{4}-\d{2}-\d{2}", "Última actualización: $date"
    $content = $content -replace "Total archivos: \d+", "Total archivos: $totalFiles"
    $content = $content -replace "Total líneas: \d+", "Total líneas: $totalLines"
    
    $content | Set-Content $processPath
}

# Función para actualizar registro de errores
function Update-ErrorLog {
    $errorLogPath = Join-Path $WorkspacePath "docs\error-solutions-log.md"
    $date = Get-Date -Format "yyyy-MM-dd"
    
    # Compilar proyecto y obtener errores
    $buildResult = dotnet build $WorkspacePath 2>&1
    $errors = $buildResult | Where-Object { $_ -match "error CS\d+:" }
    $warnings = $buildResult | Where-Object { $_ -match "warning CS\d+:" }
    
    # Crear entrada en el log
    $entry = @"

### Compilación ($date)
#### Errores ($($errors.Count))
$($errors -join "`n")

#### Advertencias ($($warnings.Count))
$($warnings -join "`n")
"@
    
    Add-Content $errorLogPath $entry
}

# Función para actualizar documentación API
function Update-ApiDocs {
    $apiDocsPath = Join-Path $WorkspacePath "docs\api"
    
    # Crear directorio si no existe
    if (-not (Test-Path $apiDocsPath)) {
        New-Item -ItemType Directory -Path $apiDocsPath
    }
    
    # Generar documentación API con Swagger
    dotnet swagger tofile --output "$apiDocsPath\swagger.json" "$WorkspacePath\IncliSafeApi\bin\Debug\net8.0\IncliSafeApi.dll" v1
}

# Función para actualizar documentación de componentes
function Update-ComponentDocs {
    $componentDocsPath = Join-Path $WorkspacePath "docs\components"
    
    # Crear directorio si no existe
    if (-not (Test-Path $componentDocsPath)) {
        New-Item -ItemType Directory -Path $componentDocsPath
    }
    
    # Obtener todos los componentes Blazor
    $components = Get-ChildItem -Path "$WorkspacePath\IncliSafe.Client" -Recurse -Filter "*.razor"
    
    foreach ($component in $components) {
        $componentName = [System.IO.Path]::GetFileNameWithoutExtension($component.Name)
        $docPath = Join-Path $componentDocsPath "$componentName.md"
        
        # Extraer documentación del componente
        $content = Get-Content $component.FullName
        $summary = $content | Where-Object { $_ -match "/// <summary>" }
        $parameters = $content | Where-Object { $_ -match "\[Parameter\]" }
        
        # Crear documentación
        $doc = @"
# $componentName

## Descripción
$summary

## Parámetros
$parameters

## Uso
\`\`\`razor
<$componentName>
    <!-- Contenido -->
</$componentName>
\`\`\`
"@
        
        $doc | Set-Content $docPath
    }
}

# Función principal
function Update-Documentation {
    try {
        Write-Host "Actualizando documentación..."
        
        # 1. Actualizar README
        Write-Host "1. Actualizando README..."
        Update-Readme
        
        # 2. Actualizar docs de desarrollo
        Write-Host "2. Actualizando documentación de desarrollo..."
        Update-DevelopmentDocs
        
        # 3. Actualizar registro de errores
        Write-Host "3. Actualizando registro de errores..."
        Update-ErrorLog
        
        # 4. Actualizar docs API
        Write-Host "4. Actualizando documentación API..."
        Update-ApiDocs
        
        # 5. Actualizar docs componentes
        Write-Host "5. Actualizando documentación de componentes..."
        Update-ComponentDocs
        
        Write-Host "Documentación actualizada correctamente."
    }
    catch {
        Write-Error "Error al actualizar la documentación: $_"
        exit 1
    }
}

# Ejecutar actualización
Update-Documentation 