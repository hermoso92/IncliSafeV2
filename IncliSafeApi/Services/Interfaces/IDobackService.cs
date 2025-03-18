using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IncliSafe.Shared.Models.Analysis;
using IncliSafe.Shared.Models.Patterns;
using IncliSafe.Shared.Models.Analysis.Core;
using CoreAnalysisPrediction = IncliSafe.Shared.Models.Analysis.Core.AnalysisPrediction;
using IncliSafe.Shared.Models.DTOs;
using IncliSafe.Shared.DTOs;
using IncliSafe.Shared.Models.Notifications;

namespace IncliSafeApi.Services.Interfaces
{
    public interface IDobackService
    {
        Task<DashboardMetrics> GetDashboardMetrics(int vehicleId);
        Task<DobackAnalysis> GetAnalysis(int id);
        Task<List<DobackAnalysis>> GetAnalyses(int vehicleId);
        Task<TrendAnalysis> GetTrendAnalysis(int analysisId);
        Task<List<Anomaly>> GetAnomalies(int vehicleId);
        Task<List<Pattern>> GetPatterns(int vehicleId);
        Task<List<AnalysisPrediction>> GetPredictions(int vehicleId);
        Task<DobackAnalysis> GetLatestAnalysisAsync(int vehicleId);
        Task<DobackAnalysis> ProcessDobackDataAsync(List<DobackData> data);
        Task<CoreMetrics> GetMetricsAsync();
        Task<List<DobackAnalysis>> GetAnalysisHistoryAsync(int vehicleId, DateTime startDate, DateTime endDate);
        Task<DobackAnalysis?> GetAnalysis(int id);
        Task<DobackAnalysis> CreateAnalysisAsync(int vehicleId, List<DobackData> data);
        Task<bool> UpdateAnalysisAsync(DobackAnalysis analysis);
        Task<bool> DeleteAnalysisAsync(int id);
        Task<List<PatternDetails>> GetDetectedPatternsAsync(int vehicleId, DateTime startDate, DateTime endDate);
        Task<TrendAnalysis> GetTrendAnalysisAsync(int vehicleId, DateTime startDate, DateTime endDate);
        Task<List<DobackData>> GetDobackDataAsync(int analysisId);
        Task<List<DobackData>> GetDobackData(int vehicleId, DateTime start, DateTime end);
        Task<List<Alert>> GetRecentAlertsAsync(int vehicleId, int count = 10);
        Task<AnalysisResult> GetAnalysisResult(int vehicleId);
        Task<CoreAnalysisPrediction> GeneratePredictionAsync(int vehicleId);
        Task<DobackAnalysis?> GetLatestAnalysis(int vehicleId);
        Task<DobackAnalysis> AnalyzeDobackAsync(int vehicleId, DobackAnalysisDTO analysisDto);
        Task<DobackAnalysis> GetAnalysisByIdAsync(int analysisId);
    }
} 