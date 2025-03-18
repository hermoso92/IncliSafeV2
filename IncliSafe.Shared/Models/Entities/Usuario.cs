using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Common;
using IncliSafe.Shared.Models.Entities;
using IncliSafe.Shared.Models.Notifications;
using IncliSafe.Shared.Models.Analysis.Core;

namespace IncliSafe.Shared.Models.Entities
{
    public class Usuario : BaseEntity
    {
        public new int Id { get; set; }
        public new DateTime CreatedAt { get; set; }
        public required string Nombre { get; set; }
        public required string Apellido { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Rol { get; set; }
        public required string Status { get; set; }
        public required bool IsActive { get; set; }
        public virtual List<Vehicle> Vehicles { get; set; } = new();
        public virtual List<Alert> Alerts { get; set; } = new();
        public virtual List<Notification> Notifications { get; set; } = new();
    }
} 