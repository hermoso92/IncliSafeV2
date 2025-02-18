using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using IncliSafeApi.Data;
using IncliSafeApi.Services;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;
using IncliSafeApi.Services.Interfaces;
using IncliSafeApi.Hubs;
using IncliSafeApi.Services.BackgroundServices;

var builder = WebApplication.CreateBuilder(args);

// Agregar configuración de logging
builder.Services.AddLogging(logging =>
{
    logging.AddConsole();
    logging.AddDebug();
});

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "IncliSafe API", Version = "v1" });
    
    // Configurar la autenticación JWT en Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Registrar servicios
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IKnowledgeBaseService, KnowledgeBaseService>();
builder.Services.AddScoped<IDobackService, DobackService>();
builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddScoped<ILicenseService, LicenseService>();

// Registrar servicios de alertas
builder.Services.AddScoped<IAlertGenerationService, AlertGenerationService>();
builder.Services.AddHostedService<AlertGenerationBackgroundService>();

// Registrar servicios de notificación en tiempo real
builder.Services.AddSignalR();
builder.Services.AddScoped<IRealTimeNotificationService, RealTimeNotificationService>();

// Registrar servicios de métricas
builder.Services.AddScoped<IVehicleMetricsService, VehicleMetricsService>();

// Registrar servicios de análisis de tendencias
builder.Services.AddScoped<ITrendAnalysisService, TrendAnalysisService>();
builder.Services.AddHostedService<TrendAnalysisBackgroundService>();

// Registrar servicios de predicción de mantenimiento
builder.Services.AddScoped<IMaintenancePredictionService, MaintenancePredictionService>();
builder.Services.AddHostedService<MaintenancePredictionBackgroundService>();

// Configurar CORS con opciones más específicas
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
        builder.WithOrigins("https://localhost:7268") // URL del cliente Blazor
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials());
});

// Configurar DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar servicios
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IKnowledgeBaseService, KnowledgeBaseService>();

// Configurar autenticación con más opciones de seguridad
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"] ?? 
                throw new InvalidOperationException("JWT Secret Key no configurada"))),
        ClockSkew = TimeSpan.Zero
    };
    
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
            {
                context.Response.Headers["Token-Expired"] = "true";
            }
            return Task.CompletedTask;
        }
    };
});

// Validar configuración crítica
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
if (string.IsNullOrEmpty(jwtSettings?.SecretKey) || jwtSettings.SecretKey.Length < 32)
{
    throw new InvalidOperationException("JWT SecretKey debe tener al menos 32 caracteres");
}

if (string.IsNullOrEmpty(jwtSettings.Issuer) || string.IsNullOrEmpty(jwtSettings.Audience))
{
    throw new InvalidOperationException("JWT Issuer y Audience son requeridos");
}

// Validar la cadena de conexión
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("La cadena de conexión DefaultConnection es requerida");
}

var app = builder.Build();

// Asegurar que la base de datos existe y está actualizada
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();
        DbInitializer.Initialize(context); // Inicializar datos
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Error al inicializar la base de datos.");
        throw;
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "IncliSafe API v1");
        c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
    });
    
    // Agregar esto para ver las rutas registradas
    app.MapGet("/debug/routes", (IEnumerable<EndpointDataSource> endpointSources) =>
        string.Join("\n", endpointSources.SelectMany(source => source.Endpoints)));
}

// Comentar o eliminar esta línea en desarrollo
//app.UseHttpsRedirection();

app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Configurar endpoints de SignalR
app.MapHub<NotificationHub>("/hubs/notification");

app.Run();
