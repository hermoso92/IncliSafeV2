using IncliSafe.Shared.Models.Analysis.Core;
public interface IDobackAnalysisService
{
    Task<DobackAnalysis> AnalyzeFile(int vehicleId, Stream fileStream, string fileName);
    Task<List<DobackAnalysis>> GetVehicleAnalyses(int vehicleId);
    Task<AnalysisResult> GetLatestAnalysis(int vehicleId);
    Task<Dictionary<string, double>> GetTrends(int vehicleId);
    Task<List<IncliSafe.Shared.Models.Patterns.DetectedPattern>> GetDetectedPatterns(int vehicleId);
} 