using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Common;
using IncliSafe.Shared.Models.Entities;
using IncliSafe.Shared.Models.Notifications;
using IncliSafe.Shared.Models.Analysis.Core;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Shared.Models.Analysis
{
    public class PredictionTypeInfo
    {
        public required PredictionType Type { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required decimal Confidence { get; set; }
        public required DateTime PredictedAt { get; set; }
        public required decimal PredictedValue { get; set; }
        public List<string> Recommendations { get; set; } = new();
        public Dictionary<string, string> Metadata { get; set; } = new();
    }
} 