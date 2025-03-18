using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Shared.Models.Analysis;

public class AnalysisParameters
{
    public required AnalysisType Type { get; set; }
    public required DateTime StartDate { get; set; }
    public required DateTime EndDate { get; set; }
    public required decimal ConfidenceThreshold { get; set; }
    public required int SampleSize { get; set; }
    public List<string> Metrics { get; set; } = new();
    public Dictionary<string, object> CustomParameters { get; set; } = new();
} 