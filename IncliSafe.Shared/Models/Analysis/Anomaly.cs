using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Common;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Shared.Models.Analysis;

public class Anomaly : BaseEntity
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required AnomalyType Type { get; set; }
    public required AlertSeverity Severity { get; set; }
    public required DateTime DetectedAt { get; set; }
    public required decimal Score { get; set; }
    public required decimal ExpectedValue { get; set; }
    public required decimal ActualValue { get; set; }
    public required decimal Deviation { get; set; }
    public Dictionary<string, object> Parameters { get; set; } = new();
} 