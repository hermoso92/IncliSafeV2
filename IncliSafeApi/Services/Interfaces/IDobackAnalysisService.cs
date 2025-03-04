using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IncliSafe.Shared.Models.Analysis;

namespace IncliSafeApi.Services.Interfaces
{
    public interface IDobackAnalysisService
    {
        Task<List<DobackAnalysis>> GetAnalysesAsync(int vehicleId);
        Task<TrendAnalysisEntity> GetTrendAnalysis(int analysisId);
        Task<List<AnalysisPrediction>> GetPredictionsAsync(int analysisId);
        Task<List<Anomaly>> GetAnomaliesAsync(int analysisId);
        Task<List<PatternDetails>> GetPatternsAsync(int analysisId);
        Task<List<DobackData>> GetDobackDataAsync(int analysisId);
        Task<AnalysisResult> GetAnalysisResultAsync(int analysisId);
        Task<DobackAnalysis?> GetAnalysis(int id);
        Task<List<DobackData>> GetHistoricalData(int vehicleId, DateTime startDate, DateTime endDate);
    }
} 