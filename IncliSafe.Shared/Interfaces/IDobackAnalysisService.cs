public interface IDobackAnalysisService
{
    Task<List<DobackData>> GetHistoricalData(int vehicleId, DateTime startDate, DateTime endDate);
    Task<List<Prediction>> GetPredictions(int analysisId);
    Task<bool> ExportAnalysis(int fileId, string format);
    Task<PatternDetails> GetPatternDetails(int patternId);
    Task<List<PatternHistory>> GetPatternHistory(int patternId);
    Task<AlertSettings> GetAlertSettings(int vehicleId);
    Task<List<DobackData>> GetData(int analysisId);
    Task<DobackAnalysis> GetAnalysis(int analysisId);
    Task<List<DobackData>> GetDobackData(int analysisId);
} 