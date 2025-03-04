using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using IncliSafeApi.Data;

namespace IncliSafeApi.Services.BackgroundServices
{
    public class AlertGenerationBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _services;
        private readonly ILogger<AlertGenerationBackgroundService> _logger;
        private readonly TimeSpan _checkInterval = TimeSpan.FromHours(24);

        public AlertGenerationBackgroundService(
            IServiceProvider services,
            ILogger<AlertGenerationBackgroundService> logger)
        {
            _services = services;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await GenerateAlertsAsync();
                    await Task.Delay(_checkInterval, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error executing alert generation background service");
                    await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
                }
            }
        }

        private async Task GenerateAlertsAsync()
        {
            using var scope = _services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var alertService = scope.ServiceProvider.GetRequiredService<IAlertGenerationService>();

            var vehicles = await dbContext.Vehiculos
                .Where(v => v.Estado == "Activo")
                .Select(v => v.Id)
                .ToListAsync();

            foreach (var vehicleId in vehicles)
            {
                try
                {
                    await alertService.GenerateInspectionAlertAsync(vehicleId);
                    await alertService.GenerateMaintenanceAlertAsync(vehicleId);
                    await alertService.GenerateLicenseExpirationAlertAsync(vehicleId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error generating alerts for vehicle {VehicleId}", vehicleId);
                }
            }
        }
    }
} 