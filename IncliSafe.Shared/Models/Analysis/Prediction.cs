using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Enums;
using IncliSafe.Shared.Models.Common;

namespace IncliSafe.Shared.Models.Analysis
{
    public class Prediction : BaseEntity
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required PredictionType Type { get; set; }
        public required DateTime PredictedAt { get; set; }
        public required decimal Probability { get; set; }
        public required decimal PredictedValue { get; set; }
        public required DateTime ValidUntil { get; set; }
        public Dictionary<string, object> Parameters { get; set; } = new Dictionary<string, object>();
        public override DateTime CreatedAt { get; set; }
        public decimal Confidence { get; set; }
        public PredictionRisk Risk { get; set; }
    }
} 


