using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IncliSafe.Shared.Models.Notifications;
using IncliSafe.Shared.Models.Patterns;

namespace IncliSafeApi.Services.Interfaces
{
    public interface INotificationService
    {
        Task<List<Notification>> GetUserNotificationsAsync(int userId);
        Task<Notification> CreateNotificationAsync(Notification notification);
        Task<bool> MarkAsReadAsync(int notificationId);
        Task<bool> DeleteNotificationAsync(int notificationId);
        Task NotifyPatternDetectedAsync(PatternDetection pattern);
        Task NotifyAnomalyDetectedAsync(int vehicleId, string message);
    }
} 