using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Common;
using IncliSafe.Shared.Models.Entities;
using IncliSafe.Shared.Models.Notifications;
using IncliSafe.Shared.Models.Analysis.Core;

namespace IncliSafe.Shared.Models.Analysis
{
    public class SafetyAnalysis : AnalysisBase
    {
        public required decimal SafetyValue { get; set; }
        public required decimal SafetyThreshold { get; set; }
        public required decimal SafetyPercentage { get; set; }
        public required bool IsSafetyDetected { get; set; }
        public List<decimal> SafetyValues { get; set; } = new();
        public List<DateTime> SafetyDates { get; set; } = new();
    }
} 