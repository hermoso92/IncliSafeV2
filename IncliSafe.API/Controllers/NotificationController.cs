using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IncliSafe.Shared.Models.Notifications;
using IncliSafe.Shared.Services;

namespace IncliSafe.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class NotificationController : ControllerBase
{
    private readonly INotificationService _notificationService;

    public NotificationController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<List<Notification>>> GetUserNotifications(int userId)
    {
        var notifications = await _notificationService.GetUserNotificationsAsync(userId);
        return Ok(notifications);
    }

    [HttpPost]
    public async Task<ActionResult<Notification>> CreateNotification([FromBody] Notification notification)
    {
        var created = await _notificationService.CreateNotificationAsync(notification);
        return CreatedAtAction(nameof(GetUserNotifications), new { userId = created.UserId }, created);
    }

    [HttpPut("{id}/read")]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        var result = await _notificationService.MarkAsReadAsync(id);
        if (!result)
            return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteNotification(int id)
    {
        var result = await _notificationService.DeleteNotificationAsync(id);
        if (!result)
            return NotFound();
        return NoContent();
    }

    [HttpGet("settings/{userId}")]
    public async Task<ActionResult<NotificationSettings>> GetUserSettings(int userId)
    {
        var settings = await _notificationService.GetUserSettingsAsync(userId);
        return Ok(settings);
    }

    [HttpPut("settings")]
    public async Task<IActionResult> UpdateUserSettings([FromBody] NotificationSettings settings)
    {
        var result = await _notificationService.UpdateUserSettingsAsync(settings);
        if (!result)
            return BadRequest();
        return NoContent();
    }

    [HttpGet("alerts/{userId}")]
    public async Task<ActionResult<List<Alert>>> GetActiveAlerts(int userId)
    {
        var alerts = await _notificationService.GetActiveAlertsAsync(userId);
        return Ok(alerts);
    }

    [HttpPost("alerts")]
    public async Task<ActionResult<Alert>> CreateAlert([FromBody] Alert alert)
    {
        var created = await _notificationService.CreateAlertAsync(alert);
        return CreatedAtAction(nameof(GetActiveAlerts), new { userId = created.UserId }, created);
    }

    [HttpPut("alerts/{id}/resolve")]
    public async Task<IActionResult> ResolveAlert(int id, [FromBody] string resolution)
    {
        var result = await _notificationService.ResolveAlertAsync(id, resolution);
        if (!result)
            return NotFound();
        return NoContent();
    }
} 