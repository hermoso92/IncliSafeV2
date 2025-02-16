using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using IncliSafe.Shared.Models.Analysis;
using IncliSafe.Shared.Models.Patterns;
using IncliSafe.Shared.Models.Notifications;

namespace IncliSafe.Client.Services.Interfaces
{
    public interface IDobackAnalysisService
    {
        Task<DobackAnalysis> GetAnalysis(int fileId);
        Task<List<DobackAnalysis>> GetAnalyses(int vehicleId);
        Task<DobackAnalysis> ProcessFile(int vehicleId, Stream fileStream);
        Task DeleteAnalysis(int fileId);
        Task<TrendData> GetTrendData(int vehicleId, DateTime start, DateTime end);
        Task<PatternDetails> GetPatternDetails(int patternId, int vehicleId);
        Task<List<DetectionHistory>> GetPatternHistory(int patternId, int vehicleId);
        Task<AlertSettings> GetAlertSettings();
        Task<List<DobackFileInfo>> GetVehicleFiles(int vehicleId);
        Task<bool> ExportAnalysis(int fileId);
        Task<bool> UpdateAlertSettings(AlertSettings settings);
        Task<List<HistoricalData>> GetHistoricalData(int vehicleId, DateTime start, DateTime end);
    }
} 