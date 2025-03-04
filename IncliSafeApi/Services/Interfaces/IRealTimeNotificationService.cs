using System.Threading.Tasks;
using IncliSafe.Shared.Models.DTOs;
using IncliSafe.Shared.Models.Notifications;

namespace IncliSafeApi.Services.Interfaces
{
    public interface IRealTimeNotificationService
    {
        Task SendNotificationAsync(Notification notification);
        Task SendAlertAsync(VehicleAlertDTO alert);
        Task SendVehicleUpdateAsync(int vehicleId);
        Task SendInspectionReminderAsync(string userId, InspeccionDTO inspeccion);
        Task SendLicenseExpirationWarningAsync(string userId, LicenseDTO license);
    }
} 