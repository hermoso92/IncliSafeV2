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

        public async Task SendAlertAsync(int userId, string title, string message, NotificationSeverity severity)
        {
            var alert = new Alert
            {
                Title = title,
                Message = message,
                Severity = severity,
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                UserId = userId
            };

            await CreateAlertAsync(alert);
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
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            await _hubContext.Clients.User(notification.UserId.ToString())
                .SendAsync("ReceiveNotification", notification);

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
            _context.Alerts.Add(alert);
            await _context.SaveChangesAsync();
            return alert;
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
                Message = $"Se ha detectado un patrón con confianza del {pattern.ConfidenceScore:P2}",
                Type = NotificationType.PatternDetected,
                Severity = NotificationSeverity.Info,
                CreatedAt = DateTime.UtcNow,
                IsRead = false,
                UserId = pattern.VehicleId
            };

            await CreateNotificationAsync(notification);
        }

        public async Task NotifyAnomalyDetectedAsync(int vehicleId, string message)
        {
            var notification = new Notification
            {
                Title = "Anomalía Detectada",
                Message = message,
                Type = NotificationType.Anomaly,
                Severity = NotificationSeverity.Warning,
                CreatedAt = DateTime.UtcNow,
                IsRead = false,
                UserId = vehicleId
            };

            await CreateNotificationAsync(notification);
        }

        public async Task<bool> UpdateNotificationSettingsAsync(int vehicleId, NotificationSettings settings)
        {
            var vehiculo = await _context.Vehiculos.FindAsync(vehicleId);
            if (vehiculo == null) return false;

            vehiculo.NotificationSettings = settings;
            await _context.SaveChangesAsync();
            return true;
        }
    }
} 