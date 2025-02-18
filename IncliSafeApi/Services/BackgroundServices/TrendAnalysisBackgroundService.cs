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
    public class TrendAnalysisBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _services;
        private readonly ILogger<TrendAnalysisBackgroundService> _logger;
        private readonly TimeSpan _analysisInterval = TimeSpan.FromHours(24);

        public TrendAnalysisBackgroundService(
            IServiceProvider services,
            ILogger<TrendAnalysisBackgroundService> logger)
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
                    await AnalyzeAllVehicleTrendsAsync();
                    await Task.Delay(_analysisInterval, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error executing trend analysis background service");
                    await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
                }
            }
        }

        private async Task AnalyzeAllVehicleTrendsAsync()
        {
            using var scope = _services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var trendService = scope.ServiceProvider.GetRequiredService<ITrendAnalysisService>();

            var vehicles = await dbContext.Vehiculos
                .Where(v => v.Estado == "Activo")
                .Select(v => v.Id)
                .ToListAsync();

            foreach (var vehicleId in vehicles)
            {
                try
                {
                    await trendService.AnalyzeVehicleTrendsAsync(vehicleId);
                    _logger.LogInformation("Completed trend analysis for vehicle {VehicleId}", vehicleId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error analyzing trends for vehicle {VehicleId}", vehicleId);
                }
            }
        }
    }
} 