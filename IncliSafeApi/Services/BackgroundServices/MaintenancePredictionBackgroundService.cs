using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using IncliSafeApi.Data;
using IncliSafeApi.Services.Interfaces;

namespace IncliSafeApi.Services.BackgroundServices
{
    public class MaintenancePredictionBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _services;
        private readonly ILogger<MaintenancePredictionBackgroundService> _logger;
        private readonly TimeSpan _predictionInterval = TimeSpan.FromHours(24);

        public MaintenancePredictionBackgroundService(
            IServiceProvider services,
            ILogger<MaintenancePredictionBackgroundService> logger)
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
                    await PredictMaintenanceForAllVehiclesAsync();
                    await Task.Delay(_predictionInterval, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error executing maintenance prediction background service");
                    await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
                }
            }
        }

        private async Task PredictMaintenanceForAllVehiclesAsync()
        {
            using var scope = _services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var predictionService = scope.ServiceProvider.GetRequiredService<IMaintenancePredictionService>();

            var vehicles = await dbContext.Vehiculos
                .Where(v => v.Estado == "Activo")
                .Select(v => v.Id)
                .ToListAsync();

            foreach (var vehicleId in vehicles)
            {
                try
                {
                    await predictionService.PredictMaintenanceAsync(vehicleId);
                    _logger.LogInformation("Completed maintenance prediction for vehicle {VehicleId}", vehicleId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error predicting maintenance for vehicle {VehicleId}", vehicleId);
                }
            }
        }
    }
} 