using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Common;
using IncliSafe.Shared.Models.Entities;
using IncliSafe.Shared.Models.Notifications;
using IncliSafe.Shared.Models.Analysis.Core;

namespace IncliSafe.Shared.Models.Analysis
{
    public class PerformanceAnalysis : AnalysisBase
    {
        public required decimal PerformanceValue { get; set; }
        public required decimal PerformanceThreshold { get; set; }
        public required decimal PerformancePercentage { get; set; }
        public required bool IsPerformanceDetected { get; set; }
        public List<decimal> PerformanceValues { get; set; } = new();
        public List<DateTime> PerformanceDates { get; set; } = new();
    }
} 