using System;
using System.Collections.Generic;

namespace IncliSafe.Shared.Models.Analysis;

public class DobackDataPoint
{
    public required DateTime Timestamp { get; set; }
    public required decimal Value { get; set; }
    public string? Unit { get; set; }
    public Dictionary<string, object> Metadata { get; set; } = new();
} 