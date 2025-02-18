using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IncliSafe.Shared.Models.Analysis;
using IncliSafe.Shared.Models.Patterns;

namespace IncliSafeApi.Services.Interfaces
{
    public interface IDobackService
    {
        Task<DashboardMetrics> GetDashboardMetrics();
        Task<List<DobackAnalysis>> GetAnalysisHistoryAsync(int vehicleId);
        Task<DobackAnalysis?> GetAnalysis(int id);
        Task<DobackAnalysis> CreateAnalysisAsync(DobackAnalysis analysis);
        Task<bool> UpdateAnalysisAsync(DobackAnalysis analysis);
        Task<bool> DeleteAnalysisAsync(int id);
        Task<List<DetectedPattern>> GetDetectedPatternsAsync(int analysisId);
        Task<TrendAnalysis> GetTrendAnalysisAsync(int vehicleId);
        Task<List<DobackData>> GetDobackDataAsync(int analysisId);
        Task<List<DobackData>> GetDobackData(int vehicleId, DateTime start, DateTime end);
        Task<List<Alert>> GetRecentAlerts(int vehicleId);
        Task<List<Anomaly>> GetRecentAnomalies(int vehicleId);
        Task<TrendAnalysis> GetTrendAnalysis(int vehicleId);
        Task<AnalysisResult> GetAnalysisResult(int vehicleId);
        Task<List<AnalysisPrediction>> GetPredictions(int vehicleId);
        Task<AnalysisPrediction> GeneratePredictionAsync(int vehicleId);
    }
} 