using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Common;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Shared.Models.Patterns
{
    public class PatternHistory : BaseEntity
    {
        public required int PatternId { get; set; }
        public required string Action { get; set; }
        public required string Description { get; set; }
        public required DateTime Timestamp { get; set; }
        public required string UserId { get; set; }
        public Dictionary<string, object> Parameters { get; set; } = new();
    }
} 