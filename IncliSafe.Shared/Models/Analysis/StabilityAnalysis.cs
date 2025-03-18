using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Common;
using IncliSafe.Shared.Models.Entities;
using IncliSafe.Shared.Models.Notifications;
using IncliSafe.Shared.Models.Analysis.Core;

namespace IncliSafe.Shared.Models.Analysis
{
    public class StabilityAnalysis : AnalysisBase
    {
        public required decimal StabilityValue { get; set; }
        public required decimal StabilityThreshold { get; set; }
        public required decimal StabilityPercentage { get; set; }
        public required bool IsStabilityDetected { get; set; }
        public List<decimal> StabilityValues { get; set; } = new();
        public List<DateTime> StabilityDates { get; set; } = new();
    }
} 