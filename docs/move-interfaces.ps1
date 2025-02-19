# Función para obtener la estructura del directorio
function Get-DirectoryStructure {
    param (
        [string]$Path,
        [string]$Indent = "",
        [string[]]$ExcludeFolder = @('bin', 'obj', 'node_modules', '.git', '.vs')
    )

    # Obtener y ordenar los directorios
    Get-ChildItem $Path -Directory | 
    Where-Object { $ExcludeFolder -notcontains $_.Name } | 
    ForEach-Object {
        # Imprimir el directorio actual
        "$Indent├── $($_.Name)"
        
        # Recursivamente obtener la estructura de los subdirectorios
        Get-DirectoryStructure -Path $_.FullName -Indent "$Indent│   " -ExcludeFolder $ExcludeFolder
    }

    # Obtener y ordenar los archivos
    Get-ChildItem $Path -File | 
    ForEach-Object {
        "$Indent├── $($_.Name)"
    }
}

# Ruta del proyecto (ajusta esto a la ruta de tu proyecto)
$projectPath = "."

# Crear archivo de salida
$outputFile = "project-structure.txt"

# Obtener el nombre del directorio base
$baseName = Split-Path $projectPath -Leaf
if ([string]::IsNullOrEmpty($baseName)) {
    $baseName = "Project Root"
}

# Guardar la estructura en el archivo
"$baseName" | Out-File $outputFile
Get-DirectoryStructure -Path $projectPath | Out-File $outputFile -Append

Write-Host "Estructura del proyecto guardada en $outputFile"