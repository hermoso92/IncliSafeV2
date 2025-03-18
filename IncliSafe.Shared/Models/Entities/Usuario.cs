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
        public override Guid Id { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public required UserRole Role { get; set; }
        public required UserStatus Status { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public Dictionary<string, object> Preferences { get; set; } = new();
        public override DateTime CreatedAt { get; set; }
        public virtual List<Vehicle> Vehicles { get; set; } = new();
        public virtual List<Alert> Alerts { get; set; } = new();
        public virtual List<Notification> Notifications { get; set; } = new();
    }
} 