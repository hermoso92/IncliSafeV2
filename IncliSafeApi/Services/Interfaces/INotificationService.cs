using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IncliSafe.Shared.Models.DTOs;
using IncliSafe.Shared.Models.Notifications;
using IncliSafe.Shared.Models.Patterns;
using IncliSafe.Shared.Models.Analysis;

namespace IncliSafeApi.Services
{
    public interface INotificationService
    {
        Task<Notification> CreateNotificationAsync(Notification notification);
        Task<IEnumerable<Notification>> GetUserNotifications(int userId);
        Task<IEnumerable<Notification>> GetActiveAlerts(int vehicleId);
        Task<bool> MarkAsReadAsync(int notificationId);
        Task<bool> DismissNotificationAsync(int notificationId);
        Task NotifyPatternDetectedAsync(PatternDetection pattern);
        Task NotifyAnomalyDetectedAsync(Anomaly anomaly);
        Task<NotificationSettings> GetNotificationSettings(int vehicleId);
        Task<NotificationSettings> UpdateNotificationSettings(NotificationSettings settings);
        Task SendAlertAsync(Alert alert);
        Task SendLicenseExpirationWarningAsync(int userId, LicenseDTO license);
        Task SendInspectionReminderAsync(int userId, InspeccionDTO inspeccion);
        Task SendMaintenanceAlertAsync(int userId, VehicleMaintenanceDTO maintenance);
        Task<bool> GenerateMaintenanceAlertAsync(int vehicleId, VehicleMaintenanceDTO maintenance);
        Task<bool> GenerateLicenseExpirationAlertAsync(int vehicleId);
        Task CheckThresholdsAsync(int vehicleId);
        Task ProcessAlertsAsync();
        Task CreateAlertAsync(AnalysisPrediction prediction);
    }
} 