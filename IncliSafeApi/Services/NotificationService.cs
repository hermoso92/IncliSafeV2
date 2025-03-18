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

        public async Task SendAlertAsync(Alert alert)
        {
            alert.CreatedAt = DateTime.UtcNow;
            alert.IsActive = true;
            
            _context.Alerts.Add(alert);
            await _context.SaveChangesAsync();

            if (alert.UserId != null)
            {
                await _hubContext.Clients.User(alert.UserId)
                    .SendAsync("ReceiveAlert", alert);
            }
        }

        public async Task MarkAlertAsReadAsync(int alertId)
        {
            var alert = await _context.Alerts.FindAsync(alertId);
            if (alert != null)
            {
                alert.ReadAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<AlertSettings> GetAlertSettingsAsync(int vehicleId)
        {
            var settings = await _context.AlertSettings
                .FirstOrDefaultAsync(s => s.VehicleId == vehicleId);

            return settings ?? new AlertSettings
            {
                VehicleId = vehicleId,
                StabilityThreshold = 0.7M,
                SafetyThreshold = 0.7M,
                MinimumSeverity = NotificationSeverity.Warning,
                EnableEmailNotifications = true,
                EnablePushNotifications = true
            };
        }

        public async Task SendVehicleAlertAsync(int vehicleId, string title, string message, NotificationSeverity severity)
        {
            var alert = new Alert
            {
                VehicleId = vehicleId,
                Title = title,
                Message = message,
                Severity = severity,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            await CreateAlertAsync(alert);
        }

        public async Task SendPatternDetectedAsync(int vehicleId, DetectedPattern pattern)
        {
            var notification = new Notification
            {
                UserId = vehicleId,
                Title = $"Patrón Detectado: {pattern.PatternName}",
                Message = pattern.Description,
                Type = NotificationType.PatternDetected,
                Severity = NotificationSeverity.Info,
                CreatedAt = DateTime.UtcNow,
                IsRead = false
            };

            await CreateNotificationAsync(notification);
        }

        public async Task MarkAsReadAsync(int notificationId, int userId)
        {
            var notification = await _context.Notifications
                .FirstOrDefaultAsync(n => n.Id == notificationId && n.UserId == userId);

            if (notification != null)
            {
                notification.IsRead = true;
                notification.ReadAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task SendNotificationAsync(string userId, string message)
        {
            var notification = new Notification
            {
                UserId = int.Parse(userId),
                Title = "Notificación",
                Message = message,
                Severity = NotificationSeverity.Info,
                CreatedAt = DateTime.UtcNow,
                Type = NotificationType.Info,
                IsRead = false
            };

            await CreateNotificationAsync(notification);
            await _hubContext.Clients.User(userId)
                .SendAsync("ReceiveNotification", notification);
        }

        private async Task<bool> ValidateUserId(int userId, int numericId)
        {
            var userExists = await _context.Usuarios.AnyAsync(u => u.Id == userId);
            return userExists && userId == numericId;
        }

        public async Task<List<Notification>> GetNotificationsAsync(int userId)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<Notification?> GetNotificationAsync(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification == null)
            {
                _logger.LogWarning("Notification with ID {Id} not found", id);
                return null;
            }
            return notification;
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

        public async Task<bool> DeleteNotificationAsync(int notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);
            if (notification == null) return false;

            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();
            return true;
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

        public async Task<List<Alert>> GetActiveAlertsAsync(int vehicleId)
        {
            return await _context.Alerts
                .Where(a => a.VehicleId == vehicleId && a.IsActive)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }

        public async Task<Alert> CreateAlertAsync(Alert alert)
        {
            alert.CreatedAt = DateTime.UtcNow;
            alert.IsActive = true;

            _context.Alerts.Add(alert);
            await _context.SaveChangesAsync();

            await NotifyUserAsync(alert);
            return alert;
        }

        private async Task NotifyUserAsync(Alert alert)
        {
            if (alert.UserId.HasValue)
            {
                await _hubContext.Clients.User(alert.UserId.Value.ToString())
                    .SendAsync("ReceiveAlert", alert);
            }
        }

        public async Task<bool> DismissAlertAsync(int alertId)
        {
            var alert = await _context.Alerts.FindAsync(alertId);
            if (alert == null) return false;

            alert.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAlertSettingsAsync(int vehicleId, AlertSettings settings)
        {
            var vehiculo = await _context.Vehiculos.FindAsync(vehicleId);
            if (vehiculo == null) return false;

            vehiculo.NotificationSettings = new NotificationSettings
            {
                EnableNotifications = true,
                StabilityThreshold = settings.StabilityThreshold ?? 0.7,
                SafetyThreshold = settings.SafetyThreshold ?? 0.8,
                MinimumSeverity = settings.MinimumSeverity ?? NotificationSeverity.Warning
            };
            
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Alert> UpdateAlertAsync(Alert alert)
        {
            _context.Alerts.Update(alert);
            await _context.SaveChangesAsync();
            return alert;
        }

        public async Task<List<Notification>> GetUserNotificationsAsync(int userId)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task NotifyPatternDetectedAsync(PatternDetection pattern)
        {
            var notification = new Notification
            {
                Title = $"Patrón Detectado: {pattern.PatternName}",
                Message = pattern.Description,
                Type = NotificationType.PatternDetected,
                Severity = NotificationSeverity.Info,
                VehicleId = pattern.VehicleId
            };

            await CreateNotificationAsync(notification);
        }

        public async Task NotifyAnomalyDetectedAsync(Anomaly anomaly)
        {
            var notification = new Notification
            {
                Title = $"Anomalía Detectada: {anomaly.Type}",
                Message = anomaly.Description,
                Type = NotificationType.Anomaly,
                Severity = NotificationSeverity.Warning,
                VehicleId = anomaly.VehicleId
            };

            await CreateNotificationAsync(notification);
        }

        public async Task<bool> UpdateNotificationSettingsAsync(int vehicleId, NotificationSettings settings)
        {
            var vehiculo = await _context.Vehiculos.FindAsync(vehicleId);
            if (vehiculo == null) return false;

            settings.StabilityThreshold = Convert.ToDecimal(settings.StabilityThreshold);
            settings.SafetyThreshold = Convert.ToDecimal(settings.SafetyThreshold);
            settings.MinimumSeverity = settings.MinimumSeverity;

            vehiculo.NotificationSettings = settings;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Alert> CreateAlert(string message, NotificationSeverity severity, int vehicleId)
        {
            var alert = new Alert
            {
                Message = message,
                Severity = severity,
                VehicleId = vehicleId,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            _context.Alerts.Add(alert);
            await _context.SaveChangesAsync();
            return alert;
        }

        public async Task<Notification> CreateNotification(string title, string message, NotificationType type, int vehicleId, int? userId = null)
        {
            var notification = new Notification
            {
                Title = title,
                Message = message,
                Type = type,
                VehicleId = vehicleId,
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                IsRead = false
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
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

        public async Task<NotificationSettings> GetNotificationSettings(int vehicleId)
        {
            return await _context.NotificationSettings
                .FirstOrDefaultAsync(s => s.VehicleId == vehicleId) 
                ?? new NotificationSettings { VehicleId = vehicleId };
        }

        public async Task<NotificationSettings> UpdateNotificationSettings(NotificationSettings settings)
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

        public async Task<bool> DismissNotificationAsync(int notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);
            if (notification == null) return false;

            notification.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<Alert> CreateAlert(string message, NotificationSeverity severity, string userId)
        {
            var alert = new Alert
            {
                Title = severity.ToString(),
                Message = message,
                Severity = severity.ToString(),
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                UserId = userId
            };

            _context.Alerts.Add(alert);
            await _context.SaveChangesAsync();
            return alert;
        }

        public async Task<bool> SendAlert(string message, NotificationSeverity severity, string userId)
        {
            try
            {
                var alert = await CreateAlert(message, severity, userId);
                await _hubContext.Clients.User(userId).SendAsync("ReceiveAlert", alert);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task SendLicenseExpirationWarningAsync(int userId, LicenseDTO license)
        {
            var notification = new Notification
            {
                UserId = userId,
                Title = "Licencia por expirar",
                Message = $"La licencia expirará el {license.ExpirationDate:d}",
                Type = NotificationType.Warning,
                CreatedAt = DateTime.UtcNow
            };
            await CreateNotificationAsync(notification);
        }

        public async Task SendInspectionReminderAsync(int userId, InspeccionDTO inspeccion)
        {
            var notification = new Notification
            {
                UserId = userId,
                Title = "Recordatorio de inspección",
                Message = $"Inspección programada para {inspeccion.Fecha:d}",
                Type = NotificationType.Info,
                CreatedAt = DateTime.UtcNow
            };
            await CreateNotificationAsync(notification);
        }

        public async Task SendMaintenanceAlertAsync(int userId, VehicleMaintenanceDTO maintenance)
        {
            var notification = new Notification
            {
                UserId = userId,
                Title = "Alerta de mantenimiento",
                Message = $"Mantenimiento requerido: {maintenance.Description}",
                Type = NotificationType.Warning,
                CreatedAt = DateTime.UtcNow
            };
            await CreateNotificationAsync(notification);
        }

        public async Task ProcessPattern(DetectedPattern pattern)
        {
            // Implementation of the method
        }

        private Alert CreateAlertFromNotification(Notification notification)
        {
            return new Alert
            {
                Title = notification.Title,
                Message = notification.Message,
                Severity = notification.Severity,
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                UserId = notification.UserId ?? 0,
                Category = notification.Category,
                Type = MapNotificationTypeToAlertType(notification.Type),
                Priority = MapSeverityToPriority(notification.Severity)
            };
        }

        private AlertType MapNotificationTypeToAlertType(NotificationType type)
        {
            return type switch
            {
                NotificationType.Maintenance => AlertType.Maintenance,
                NotificationType.Safety => AlertType.Safety,
                NotificationType.Vehicle => AlertType.Vehicle,
                _ => AlertType.System
            };
        }

        private AlertPriority MapSeverityToPriority(NotificationSeverity severity)
        {
            return severity switch
            {
                NotificationSeverity.Critical => AlertPriority.High,
                NotificationSeverity.Warning => AlertPriority.Medium,
                _ => AlertPriority.Low
            };
        }

        public async Task<bool> GenerateMaintenanceAlertAsync(int vehicleId, VehicleMaintenanceDTO maintenance)
        {
            try
            {
                var vehicle = await _context.Vehiculos.FindAsync(vehicleId)
                    ?? throw new InvalidOperationException($"Vehicle with ID {vehicleId} not found.");

                var alert = new Alert
                {
                    VehicleId = vehicleId,
                    Type = AlertType.Maintenance,
                    Severity = AlertSeverity.Warning,
                    Title = $"Mantenimiento programado para {vehicle.Placa}",
                    Message = $"El vehículo {vehicle.Placa} tiene un mantenimiento programado para {maintenance.FechaProgramada:d}",
                    CreatedAt = DateTime.UtcNow,
                    IsRead = false,
                    IsActive = true
                };

                _context.Alerts.Add(alert);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating maintenance alert for vehicle {Id}", vehicleId);
                return false;
            }
        }

        public async Task<bool> GenerateLicenseExpirationAlertAsync(int vehicleId)
        {
            try
            {
                var vehicle = await _context.Vehiculos.FindAsync(vehicleId)
                    ?? throw new InvalidOperationException($"Vehicle with ID {vehicleId} not found.");

                if (vehicle.FechaVencimientoLicencia <= DateTime.Now.AddDays(30))
                {
                    var alert = new Alert
                    {
                        VehicleId = vehicleId,
                        Type = AlertType.License,
                        Severity = AlertSeverity.Warning,
                        Title = $"Licencia próxima a vencer para {vehicle.Placa}",
                        Message = $"La licencia del vehículo {vehicle.Placa} vence el {vehicle.FechaVencimientoLicencia:d}",
                        CreatedAt = DateTime.UtcNow,
                        IsRead = false,
                        IsActive = true
                    };

                    _context.Alerts.Add(alert);
                    await _context.SaveChangesAsync();

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating license expiration alert for vehicle {Id}", vehicleId);
                return false;
            }
        }

        public async Task CheckThresholdsAsync(int vehicleId)
        {
            try
            {
                var vehicle = await _context.Vehiculos.FindAsync(vehicleId)
                    ?? throw new InvalidOperationException($"Vehicle with ID {vehicleId} not found.");

                var latestAnalysis = await _context.DobackAnalyses
                    .Where(a => a.VehicleId == vehicleId)
                    .OrderByDescending(a => a.AnalysisDate)
                    .FirstOrDefaultAsync();

                if (latestAnalysis != null)
                {
                    if (latestAnalysis.StabilityScore < 0.6m)
                    {
                        await CreateThresholdAlert(vehicle, "Estabilidad", latestAnalysis.StabilityScore);
                    }

                    if (latestAnalysis.SafetyScore < 0.7m)
                    {
                        await CreateThresholdAlert(vehicle, "Seguridad", latestAnalysis.SafetyScore);
                    }

                    if (latestAnalysis.MaintenanceScore < 0.65m)
                    {
                        await CreateThresholdAlert(vehicle, "Mantenimiento", latestAnalysis.MaintenanceScore);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking thresholds for vehicle {Id}", vehicleId);
                throw;
            }
        }

        public async Task ProcessAlertsAsync()
        {
            try
            {
                var vehicles = await _context.Vehiculos.ToListAsync();
                foreach (var vehicle in vehicles)
                {
                    await CheckThresholdsAsync(vehicle.Id);
                    await GenerateLicenseExpirationAlertAsync(vehicle.Id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing alerts");
                throw;
            }
        }

        private async Task CreateThresholdAlert(Vehiculo vehicle, string metricName, decimal value)
        {
            var alert = new Alert
            {
                VehicleId = vehicle.Id,
                Type = AlertType.Vehicle,
                Severity = AlertSeverity.Warning,
                Title = $"Métrica baja de {metricName} para {vehicle.Placa}",
                Message = $"El índice de {metricName.ToLower()} del vehículo {vehicle.Placa} es {value:P0}, por debajo del umbral recomendado.",
                CreatedAt = DateTime.UtcNow,
                IsRead = false,
                IsActive = true
            };

            _context.Alerts.Add(alert);
            await _context.SaveChangesAsync();
        }

        public async Task CreateAlertAsync(AnalysisPrediction prediction)
        {
            try
            {
                var alert = new Alert
                {
                    VehicleId = prediction.VehicleId,
                    CreatedAt = DateTime.UtcNow,
                    Type = AlertType.PredictionWarning,
                    Severity = AlertSeverity.Medium,
                    Message = $"Nueva predicción generada para el vehículo {prediction.VehicleId}",
                    Details = $"Probabilidad: {prediction.Probability:P2}, Valor predicho: {prediction.PredictedValue}"
                };

                _context.Alerts.Add(alert);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating alert for prediction {PredictionId}", prediction.Id);
                throw;
            }
        }
    }
} 