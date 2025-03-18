using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using IncliSafe.Shared.Models.Analysis;
using IncliSafe.Shared.Models.Notifications;
using IncliSafe.Shared.Models.Entities;
using IncliSafe.Shared.Models.Patterns;
using IncliSafe.Shared.Models.Analysis.Core;

namespace IncliSafe.Client.Services.Interfaces
{
    public interface IDobackAnalysisService
    {
        Task<DashboardMetrics> GetDashboardMetrics();
        Task<List<DobackData>> GetHistoricalData(int vehicleId, DateTime startDate, DateTime endDate);
        Task<DobackAnalysis?> GetAnalysis(int id);
        Task<List<DobackData>> GetDobackDataAsync(int analysisId);
        Task<List<Anomaly>> GetAnomaliesAsync(int analysisId);
        Task<List<AnalysisPrediction>> GetPredictionsAsync(int analysisId);
        Task<AnalysisPatternDetails> GetPatternDetailsAsync(int patternId);
        Task<TrendAnalysis> GetTrendAnalysis(int analysisId);
        Task<AlertSettings> GetAlertSettings(int patternId);
        Task<bool> ExportAnalysis(int fileId, string format);
        Task<NotificationSettings> GetNotificationSettings(int vehicleId);
        Task<NotificationSettings> UpdateNotificationSettings(NotificationSettings settings);
        Task<IEnumerable<DobackAnalysis>> GetAnalysesAsync(int vehicleId);
        Task<TrendData> GetTrendData(int vehicleId, DateTime start, DateTime end);
        Task<List<DobackFileInfo>> GetVehicleFiles(int vehicleId);
        Task<AnalysisPrediction> GeneratePredictionAsync(int vehicleId);
        Task<AnalysisResult> AnalyzeFileAsync(int fileId);
        Task<bool> DeleteAnalysisAsync(int analysisId);
    }
} 