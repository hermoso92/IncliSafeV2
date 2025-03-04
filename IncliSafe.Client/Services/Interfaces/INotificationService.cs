using System.Collections.Generic;
using System.Threading.Tasks;
using IncliSafe.Shared;
using IncliSafe.Shared.Models;
using IncliSafe.Shared.Models.Notifications;

namespace IncliSafe.Client.Services.Interfaces
{
    public interface INotificationService
    {
        Task<List<Notification>> GetNotificationsAsync();
        Task<bool> MarkAsRead(int notificationId);
        Task<bool> DeleteNotification(int notificationId);
        Task<NotificationSettings> GetNotificationSettings(int vehicleId);
        Task<NotificationSettings> UpdateNotificationSettings(NotificationSettings settings);
    }
} 