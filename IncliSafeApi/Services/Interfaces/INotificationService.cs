using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IncliSafe.Shared.Models.Notifications;
using IncliSafe.Shared.Models.Patterns;
using IncliSafe.Shared.Models.Analysis;

namespace IncliSafeApi.Services.Interfaces
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
    }
} 