# Función para ejecutar dotnet build y capturar errores
function Build-AndFix {
    while ($true) {
        # Ejecutar dotnet build y redirigir errores a errores.txt
        dotnet build 2> errores.txt

        # Verificar si hay errores
        if ((Get-Content errores.txt).Length -gt 0) {
            Write-Host "Errores detectados. Intentando corregir..."
            # Llamar al script de Python para corregir errores
            python fix_errors.py
        } else {
            Write-Host "Compilación exitosa. No se detectaron errores."
            break
        }
    }
}
