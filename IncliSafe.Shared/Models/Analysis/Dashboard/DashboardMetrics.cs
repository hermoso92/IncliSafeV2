using IncliSafe.Shared.Models.Enums;
using System;

namespace IncliSafe.Shared.Models.Analysis.Dashboard;

public class DashboardMetrics
{
    public required decimal StabilityScore { get; set; }
    public required decimal SafetyScore { get; set; }
    public required decimal MaintenanceScore { get; set; }
    public required decimal PerformanceScore { get; set; }
    public required decimal EfficiencyScore { get; set; }
    public required decimal ReliabilityScore { get; set; }
    public required decimal AvailabilityScore { get; set; }
    public required decimal UtilizationScore { get; set; }
    public required decimal ComplianceScore { get; set; }
    public required decimal CostScore { get; set; }
    public required decimal RiskScore { get; set; }
    public required decimal QualityScore { get; set; }
    public required decimal ProductivityScore { get; set; }
    public required decimal SustainabilityScore { get; set; }
    public required decimal InnovationScore { get; set; }
    public required decimal CompetitivenessScore { get; set; }
    public required decimal CustomerSatisfactionScore { get; set; }
    public required decimal EmployeeSatisfactionScore { get; set; }
    public required decimal StakeholderSatisfactionScore { get; set; }
    public required decimal MarketShareScore { get; set; }
    public required int TotalAnalyses { get; set; }
    public required int TotalAlerts { get; set; }
    public required int TotalAnomalies { get; set; }
    public required int TotalPatterns { get; set; }
    public required int TotalPredictions { get; set; }
    public required DateTime LastAnalysisDate { get; set; }
    public required DateTime LastAlertDate { get; set; }
    public required DateTime LastAnomalyDate { get; set; }
    public required DateTime LastPatternDate { get; set; }
    public required DateTime LastPredictionDate { get; set; }
}
 
