using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Common;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Shared.Models.Analysis;

public class AnalysisCache : BaseEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required AnalysisType Type { get; set; }
    public required DateTime ExpiresAt { get; set; }
    public required string CacheKey { get; set; }
    public required object CacheValue { get; set; }
    public string? ETag { get; set; }
    public List<string> Metrics { get; set; } = new();
    public Dictionary<string, object> Parameters { get; set; } = new();
} 