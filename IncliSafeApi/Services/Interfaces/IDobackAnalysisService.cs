using IncliSafe.Shared.Models.Analysis;

namespace IncliSafeApi.Services.Interfaces
{
    public interface IDobackAnalysisService
    {
        Task<List<DobackData>> GetHistoricalData(int vehicleId, DateTime startDate, DateTime endDate);
        Task<DobackAnalysis> GetAnalysis(int id);
        Task<List<DobackData>> GetDobackDataAsync(int analysisId);
        Task<List<AnalysisPrediction>> GetPredictions(int analysisId);
        Task<TrendAnalysis> GetTrendAnalysis(int analysisId);
        Task<List<DetectedPattern>> GetDetectedPatterns(int analysisId);
        Task<List<Pattern>> GetPatternHistory(int patternId);
        Task<PatternDetails> GetPatternDetails(int patternId);
        Task<AlertSettings> GetAlertSettings(int vehicleId);
        Task<bool> ExportAnalysis(int fileId, string format);
    }
} 