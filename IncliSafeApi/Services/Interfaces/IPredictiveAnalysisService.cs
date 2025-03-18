using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IncliSafe.Shared.Models.Analysis;
using IncliSafe.Shared.Models.Entities;
using CoreAnalysisPrediction = IncliSafe.Shared.Models.Analysis.Core.AnalysisPrediction;

namespace IncliSafeApi.Services
{
    public interface IPredictiveAnalysisService
    {
        Task<List<Anomaly>> GetAnomalies(int vehicleId);
        Task<List<Pattern>> GetPatterns(int vehicleId);
        Task<List<AnalysisPrediction>> GetPredictions(int vehicleId);
        Task<TrendAnalysis> AnalyzeTrends(int vehicleId, DateTime startDate, DateTime endDate);
        Task<PredictionResult> PredictStability(int vehicleId, DateTime startDate, DateTime endDate);
        Task<List<Anomaly>> DetectAnomalies(int vehicleId, DateTime startDate, DateTime endDate);
        Task<List<Pattern>> DetectPatterns(int vehicleId, DateTime startDate, DateTime endDate);
        Task<List<CoreAnalysisPrediction>> GetPredictions(int analysisId);
        Task<AnalysisPrediction> GeneratePredictionAsync(Vehicle vehicle);
    }
} 