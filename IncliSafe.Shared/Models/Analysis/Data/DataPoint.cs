using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Common;

namespace IncliSafe.Shared.Models.Analysis.Data;

public class DataPoint : BaseEntity
{
    public required string Name { get; set; }
    public required string Unit { get; set; }
    public required DateTime Timestamp { get; set; }
    public string? Label { get; set; }
    public required decimal Value { get; set; }
    public required Dictionary<string, string> Metadata { get; set; } = new();
    public Dictionary<string, decimal> Metrics { get; set; } = new();
} 