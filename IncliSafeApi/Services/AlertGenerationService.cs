using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using IncliSafeApi.Services.Interfaces;
using IncliSafe.Shared.Models.DTOs;
using IncliSafe.Shared.Models.Notifications;
using Microsoft.EntityFrameworkCore;
using IncliSafeApi.Data;
using IncliSafe.Shared.Models.Entities;
using IncliSafe.Shared.Models.Enums;
using IncliSafe.Shared.Models.Analysis.Core;

namespace IncliSafeApi.Services
{
    public class AlertGenerationService : IAlertGenerationService
    {
        private readonly IVehicleService _vehicleService;
        private readonly INotificationService _notificationService;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AlertGenerationService> _logger;

        public AlertGenerationService(
            IVehicleService vehicleService,
            INotificationService notificationService,
            ApplicationDbContext context,
            ILogger<AlertGenerationService> logger)
        {
            _vehicleService = vehicleService;
            _notificationService = notificationService;
            _context = context;
            _logger = logger;
        }

        public async Task<VehicleAlertDTO> CreateAlertAsync(int vehicleId, VehicleAlertDTO alert)
        {
            var entity = new VehicleAlert
            {
                VehicleId = vehicleId,
                Title = alert.Title,
                Message = alert.Message,
                Type = alert.Type,
                Severity = alert.Severity,
                CreatedAt = DateTime.UtcNow
            };

            _context.VehicleAlerts.Add(entity);
            await _context.SaveChangesAsync();
            await _notificationService.SendAlertAsync(new Alert
            {
                VehicleId = vehicleId,
                Title = alert.Title,
                Message = alert.Message,
                Type = alert.Type,
                Severity = alert.Severity,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            });
            return alert;
        }

        public async Task<List<Alert>> GetAlertsAsync(int userId)
        {
            return await _context.Alerts
                .Where(a => a.UserId == userId.ToString())
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }

        public async Task<bool> MarkAsReadAsync(int alertId)
        {
            var alert = await _context.Alerts.FindAsync(alertId);
            if (alert == null) return false;

            alert.ReadAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAlertAsync(int alertId)
        {
            var alert = await _context.Alerts.FindAsync(alertId);
            if (alert == null) return false;

            _context.Alerts.Remove(alert);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<NotificationSettings> GetAlertSettingsAsync(int userId)
        {
            return await _context.NotificationSettings
                .FirstOrDefaultAsync(s => s.VehicleId == userId) 
                ?? new NotificationSettings { VehicleId = userId };
        }

        public async Task<NotificationSettings> UpdateAlertSettingsAsync(NotificationSettings settings)
        {
            var existing = await _context.NotificationSettings
                .FirstOrDefaultAsync(s => s.VehicleId == settings.VehicleId);

            if (existing == null)
            {
                _context.NotificationSettings.Add(settings);
            }
            else
            {
                _context.Entry(existing).CurrentValues.SetValues(settings);
            }

            await _context.SaveChangesAsync();
            return settings;
        }

        public async Task GenerateInspectionAlertAsync(int vehicleId)
        {
            try
            {
                var vehicle = await _vehicleService.GetVehicleSummaryAsync(vehicleId);
                if (vehicle.RequiereInspeccion)
                {
                    var alert = new VehicleAlertDTO
                    {
                        VehicleId = vehicleId,
                        Title = "Inspección Requerida",
                        Message = $"El vehículo {vehicle.Nombre} requiere una inspección programada.",
                        Type = AlertType.Inspection,
                        Severity = AlertSeverity.Warning
                    };

                    var createdAlert = await _vehicleService.CreateAlertAsync(vehicleId, alert);
                    await _notificationService.SendAlertAsync(vehicle.UserId.ToString(), createdAlert);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating inspection alert for vehicle {VehicleId}", vehicleId);
            }
        }

        public async Task GenerateMaintenanceAlertAsync(int vehicleId)
        {
            try
            {
                var vehicle = await _vehicleService.GetVehicleSummaryAsync(vehicleId);
                if (vehicle.RequiereMantenimiento)
                {
                    var alert = new VehicleAlertDTO
                    {
                        VehicleId = vehicleId,
                        Title = "Mantenimiento Requerido",
                        Message = $"El vehículo {vehicle.Nombre} requiere mantenimiento basado en sus últimos análisis.",
                        Type = AlertType.Maintenance,
                        Severity = AlertSeverity.Warning
                    };

                    var createdAlert = await _vehicleService.CreateAlertAsync(vehicleId, alert);
                    await _notificationService.SendAlertAsync(vehicle.UserId.ToString(), createdAlert);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating maintenance alert for vehicle {VehicleId}", vehicleId);
            }
        }

        public async Task GenerateLicenseExpirationAlertAsync(int vehicleId)
        {
            try
            {
                var vehicle = await _vehicleService.GetVehicleSummaryAsync(vehicleId);
                var license = await _vehicleService.GetLicenseAsync(vehicleId);

                if (license != null && license.DaysUntilExpiration <= 30)
                {
                    var severity = license.DaysUntilExpiration <= 7 ? AlertSeverity.Critical : AlertSeverity.Warning;
                    var alert = new VehicleAlertDTO
                    {
                        VehicleId = vehicleId,
                        Title = "Licencia por Expirar",
                        Message = $"La licencia del vehículo {vehicle.Nombre} expirará en {license.DaysUntilExpiration} días.",
                        Type = AlertType.License,
                        Severity = severity
                    };

                    var createdAlert = await _vehicleService.CreateAlertAsync(vehicleId, alert);
                    await _notificationService.SendAlertAsync(vehicle.UserId.ToString(), createdAlert);
                    await _notificationService.SendLicenseExpirationWarningAsync(vehicle.UserId.ToString(), license);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating license expiration alert for vehicle {VehicleId}", vehicleId);
            }
        }

        public async Task GenerateSafetyAlertAsync(int vehicleId, decimal safetyScore)
        {
            try
            {
                var vehicle = await _vehicleService.GetVehicleSummaryAsync(vehicleId);
                if (safetyScore < 0.7M)
                {
                    var alert = new VehicleAlertDTO
                    {
                        VehicleId = vehicleId,
                        Title = "Alerta de Seguridad",
                        Message = $"El vehículo {vehicle.Nombre} ha registrado un índice de seguridad bajo ({safetyScore:P0}).",
                        Type = AlertType.Safety,
                        Severity = safetyScore < 0.5M ? AlertSeverity.Critical : AlertSeverity.Warning
                    };

                    var createdAlert = await _vehicleService.CreateAlertAsync(vehicleId, alert);
                    await _notificationService.SendAlertAsync(vehicle.UserId.ToString(), createdAlert);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating safety alert for vehicle {VehicleId}", vehicleId);
            }
        }

        public async Task<bool> CheckThresholdsAsync(int vehicleId)
        {
            try
            {
                var vehicle = await _context.Vehicles
                    .Include(v => v.SensorReadings)
                    .FirstOrDefaultAsync(v => v.Id == vehicleId);

                if (vehicle == null)
                {
                    _logger.LogWarning("No se encontró el vehículo {VehicleId}", vehicleId);
                    return false;
                }

                // Implementar lógica de verificación de umbrales
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al verificar umbrales para el vehículo {VehicleId}", vehicleId);
                return false;
            }
        }

        public async Task<bool> ProcessAlertsAsync(int vehicleId)
        {
            try
            {
                var alerts = await _context.VehicleAlerts
                    .Where(a => a.VehicleId == vehicleId && !a.IsProcessed)
                    .ToListAsync();

                foreach (var alert in alerts)
                {
                    alert.IsProcessed = true;
                    alert.ProcessedDate = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar alertas para el vehículo {VehicleId}", vehicleId);
                return false;
            }
        }
    }
} 