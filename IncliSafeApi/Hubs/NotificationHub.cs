using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using IncliSafe.Shared.Models.DTOs;

namespace IncliSafeApi.Hubs
{
    [Authorize]
    public class NotificationHub : Hub
    {
        private readonly ILogger<NotificationHub> _logger;

        public NotificationHub(ILogger<NotificationHub> logger)
        {
            _logger = logger;
        }

        public override async Task OnConnectedAsync()
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"User_{userId}");
                _logger.LogInformation($"Usuario {userId} conectado a notificaciones");
            }
            await Groups.AddToGroupAsync(Context.ConnectionId, Context.UserIdentifier!);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"User_{userId}");
            }
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, Context.UserIdentifier!);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task JoinVehicleGroup(string vehicleId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"Vehicle_{vehicleId}");
        }

        public async Task LeaveVehicleGroup(string vehicleId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Vehicle_{vehicleId}");
        }

        public async Task SendNotification(string user, string message)
        {
            await Clients.User(user).SendAsync("ReceiveNotification", message);
        }

        public async Task SendAlert(VehicleAlertDTO alert)
        {
            await Clients.User(Context.UserIdentifier!).SendAsync("ReceiveAlert", alert);
        }

        public async Task MarkAlertAsRead(int alertId)
        {
            await Clients.User(Context.UserIdentifier!).SendAsync("AlertRead", alertId);
        }
    }
} 