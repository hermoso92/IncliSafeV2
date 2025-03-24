using IncliSafe.Shared.Models.Base;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Shared.Models.Analysis.Core;

public class AnalysisTrend : BaseEntity
{
    public required int VehicleId { get; set; }
    public required DateTime AnalyzedAt { get; set; }
    public required TrendType Type { get; set; }
    public required string Description { get; set; }
    public required decimal Score { get; set; }
    public required decimal Confidence { get; set; }
    public required bool IsActive { get; set; }
    public required bool IsValidated { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required DateTime UpdatedAt { get; set; }
    public required string Resolution { get; set; }
    public required Dictionary<string, string> Parameters { get; set; } = new();
    public required List<DobackData> RelatedData { get; set; } = new();
    public required List<AnalysisAlert> Alerts { get; set; } = new();
    public required List<AnalysisNotification> Notifications { get; set; } = new();
} 