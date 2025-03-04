using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using IncliSafe.Shared.Models.Analysis;
using IncliSafe.Shared.Models.Entities;
using IncliSafe.Shared.Models.Patterns;
using IncliSafe.Shared.Models.Notifications;
using IncliSafe.Shared.Models.Analysis.Core;

namespace IncliSafe.Client.Services.Interfaces
{
    public interface IDobackAnalysisService
    {
        Task<DashboardMetrics> GetDashboardMetrics();
        Task<List<DobackData>> GetHistoricalData(int vehicleId, DateTime startDate, DateTime endDate);
        Task<DobackAnalysis?> GetAnalysisAsync(int analysisId);
        Task<DobackData> GetDobackDataAsync(int vehicleId);
        Task<List<IncliSafe.Shared.Models.Analysis.AnalysisPrediction>> GetPredictionsAsync(int analysisId);
        Task<TrendAnalysisEntity> GetTrendAnalysis(int analysisId);
        Task<List<IncliSafe.Shared.Models.Patterns.DetectedPattern>> GetDetectedPatterns(int analysisId);
        Task<List<IncliSafe.Shared.Models.Analysis.Core.PatternDetails>> GetPatternDetails(int patternId);
        Task<List<IncliSafe.Shared.Models.Analysis.Core.PatternHistory>> GetPatternHistory(int patternId);
        Task<AlertSettings> GetAlertSettings(int patternId);
        Task<bool> ExportAnalysis(int fileId, string format);
        Task<NotificationSettings> GetNotificationSettings(int vehicleId);
        Task<NotificationSettings> UpdateNotificationSettings(NotificationSettings settings);
        Task<IEnumerable<DobackAnalysis>> GetAnalysesAsync(int vehicleId);
        Task<IncliSafe.Shared.Models.Analysis.TrendData> GetTrendData(int vehicleId, DateTime start, DateTime end);
        Task<List<DobackFileInfo>> GetVehicleFiles(int vehicleId);
        Task<IncliSafe.Shared.Models.Analysis.AnalysisPrediction> GeneratePredictionAsync(int vehicleId);
        Task<AnalysisResult> AnalyzeFileAsync(int fileId);
        Task<bool> DeleteAnalysisAsync(int analysisId);
    }
} 