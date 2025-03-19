using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Common;
using IncliSafe.Shared.Models.Entities;
using IncliSafe.Shared.Models.Notifications;
using IncliSafe.Shared.Models.Analysis.Core;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Shared.Models.Entities
{
    public class Usuario : BaseEntity
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public string? Phone { get; set; }
        public required UserRole Role { get; set; }
        public required UserStatus Status { get; set; }
        public required bool IsActive { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public string? LastLoginIp { get; set; }
        public List<Notification> Notifications { get; set; } = new();
        public Dictionary<string, object> Preferences { get; set; } = new();
    }
} 