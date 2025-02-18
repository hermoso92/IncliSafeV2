using System.Threading.Tasks;
using IncliSafe.Shared.Models.DTOs;

namespace IncliSafeApi.Services.Interfaces
{
    public interface IAlertGenerationService
    {
        Task<Alert> SendAlertAsync(Alert alert);
        Task<List<Alert>> GetAlertsAsync(int userId);
        Task<bool> MarkAsReadAsync(int alertId);
        Task<bool> DeleteAlertAsync(int alertId);
        Task<AlertSettings> GetAlertSettingsAsync(int userId);
        Task<AlertSettings> UpdateAlertSettingsAsync(AlertSettings settings);
    }
} 