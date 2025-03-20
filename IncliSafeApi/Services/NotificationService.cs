using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IncliSafeApi.Data;
using IncliSafeApi.Hubs;
using IncliSafeApi.Services.Interfaces;
using IncliSafe.Shared.Models;
using System.Security.Claims;
using IncliSafe.Shared.Models.Notifications;
using System.Linq;
using IncliSafe.Shared.Models.Patterns;
using IncliSafe.Shared.Models.Analysis;
using IncliSafe.Shared.Models.DTOs;
using IncliSafe.Shared.Models.Analysis.Core;
using DetectedPattern = IncliSafe.Shared.Models.Patterns.DetectedPattern;
using IncliSafe.Shared.Models.Entities;

namespace IncliSafeApi.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(
            IHubContext<NotificationHub> hubContext,
            ApplicationDbContext context,
            ILogger<NotificationService> logger)
        {
            _hubContext = hubContext;
            _context = context;
            _logger = logger;
        }

        public async Task<Notification> CreateNotificationAsync(Notification notification)
        {
            notification.CreatedAt = DateTime.UtcNow;
            notification.IsRead = false;
            notification.IsActive = true;
            
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            if (notification.UserId.HasValue)
            {
                await _hubContext.Clients.User(notification.UserId.ToString())
                    .SendAsync("ReceiveNotification", notification);
            }

            return notification;
        }

        public async Task<IEnumerable<Notification>> GetUserNotifications(int userId)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Notification>> GetActiveAlerts(int vehicleId)
        {
            return await _context.Notifications
                .Where(n => n.VehicleId == vehicleId && n.IsActive)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<bool> MarkAsReadAsync(int notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);
            if (notification == null) return false;
            
            notification.IsRead = true;
            notification.ReadAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DismissNotificationAsync(int notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);
            if (notification == null) return false;

            notification.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task NotifyPatternDetectedAsync(PatternDetection pattern)
        {
            var notification = new Notification
            {
                VehicleId = pattern.VehicleId,
                Title = $"Patrón Detectado: {pattern.PatternName}",
                Message = pattern.Description,
                Type = NotificationType.PatternDetected,
                Severity = NotificationSeverity.Info,
                CreatedAt = DateTime.UtcNow,
                IsRead = false,
                IsActive = true
            };

            await CreateNotificationAsync(notification);
        }

        public async Task NotifyAnomalyDetectedAsync(Anomaly anomaly)
        {
            var notification = new Notification
            {
                VehicleId = anomaly.VehicleId,
                Title = $"Anomalía Detectada: {anomaly.Name}",
                Message = anomaly.Description,
                Type = NotificationType.AnomalyDetected,
                Severity = MapSeverityToNotificationSeverity(anomaly.Severity),
                CreatedAt = DateTime.UtcNow,
                IsRead = false,
                IsActive = true
            };

            await CreateNotificationAsync(notification);
        }

        private NotificationSeverity MapSeverityToNotificationSeverity(AnomalySeverity severity)
        {
            return severity switch
            {
                AnomalySeverity.Low => NotificationSeverity.Info,
                AnomalySeverity.Medium => NotificationSeverity.Warning,
                AnomalySeverity.High => NotificationSeverity.Error,
                AnomalySeverity.Critical => NotificationSeverity.Critical,
                _ => NotificationSeverity.Info
            };
        }

        public async Task<NotificationSettings> GetNotificationSettings(int vehicleId)
        {
            return await _context.NotificationSettings
                .FirstOrDefaultAsync(s => s.VehicleId == vehicleId) 
                ?? new NotificationSettings
                {
                    VehicleId = vehicleId,
                    EnableEmailNotifications = true,
                    EnablePushNotifications = true,
                    MinimumSeverity = NotificationSeverity.Warning
                };
        }

        public async Task<NotificationSettings> UpdateNotificationSettings(NotificationSettings settings)
        {
            var existingSettings = await _context.NotificationSettings
                .FirstOrDefaultAsync(s => s.VehicleId == settings.VehicleId);

            if (existingSettings == null)
            {
                _context.NotificationSettings.Add(settings);
            }
            else
            {
                _context.Entry(existingSettings).CurrentValues.SetValues(settings);
            }

            await _context.SaveChangesAsync();
            return settings;
        }

        public async Task SendAlertAsync(Alert alert)
        {
            alert.CreatedAt = DateTime.UtcNow;
            alert.IsActive = true;
            
            _context.Alerts.Add(alert);
            await _context.SaveChangesAsync();

            if (alert.UserId.HasValue)
            {
                await _hubContext.Clients.User(alert.UserId.Value.ToString())
                    .SendAsync("ReceiveAlert", alert);
            }
        }

        public async Task SendLicenseExpirationWarningAsync(int userId, LicenseDTO license)
        {
            var notification = new Notification
            {
                UserId = userId,
                Title = "Advertencia de Expiración de Licencia",
                Message = $"La licencia del vehículo expirará el {license.ExpirationDate:d}",
                Type = NotificationType.LicenseExpiration,
                Severity = NotificationSeverity.Warning,
                CreatedAt = DateTime.UtcNow,
                IsRead = false,
                IsActive = true
            };

            await CreateNotificationAsync(notification);
        }

        public async Task SendInspectionReminderAsync(int userId, InspeccionDTO inspeccion)
        {
            var notification = new Notification
            {
                UserId = userId,
                Title = "Recordatorio de Inspección",
                Message = $"La inspección del vehículo está programada para el {inspeccion.FechaProgramada:d}",
                Type = NotificationType.InspectionReminder,
                Severity = NotificationSeverity.Info,
                CreatedAt = DateTime.UtcNow,
                IsRead = false,
                IsActive = true
            };

            await CreateNotificationAsync(notification);
        }

        public async Task SendMaintenanceAlertAsync(int userId, VehicleMaintenanceDTO maintenance)
        {
            var notification = new Notification
            {
                UserId = userId,
                Title = "Alerta de Mantenimiento",
                Message = $"Se requiere mantenimiento: {maintenance.Description}",
                Type = NotificationType.MaintenanceRequired,
                Severity = NotificationSeverity.Warning,
                CreatedAt = DateTime.UtcNow,
                IsRead = false,
                IsActive = true
            };

            await CreateNotificationAsync(notification);
        }

        public async Task<bool> GenerateMaintenanceAlertAsync(int vehicleId, VehicleMaintenanceDTO maintenance)
        {
            try
            {
                var alert = new Alert
                {
                    VehicleId = vehicleId,
                    Title = "Mantenimiento Requerido",
                    Message = maintenance.Description,
                    Type = AlertType.Maintenance,
                    Severity = NotificationSeverity.Warning,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                await SendAlertAsync(alert);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al generar alerta de mantenimiento para el vehículo {VehicleId}", vehicleId);
                return false;
            }
        }

        public async Task<bool> GenerateLicenseExpirationAlertAsync(int vehicleId)
        {
            try
            {
                var vehicle = await _context.Vehiculos
                    .Include(v => v.License)
                    .FirstOrDefaultAsync(v => v.Id == vehicleId);

                if (vehicle?.License == null) return false;

                var daysUntilExpiration = (vehicle.License.ExpirationDate - DateTime.UtcNow).Days;
                if (daysUntilExpiration > 30) return false;

                var alert = new Alert
                {
                    VehicleId = vehicleId,
                    Title = "Expiración de Licencia",
                    Message = $"La licencia expirará en {daysUntilExpiration} días",
                    Type = AlertType.LicenseExpiration,
                    Severity = daysUntilExpiration <= 7 ? NotificationSeverity.Error : NotificationSeverity.Warning,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                await SendAlertAsync(alert);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al generar alerta de expiración de licencia para el vehículo {VehicleId}", vehicleId);
                return false;
            }
        }

        public async Task CheckThresholdsAsync(int vehicleId)
        {
            var settings = await GetNotificationSettings(vehicleId);
            var vehicle = await _context.Vehiculos
                .Include(v => v.LastAnalysis)
                .FirstOrDefaultAsync(v => v.Id == vehicleId);

            if (vehicle?.LastAnalysis == null) return;

            if (vehicle.LastAnalysis.StabilityScore < settings.StabilityThreshold)
            {
                await CreateThresholdAlert(vehicle, "Estabilidad", vehicle.LastAnalysis.StabilityScore);
            }

            if (vehicle.LastAnalysis.SafetyScore < settings.SafetyThreshold)
            {
                await CreateThresholdAlert(vehicle, "Seguridad", vehicle.LastAnalysis.SafetyScore);
            }
        }

        private async Task CreateThresholdAlert(Vehiculo vehicle, string metricName, decimal value)
        {
            var alert = new Alert
            {
                VehicleId = vehicle.Id,
                Title = $"Umbral de {metricName} Excedido",
                Message = $"El valor de {metricName} ({value:P}) está por debajo del umbral establecido",
                Type = AlertType.ThresholdExceeded,
                Severity = NotificationSeverity.Warning,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            await SendAlertAsync(alert);
        }

        public async Task ProcessAlertsAsync()
        {
            var activeAlerts = await _context.Alerts
                .Where(a => a.IsActive)
                .ToListAsync();

            foreach (var alert in activeAlerts)
            {
                if ((DateTime.UtcNow - alert.CreatedAt).TotalDays > 7)
                {
                    alert.IsActive = false;
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task CreateAlertAsync(AnalysisPrediction prediction)
        {
            var alert = new Alert
            {
                VehicleId = prediction.VehicleId,
                Title = $"Predicción: {prediction.Name}",
                Message = prediction.Description ?? "Se ha generado una nueva predicción",
                Type = AlertType.Prediction,
                Severity = MapProbabilityToSeverity(prediction.Probability),
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            await SendAlertAsync(alert);
        }

        private NotificationSeverity MapProbabilityToSeverity(decimal probability)
        {
            return probability switch
            {
                >= 0.9M => NotificationSeverity.Critical,
                >= 0.7M => NotificationSeverity.Error,
                >= 0.5M => NotificationSeverity.Warning,
                _ => NotificationSeverity.Info
            };
        }
    }
} 