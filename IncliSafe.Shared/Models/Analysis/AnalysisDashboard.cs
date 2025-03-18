using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Shared.Models.Analysis;

public class AnalysisDashboard
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required AnalysisType Type { get; set; }
    public required DateTime LastUpdated { get; set; }
    public List<AnalysisWidget> Widgets { get; set; } = new();
    public List<string> Metrics { get; set; } = new();
    public Dictionary<string, object> Parameters { get; set; } = new();
}

public class AnalysisWidget
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required WidgetType Type { get; set; }
    public required int Position { get; set; }
    public required int Size { get; set; }
    public List<string> Metrics { get; set; } = new();
    public Dictionary<string, object> Parameters { get; set; } = new();
} 