using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IncliSafe.Shared.Models.Analysis.Core;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Client.Services.Interfaces
{
    public interface IDobackService
    {
        Task<DashboardMetrics> GetDashboardMetricsAsync(int vehicleId);
        Task<DobackAnalysis> GetAnalysisAsync(int vehicleId);
        Task<DobackAnalysis> GetAnalysisWithDataAsync(int vehicleId, DobackData data);
        Task<DobackAnalysis> GetLatestAnalysisAsync(int vehicleId);
        Task<List<AnalysisPrediction>> GetPredictionsAsync(int vehicleId, PredictionType type);
        Task<DobackAnalysis> GetTrendAnalysisAsync(int vehicleId);
        Task<DobackAnalysis> GetPatternAnalysisAsync(int vehicleId);
        Task<DobackAnalysis> GetAnomalyAnalysisAsync(int vehicleId);
    }
} 