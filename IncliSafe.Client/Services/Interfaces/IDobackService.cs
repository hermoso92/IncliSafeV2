using System.Threading.Tasks;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Analysis;

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
    }
} 