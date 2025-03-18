using IncliSafe.Shared.Models.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IncliSafe.Shared.Models.Notifications;

namespace IncliSafe.Shared.Models.Patterns
{
    public interface IPatternService
    {
        Task<List<PatternDetection>> GetPatternDetectionsAsync(int vehicleId);
        Task<NotificationSettings> GetNotificationSettingsAsync(int vehicleId);
        Task<bool> UpdateNotificationSettingsAsync(int vehicleId, NotificationSettings settings);
        Task<List<PatternDetection>> DetectPatternsAsync(int vehicleId, DateTime start, DateTime end);
        Task<NotificationSettings> GetDefaultNotificationSettingsAsync();
        Task<bool> ApplyNotificationSettingsAsync(int vehicleId, NotificationSettings settings);
    }
} 
