using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Common;
using IncliSafe.Shared.Models.Entities;
using IncliSafe.Shared.Models.Notifications;
using IncliSafe.Shared.Models.Analysis.Core;

namespace IncliSafe.Shared.Models.Analysis
{
    public class AnalysisPrediction : BaseEntity
    {
        public new int Id { get; set; }
        public new DateTime CreatedAt { get; set; }
        public required int VehicleId { get; set; }
        public required DateTime PredictedAt { get; set; }
        public required PredictionType Type { get; set; }
        public required decimal Probability { get; set; }
        public required string Description { get; set; } = string.Empty;
        public required decimal PredictedValue { get; set; }
        public int? AnalysisId { get; set; }
        public virtual Vehicle Vehicle { get; set; } = null!;
        public virtual DobackAnalysis? Analysis { get; set; }
    }
} 