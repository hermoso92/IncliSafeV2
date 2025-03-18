using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Common;
using IncliSafe.Shared.Models.Entities;
using IncliSafe.Shared.Models.Notifications;
using IncliSafe.Shared.Models.Analysis.Core;

namespace IncliSafe.Shared.Models.Analysis
{
    public class OverallAnalysis : AnalysisBase
    {
        public required decimal OverallValue { get; set; }
        public required decimal OverallThreshold { get; set; }
        public required decimal OverallPercentage { get; set; }
        public required bool IsOverallDetected { get; set; }
        public List<decimal> OverallValues { get; set; } = new();
        public List<DateTime> OverallDates { get; set; } = new();
    }
} 