using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IncliSafe.Shared.Models.Analysis.Core;

namespace IncliSafe.Client.Services.Interfaces
{
    public interface IPredictiveAnalysisService
    {
        Task<PredictionResult> PredictStability(int vehicleId, DateTime startDate, DateTime endDate);
        Task<List<Anomaly>> DetectAnomalies(int vehicleId, DateTime startDate, DateTime endDate);
        Task<TrendAnalysis> AnalyzeTrends(int vehicleId, DateTime startDate, DateTime endDate);
        Task<List<Pattern>> DetectPatterns(int vehicleId, DateTime startDate, DateTime endDate);
        Task<List<AnalysisPrediction>> GetPredictions(int vehicleId);
    }
} 