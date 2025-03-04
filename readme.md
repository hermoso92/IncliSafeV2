# IncliSafe v2.0

## Descripción
Sistema de monitoreo y análisis de inclinación para vehículos pesados utilizando Blazor WebAssembly, .NET 8 y SQL Server.

## Estructura del Proyecto
```
📂 IncliSafe.Client (Blazor WebAssembly)
 ├── 📂 Auth          # Autenticación y autorización
 ├── 📂 Dialogs      # Componentes de diálogo
 ├── 📂 Models       # Modelos del cliente
 ├── 📂 Pages        # Páginas de la aplicación
 ├── 📂 Services     # Servicios del cliente
 ├── 📂 Shared       # Componentes compartidos
 └── Program.cs      # Punto de entrada

📂 IncliSafeApi (.NET 8)
 ├── 📂 Controllers  # Controladores API
 ├── 📂 Data        # Contexto y migraciones
 ├── 📂 Models      # Modelos de la API
 ├── 📂 Services    # Servicios de negocio
 └── Program.cs     # Configuración API

📂 IncliSafe.Shared
 └── 📂 Models      # Modelos compartidos
```

## Tecnologías
- .NET 8
- Blazor WebAssembly
- Microsoft SQL Server
- Entity Framework Core
- MudBlazor

## Configuración del Desarrollo

### Requisitos
- .NET 8 SDK
- SQL Server 2019+
- Visual Studio 2022 o VS Code
- Node.js 18+

### Instalación
1. Clonar el repositorio:
   ```bash
   git clone https://github.com/tu-usuario/IncliSafeV2.git
   cd IncliSafeV2
   ```

2. Restaurar dependencias:
   ```bash
   dotnet restore
   ```

3. Configurar base de datos:
   ```bash
   cd IncliSafeApi
   dotnet ef database update
   ```

4. Iniciar la aplicación:
   ```bash
   dotnet run --project IncliSafeApi
   dotnet run --project IncliSafe.Client
   ```

## Arquitectura

### Cliente (Blazor WebAssembly)
- Arquitectura basada en componentes
- Gestión de estado con Fluxor
- Servicios HTTP tipados
- Autenticación JWT
- Componentes MudBlazor

### API (.NET 8)
- Arquitectura en capas
- Repository Pattern
- Unit of Work
- CQRS para operaciones complejas
- Middleware personalizado

### Base de Datos
- SQL Server
- Entity Framework Core
- Migraciones automáticas
- Índices optimizados

## Convenciones de Código

### Nomenclatura
- PascalCase: Clases, interfaces, propiedades públicas
- camelCase: Variables privadas, parámetros
- UPPER_CASE: Constantes
- I-prefix: Interfaces (IService)
- Async-suffix: Métodos asíncronos

### Estructura de Archivos
- Un archivo por clase
- Namespaces coinciden con estructura de carpetas
- DTOs en carpeta Models/DTOs
- Interfaces en carpeta Services/Interfaces

### Patrones
- Repository para acceso a datos
- Factory para creación de objetos
- Strategy para algoritmos variables
- Observer para notificaciones

## Automatización

### Scripts PowerShell
- `scripts/Update-ProjectState.ps1`: Actualiza estado del proyecto
- `scripts/Run-Tests.ps1`: Ejecuta pruebas
- `scripts/Update-Documentation.ps1`: Actualiza documentación

### Documentación Automática
- Estado del proyecto en `docs/state-tracking.md`
- Registro de errores en `docs/error-solutions-log.md`
- Proceso sistemático en `docs/systematic-process.md`

## Seguridad

### Autenticación
- JWT con refresh tokens
- Roles y claims
- Políticas de autorización
- Almacenamiento seguro de tokens

### API
- HTTPS obligatorio
- Validación de modelos
- Sanitización de datos
- Rate limiting
- CORS configurado

### Datos
- Encriptación en reposo
- Auditoría de cambios
- Backups automáticos
- Logs seguros

## Mantenimiento

### Diario
1. Actualizar estado:
   ```powershell
   ./scripts/Update-ProjectState.ps1
   ```

2. Verificar errores:
   ```powershell
   dotnet build
   ```

3. Ejecutar pruebas:
   ```powershell
   ./scripts/Run-Tests.ps1
   ```

### Semanal
1. Revisión de código
2. Actualización de dependencias
3. Optimización de consultas
4. Backup de base de datos

## Contribución
1. Fork del repositorio
2. Crear rama feature/fix
3. Commit con convención
4. Pull request a develop

## Licencia
MIT License

## Contacto
- Email: soporte@inclisafe.com
- Web: https://inclisafe.com