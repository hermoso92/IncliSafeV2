using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using IncliSafeApi.Services.Interfaces;
using IncliSafe.Shared.Models.DTOs;
using IncliSafe.Shared.Models.Notifications;
using Microsoft.EntityFrameworkCore;
using IncliSafeApi.Data;

namespace IncliSafeApi.Services
{
    public class AlertGenerationService : IAlertGenerationService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AlertGenerationService> _logger;

        public AlertGenerationService(ApplicationDbContext context, ILogger<AlertGenerationService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Alert> SendAlertAsync(Alert alert)
        {
            _context.Alerts.Add(alert);
            await _context.SaveChangesAsync();
            return alert;
        }

        public async Task<List<Alert>> GetAlertsAsync(int userId)
        {
            return await _context.Alerts
                .Where(a => a.UserId == userId)
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

        public async Task<AlertSettings> GetAlertSettingsAsync(int userId)
        {
            return await _context.AlertSettings
                .FirstOrDefaultAsync(s => s.UserId == userId) 
                ?? new AlertSettings { UserId = userId };
        }

        public async Task<AlertSettings> UpdateAlertSettingsAsync(AlertSettings settings)
        {
            var existing = await _context.AlertSettings
                .FirstOrDefaultAsync(s => s.UserId == settings.UserId);

            if (existing == null)
            {
                _context.AlertSettings.Add(settings);
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

        public async Task<VehicleAlertDTO> CreateAlertAsync(int vehicleId, VehicleAlertDTO alert)
        {
            try
            {
                var vehicle = await _vehicleService.GetVehicleSummaryAsync(vehicleId);
                var createdAlert = await _vehicleService.CreateAlertAsync(vehicleId, alert);
                await _notificationService.SendAlertAsync(createdAlert);
                return createdAlert;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating alert for vehicle {VehicleId}", vehicleId);
                throw;
            }
        }
    }
} 