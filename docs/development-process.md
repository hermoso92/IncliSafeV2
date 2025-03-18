# Proceso de Desarrollo IncliSafe

## Flujo de Trabajo

### 1. Inicio de Sesión
```powershell
# 1. Actualizar estado del proyecto
./scripts/Update-ProjectState.ps1

# 2. Verificar errores pendientes
Get-Content docs/state-tracking.md

# 3. Actualizar dependencias
dotnet restore
```

### 2. Desarrollo
1. Crear rama feature/fix:
   ```bash
   git checkout -b feature/nombre-caracteristica
   ```

2. Implementar cambios siguiendo:
   - Convenciones de código
   - Patrones establecidos
   - Tests unitarios

3. Verificar cambios:
   ```powershell
   # Compilar
   dotnet build

   # Ejecutar tests
   dotnet test

   # Actualizar estado
   ./scripts/Update-ProjectState.ps1
   ```

### 3. Revisión de Código
1. Auto-revisión:
   - Convenciones cumplidas
   - Tests pasando
   - Sin warnings/errores
   - Documentación actualizada

2. Pull Request:
   - Descripción clara
   - Screenshots si aplica
   - Tests incluidos
   - Documentación actualizada

## Estándares de Código

### 1. Arquitectura
```csharp
// 1. Capa de Presentación (Blazor Components)
public class MyComponent : ComponentBase
{
    [Inject] private IMyService Service { get; set; } = null!;
}

// 2. Capa de Servicios
public interface IMyService
{
    Task<Result<T>> GetDataAsync();
}

// 3. Capa de Acceso a Datos
public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(int id);
}
```

### 2. Patrones Comunes
```csharp
// Repository Pattern
public class Repository<T> : IRepository<T> where T : class
{
    private readonly ApplicationDbContext _context;
    
    public Repository(ApplicationDbContext context)
    {
        _context = context;
    }
}

// Unit of Work
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    
    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }
}

// Service Pattern
public class MyService : IMyService
{
    private readonly IUnitOfWork _unitOfWork;
    
    public MyService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
}
```

### 3. Manejo de Errores
```csharp
// Result Pattern
public class Result<T>
{
    public bool Success { get; }
    public T? Data { get; }
    public string? Error { get; }
}

// Uso en servicios
public async Task<Result<T>> GetDataAsync()
{
    try
    {
        var data = await _repository.GetAsync();
        return Result<T>.Success(data);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error getting data");
        return Result<T>.Error("Error interno del servidor");
    }
}
```

## Optimización

### 1. Entity Framework
```csharp
// Consultas optimizadas
public async Task<List<T>> GetAllAsync()
{
    return await _context.Set<T>()
        .AsNoTracking()  // Mejor rendimiento
        .ToListAsync();
}

// Includes explícitos
public async Task<T?> GetByIdWithDetailsAsync(int id)
{
    return await _context.Set<T>()
        .Include(x => x.RelatedEntity)
        .FirstOrDefaultAsync(x => x.Id == id);
}
```

### 2. Blazor
```csharp
// Evitar renders innecesarios
protected override bool ShouldRender()
{
    return _shouldRender;
}

// Usar EventCallback
[Parameter]
public EventCallback<T> OnChange { get; set; }

// Memoización de valores
private readonly Func<T, TResult> _memoizedFunction;
```

### 3. API
```csharp
// Caché
[ResponseCache(Duration = 60)]
public async Task<IActionResult> GetData()

// Compresión
app.UseResponseCompression();

// Rate limiting
services.AddRateLimiting(options => {
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: context.User.Identity?.Name ?? context.Request.Headers.Host.ToString(),
            factory: partition => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 10,
                QueueLimit = 0,
                Window = TimeSpan.FromSeconds(1)
            }));
});
```

## Testing

### 1. Unit Tests
```csharp
public class MyServiceTests
{
    private readonly Mock<IRepository<T>> _mockRepo;
    private readonly MyService _service;

    public MyServiceTests()
    {
        _mockRepo = new Mock<IRepository<T>>();
        _service = new MyService(_mockRepo.Object);
    }

    [Fact]
    public async Task GetData_ShouldReturnData()
    {
        // Arrange
        _mockRepo.Setup(x => x.GetAsync())
            .ReturnsAsync(new List<T>());

        // Act
        var result = await _service.GetDataAsync();

        // Assert
        Assert.True(result.Success);
    }
}
```

### 2. Integration Tests
```csharp
public class IntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public IntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetEndpoint_ShouldReturnSuccess()
    {
        // Act
        var response = await _client.GetAsync("/api/data");

        // Assert
        response.EnsureSuccessStatusCode();
    }
}
```

## Seguridad

### 1. Autenticación
```csharp
// JWT Configuration
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = Configuration["Jwt:Issuer"],
            ValidAudience = Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
        };
    });
```

### 2. Autorización
```csharp
[Authorize(Roles = "Admin")]
[HttpPost]
public async Task<IActionResult> CreateData([FromBody] T data)

[Authorize(Policy = "RequireAdminRole")]
public class AdminController : ControllerBase
```

### 3. Validación
```csharp
public class DataValidator : AbstractValidator<T>
{
    public DataValidator()
    {
        RuleFor(x => x.Property)
            .NotEmpty()
            .MaximumLength(100);
    }
}
```

## Mantenimiento

### 1. Logs
```csharp
public class LoggingBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling {RequestName}", typeof(TRequest).Name);
        var response = await next();
        _logger.LogInformation("Handled {RequestName}", typeof(TRequest).Name);
        return response;
    }
}
```

### 2. Monitoreo
```csharp
// Health Checks
services.AddHealthChecks()
    .AddDbContextCheck<ApplicationDbContext>()
    .AddUrlGroup(new Uri(Configuration["ExternalService:Url"]))
    .AddCheck<CustomHealthCheck>("Custom");

// Métricas
services.AddMetrics()
    .AddPrometheusFormatter()
    .AddHealthChecks();
```

### 3. Backup
```powershell
# Backup script
$date = Get-Date -Format "yyyy-MM-dd"
$backupPath = "backups/$date"

# Database backup
Backup-SqlDatabase -ServerInstance "." -Database "IncliSafe" -BackupFile "$backupPath/db.bak"

# Files backup
Copy-Item -Path "data/*" -Destination "$backupPath/files" -Recurse
```

## Documentación

### 1. API
```csharp
/// <summary>
/// Obtiene datos paginados
/// </summary>
/// <param name="page">Número de página</param>
/// <param name="pageSize">Tamaño de página</param>
/// <returns>Lista paginada de datos</returns>
[ProducesResponseType(typeof(PagedResult<T>), 200)]
[ProducesResponseType(typeof(ErrorResponse), 400)]
public async Task<IActionResult> GetPaged(int page, int pageSize)
```

### 2. Componentes
```csharp
/// <summary>
/// Componente de tabla con paginación
/// </summary>
public partial class DataTable<T>
{
    /// <summary>
    /// Datos a mostrar
    /// </summary>
    [Parameter]
    public IEnumerable<T> Data { get; set; } = null!;

    /// <summary>
    /// Evento al cambiar página
    /// </summary>
    [Parameter]
    public EventCallback<int> OnPageChange { get; set; }
}
``` 