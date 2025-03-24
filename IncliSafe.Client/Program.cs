using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Authorization;
using IncliSafe.Client;
using IncliSafe.Client.Auth;
using IncliSafe.Client.Services;
using MudBlazor;
using MudBlazor.Services;
using Blazored.LocalStorage;
using IncliSafe.Client.Services.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.Components;
using IncliSafe.Shared.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Cargar configuración antes de cualquier otro servicio
builder.Configuration.AddJsonFile("appsettings.json", optional: false);
builder.Configuration.AddJsonFile($"appsettings.{builder.HostEnvironment.Environment}.json", optional: true);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Servicios fundamentales
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopRight;
    config.SnackbarConfiguration.PreventDuplicates = false;
    config.SnackbarConfiguration.NewestOnTop = false;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 3000;
    config.SnackbarConfiguration.HideTransitionDuration = 500;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
});

// Configuración
builder.Services.Configure<AppSettings>(builder.Configuration);

// Autenticación
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

// Handlers y servicios base
builder.Services.AddScoped<AuthenticationHttpMessageHandler>();
builder.Services.AddScoped<HttpErrorInterceptor>();
builder.Services.AddScoped<CacheService>();
builder.Services.AddScoped<StateContainer>();

// HttpClient
builder.Services.AddScoped(sp => 
{
    var authHandler = sp.GetRequiredService<AuthenticationHttpMessageHandler>();
    var errorHandler = sp.GetRequiredService<HttpErrorInterceptor>();
    
    // Encadenar los handlers
    authHandler.InnerHandler = errorHandler;
    errorHandler.InnerHandler = new HttpClientHandler();
    
    return new HttpClient(authHandler)
    {
        BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"] ?? "http://localhost:5000/")
    };
});

// Servicios de la aplicación
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<VehiculoService>();
builder.Services.AddScoped<InspeccionService>();
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<IKnowledgeBaseService, KnowledgeBaseService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddScoped<IDobackAnalysisService, DobackAnalysisService>();
builder.Services.AddScoped<HubConnection>(sp => {
    var navigationManager = sp.GetRequiredService<NavigationManager>();
    return new HubConnectionBuilder()
        .WithUrl(navigationManager.ToAbsoluteUri("/notificationHub"))
        .WithAutomaticReconnect()
        .Build();
});

// Después de cargar la configuración
var appSettings = builder.Configuration.Get<AppSettings>();
if (appSettings == null)
{
    throw new InvalidOperationException("No se pudo cargar la configuración de la aplicación");
}

if (string.IsNullOrEmpty(appSettings.ApiBaseUrl))
{
    throw new InvalidOperationException("La URL de la API no está configurada");
}

builder.Logging.SetMinimumLevel(LogLevel.Debug);
builder.Logging.AddFilter("Microsoft.AspNetCore.Components", LogLevel.Debug);

// Después de los servicios fundamentales
builder.Services.AddScoped<ErrorBoundary>();

// Agregar después de los servicios de MudBlazor
builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopRight;
    config.SnackbarConfiguration.PreventDuplicates = false;
    config.SnackbarConfiguration.NewestOnTop = false;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 3000;
    config.SnackbarConfiguration.HideTransitionDuration = 500;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
});

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<INotificationService, NotificationService>();

var host = builder.Build();

// Asegurarse que los servicios estén inicializados
await host.RunAsync();
