using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IncliSafe.Shared.Models.Analysis;
using IncliSafe.Shared.Models.DTOs;

namespace IncliSafe.Client.Services.Interfaces
{
    public interface IDobackService
    {
        Task<DashboardMetrics> GetDashboardMetrics();
        Task<List<Alert>> GetRecentAlerts();
        Task<List<Anomaly>> GetRecentAnomalies();
        Task<AnalysisResult> GetAnalysisResult(int vehicleId);
        Task<List<DobackData>> GetDobackData(int analysisId);
        Task<TrendAnalysis> GetTrendAnalysis(int analysisId);
        Task<PredictionResult> GetPredictions(int analysisId);
        Task<DobackAnalysis?> GetAnalysis(int id);
        Task<DobackAnalysis?> GetLatestAnalysis(int vehicleId);
        Task<List<AnalysisPrediction>> GetPredictions(int vehicleId);
        Task<CoreMetrics> GetMetricsAsync();
        Task<DobackAnalysis> AnalyzeDobackAsync(int vehicleId, DobackAnalysisDTO analysisDto);
        Task<TrendAnalysis> AnalyzeTrendsAsync(int vehicleId, DateTime startDate, DateTime endDate);
    }
} 