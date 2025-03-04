using System.Collections.Generic;
using System.Threading.Tasks;
using IncliSafe.Shared.Models.Analysis;
using IncliSafe.Shared.Models.Analysis.Core;

namespace IncliSafeApi.Services.Interfaces
{
    public interface IDobackAnalysisService
    {
        Task<List<DobackAnalysis>> GetAnalysesAsync(int vehicleId);
        Task<TrendAnalysisEntity> GetTrendAnalysis(int analysisId);
        Task<List<IncliSafe.Shared.Models.Analysis.Core.Prediction>> GetPredictionsAsync(int analysisId);
        Task<List<Anomaly>> GetAnomaliesAsync(int vehicleId);
        Task<DobackAnalysis> CreateAnalysisAsync(DobackAnalysis analysis);
        Task<bool> DeleteAnalysisAsync(int analysisId);
        Task<List<DobackData>> GetDobackDataAsync(int analysisId);
        Task<AnalysisResult> GetAnalysisResultAsync(int analysisId);
    }
} 