using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Shared.Models.Analysis;

public class AnalysisQueue
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required AnalysisType Type { get; set; }
    public required QueueStatus Status { get; set; }
    public required DateTime CreatedAt { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public int Priority { get; set; }
    public List<string> Metrics { get; set; } = new();
    public List<string> Dependencies { get; set; } = new();
    public Dictionary<string, object> Parameters { get; set; } = new();
} 