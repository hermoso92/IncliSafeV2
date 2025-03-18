using System.Threading.Tasks;
using IncliSafe.Shared.Models.Notifications;

namespace IncliSafe.Shared.Interfaces
{
    public interface IAlertGenerationService
    {
        Task<Alert> SendAlertAsync(Alert alert);
        Task<Alert> SendAlertAsync(Alert alert, NotificationSeverity severity);
        Task<bool> CheckThresholdsAsync(int vehicleId);
        Task ProcessAlertsAsync();
    }
} 