using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using IncliSafe.Shared.Models.Analysis;
using IncliSafe.Shared.Models.Entities;
using IncliSafe.Shared.Models.Patterns;
using IncliSafe.Shared.Models.Notifications;

namespace IncliSafe.Client.Services.Interfaces
{
    public interface IDobackAnalysisService
    {
        Task<List<DobackData>> GetHistoricalData(int vehicleId, DateTime startDate, DateTime endDate);
        Task<DobackAnalysis?> GetAnalysisAsync(int id);
        Task<List<DobackData>> GetDobackDataAsync(int analysisId);
        Task<List<AnalysisPrediction>> GetPredictionsAsync(int analysisId);
        Task<TrendAnalysis> GetTrendAnalysis(int analysisId);
        Task<List<DetectedPattern>> GetDetectedPatterns(int analysisId);
        Task<List<Pattern>> GetPatternHistory(int patternId);
        Task<PatternDetails> GetPatternDetails(int patternId);
        Task<AlertSettings> GetAlertSettings(int patternId);
        Task<bool> ExportAnalysis(int fileId, string format);
        Task<NotificationSettings> GetNotificationSettings(int vehicleId);
        Task<NotificationSettings> UpdateNotificationSettings(NotificationSettings settings);
        Task<List<DobackAnalysis>> GetAnalysesAsync(int vehicleId);
        Task<TrendData> GetTrendData(int vehicleId, DateTime start, DateTime end);
        Task<List<DobackFileInfo>> GetVehicleFiles(int vehicleId);
        Task<AnalysisPrediction> GeneratePredictionAsync(int vehicleId);
    }
} 