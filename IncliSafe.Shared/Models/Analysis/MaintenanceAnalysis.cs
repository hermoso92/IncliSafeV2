using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Common;
using IncliSafe.Shared.Models.Entities;
using IncliSafe.Shared.Models.Notifications;
using IncliSafe.Shared.Models.Analysis.Core;

namespace IncliSafe.Shared.Models.Analysis
{
    public class MaintenanceAnalysis : AnalysisBase
    {
        public required decimal MaintenanceValue { get; set; }
        public required decimal MaintenanceThreshold { get; set; }
        public required decimal MaintenancePercentage { get; set; }
        public required bool IsMaintenanceDetected { get; set; }
        public List<decimal> MaintenanceValues { get; set; } = new();
        public List<DateTime> MaintenanceDates { get; set; } = new();
    }
} 