using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Common;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Shared.Models.Analysis;

public class AnalysisDashboard : BaseEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required AnalysisType Type { get; set; }
    public required bool IsEnabled { get; set; }
    public required DateTime LastUpdated { get; set; }
    public required string Layout { get; set; }
    public required string Theme { get; set; }
    public List<Widget> Widgets { get; set; } = new();
    public Dictionary<string, object> Parameters { get; set; } = new();
}

public class Widget
{
    public required string Id { get; set; }
    public required string Title { get; set; }
    public required WidgetType Type { get; set; }
    public required string DataSource { get; set; }
    public required string Position { get; set; }
    public required string Size { get; set; }
    public Dictionary<string, object> Configuration { get; set; } = new();
} 