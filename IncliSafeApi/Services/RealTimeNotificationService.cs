using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using IncliSafe.Shared.Models.DTOs;
using IncliSafeApi.Hubs;
using IncliSafeApi.Services.Interfaces;
using IncliSafe.Shared.Models.Notifications;

namespace IncliSafeApi.Services
{
    public class RealTimeNotificationService : IRealTimeNotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly ILogger<RealTimeNotificationService> _logger;

        public RealTimeNotificationService(
            IHubContext<NotificationHub> hubContext,
            ILogger<RealTimeNotificationService> logger)
        {
            _hubContext = hubContext;
            _logger = logger;
        }

        public async Task SendNotificationAsync(Notification notification)
        {
            await _hubContext.Clients.User(notification.UserId.ToString())
                .SendAsync("ReceiveNotification", notification);
        }

        public async Task SendAlertAsync(VehicleAlertDTO alert)
        {
            await _hubContext.Clients.All
                .SendAsync("ReceiveAlert", alert);
        }

        public async Task SendVehicleUpdateAsync(int vehicleId)
        {
            await _hubContext.Clients.All
                .SendAsync("VehicleUpdated", vehicleId);
        }

        public async Task SendInspectionReminderAsync(string userId, InspeccionDTO inspeccion)
        {
            await _hubContext.Clients.User(userId)
                .SendAsync("InspectionReminder", inspeccion);
        }

        public async Task SendLicenseExpirationWarningAsync(string userId, LicenseDTO license)
        {
            await _hubContext.Clients.User(userId)
                .SendAsync("LicenseWarning", license);
        }
    }
} 