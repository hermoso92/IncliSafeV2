using System.Collections.Generic;
using System.Threading.Tasks;
using IncliSafe.Shared.Models.Notifications;

namespace IncliSafe.Client.Services.Interfaces
{
    public interface IAlertService
    {
        Task<IEnumerable<Alert>> GetRecentAlertsAsync();
        Task<Alert> CreateAlertAsync(Alert alert);
        Task<bool> MarkAsReadAsync(int alertId);
        Task<bool> DeleteAlertAsync(int alertId);
        Task<IEnumerable<Alert>> GetAlertsByVehicleAsync(int vehicleId);
        Task<IEnumerable<Alert>> GetAlertsByUserAsync(int userId);
        Task<NotificationSettings> GetNotificationSettingsAsync(int userId);
        Task<NotificationSettings> UpdateNotificationSettingsAsync(NotificationSettings settings);
    }
} 