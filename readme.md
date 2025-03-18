# 🚀 IncliSafe V2

Sistema de monitoreo y análisis de inclinación para vehículos pesados.

## 📋 Estructura del Proyecto

```
IncliSafe.V2/
├── IncliSafe.Client/           # Cliente Blazor WebAssembly
├── IncliSafe.Server/           # API Backend .NET 8
└── IncliSafe.Shared/          # Modelos y lógica compartida
    ├── Models/
    │   ├── Analysis/          # Modelos de análisis
    │   │   └── Core/         # Clases base de análisis
    │   ├── DTOs/             # Data Transfer Objects
    │   ├── Enums/            # Enumeraciones
    │   ├── Patterns/         # Modelos de patrones
    │   ├── Notifications/    # Sistema de notificaciones
    │   ├── Common/           # Clases base y utilidades
    │   └── Entities/         # Entidades de dominio
    ├── Extensions/           # Métodos de extensión
    └── Interfaces/           # Interfaces compartidas
```

## 🔧 Regeneración del Proyecto

### Prerrequisitos

- .NET 8 SDK
- Visual Studio 2022 o superior
- SQL Server 2019 o superior
- Git

### Script de Regeneración

El proyecto incluye un script PowerShell (`regenerate.ps1`) que automatiza la regeneración:

```powershell
# Limpiar, restaurar y compilar
.\regenerate.ps1 -Clean -Restore -Build

# Solo regenerar estructura sin sobrescribir
.\regenerate.ps1

# Forzar regeneración (sobrescribe archivos)
.\regenerate.ps1 -Force
```

### Pasos Manuales

1. Clonar el repositorio:
```bash
git clone https://github.com/tu-usuario/IncliSafeV2.git
cd IncliSafeV2
```

2. Restaurar paquetes:
```bash
dotnet restore
```

3. Crear estructura de directorios:
```bash
mkdir -p IncliSafe.Shared/Models/{Analysis/Core,DTOs,Enums,Patterns,Notifications,Common,Entities}
mkdir -p IncliSafe.Shared/{Extensions,Interfaces}
```

4. Compilar la solución:
```bash
dotnet build
```

## 📚 Estructura de Modelos

### Core Analysis Models

- `AnalysisPrediction`: Predicciones basadas en análisis histórico
- `AnalysisResult`: Resultados de análisis con métricas
- `Anomaly`: Detección de anomalías
- `DashboardMetrics`: Métricas para el dashboard
- `PredictionType`: Tipos de predicción
- `PatternType`: Tipos de patrones detectados

### DTOs

- `AnalysisDTO`: Base para DTOs de análisis
- `DobackAnalysisDTO`: Análisis Doback específico
- `VehicleDTO`: Información de vehículos
- `VehicleAlertDTO`: Alertas de vehículos
- `AlertSettingsDTO`: Configuración de alertas

### Enumeraciones

- `AnalysisType`: Tipos de análisis
- `AnomalyType`: Tipos de anomalías
- `PredictionCategory`: Categorías de predicción
- `AnalysisStatus`: Estados de análisis
- `TrendDirection`: Dirección de tendencias
- `PerformanceTrend`: Tendencias de rendimiento

## 🔄 Métodos de Extensión

### Analysis Extensions

```csharp
public static class AnalysisExtensions
{
    // Cálculo de estabilidad
    public static decimal CalculateStabilityScore(this DobackData data);
    
    // Análisis de tendencias
    public static TrendDirection CalculateTrendDirection(this IEnumerable<decimal> values);
    
    // Evaluación de rendimiento
    public static PerformanceTrend CalculatePerformanceTrend(this IEnumerable<decimal> scores);
}
```

## 🔒 Seguridad

### Blazor WebAssembly
- Autenticación JWT
- Protección de rutas con `AuthorizeView`
- Manejo seguro de tokens en `LocalStorage`

### API Backend
- Bearer Authentication
- Autorización basada en roles
- Sanitización de datos de entrada

## 📦 Paquetes NuGet Principales

```xml
<ItemGroup>
    <PackageReference Include="Microsoft.AspNetCore.Components.WebAssembly" Version="8.0.0" />
    <PackageReference Include="Microsoft.AspNetCore.Components.WebAssembly.Authentication" Version="8.0.0" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="8.0.0" />
    <PackageReference Include="AutoMapper.Collection" Version="9.0.0" />
    <PackageReference Include="FluentValidation" Version="11.8.0" />
</ItemGroup>
```

## 🚀 Flujo de Trabajo Git

1. Inicializar repositorio:
```bash
git init
git add .
git commit -m "Initial commit"
```

2. Configurar remote:
```bash
git remote add origin https://github.com/tu-usuario/IncliSafeV2.git
git push -u origin main
```

3. Flujo de trabajo diario:
```bash
git pull                           # Obtener cambios
git checkout -b feature/nueva-funcionalidad  # Nueva rama
git add .                          # Agregar cambios
git commit -m "feat: descripción"  # Commit
git push origin feature/nueva-funcionalidad  # Push
```

## 📝 Convenciones de Código

- Usar `required` para propiedades obligatorias
- Preferir `decimal` sobre `double` para cálculos precisos
- Inicializar colecciones en constructores
- Usar DTOs para transferencia de datos
- Implementar métodos de mapeo en DTOs

## 🔍 Debugging

1. Configurar logging:
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  }
}
```

2. Usar puntos de interrupción estratégicos en:
   - Métodos de análisis
   - Cálculos de métricas
   - Procesamiento de datos en tiempo real

## 📊 Optimización

- Usar `AsNoTracking()` para consultas de solo lectura
- Implementar caché donde sea apropiado
- Evitar N+1 queries usando `Include()`
- Optimizar renders en Blazor con `ShouldRender()`

## 🤝 Contribución

1. Fork el repositorio
2. Crear rama feature (`git checkout -b feature/AmazingFeature`)
3. Commit cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abrir Pull Request

## 📄 Licencia

Distribuido bajo la Licencia MIT. Ver `LICENSE` para más información.

## 📞 Contacto

Tu Nombre - [@tutwitter](https://twitter.com/tutwitter) - email@example.com

Project Link: [https://github.com/tu-usuario/IncliSafeV2](https://github.com/tu-usuario/IncliSafeV2)