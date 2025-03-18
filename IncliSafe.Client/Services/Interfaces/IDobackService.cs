using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IncliSafe.Shared.Models.Analysis;
using IncliSafe.Shared.DTOs;

namespace IncliSafe.Client.Services.Interfaces
{
    public interface IDobackService
    {
        Task<DashboardMetrics> GetDashboardMetricsAsync(int vehicleId);
        Task<List<DobackAnalysis>> GetAnalysisHistoryAsync(int vehicleId, DateTime startDate, DateTime endDate);
        Task<DobackAnalysis> CreateAnalysisAsync(int vehicleId, List<DobackData> data);
        Task<List<AnalysisPattern>> GetDetectedPatternsAsync(int vehicleId, DateTime startDate, DateTime endDate);
        Task<List<AnalysisAlert>> GetRecentAlertsAsync(int vehicleId, int count);
        Task<List<AnalysisPrediction>> GetPredictionsAsync(int vehicleId, PredictionType type);
        Task<DobackAnalysis> GetAnalysisByIdAsync(int id);
        Task<TrendAnalysis> GetTrendAnalysisAsync(int vehicleId, DateTime startDate, DateTime endDate);
        Task<DobackAnalysis> AnalyzeDobackAsync(int vehicleId, DobackAnalysisDTO analysis);
        Task<DobackAnalysis> GetAnalysisAsync(int id);
    }
} 