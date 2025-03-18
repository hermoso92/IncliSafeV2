using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IncliSafe.Shared.Models.Analysis.Core;
using IncliSafe.Shared.Models.DTOs;

namespace IncliSafeApi.Services.Interfaces
{
    public interface IDobackAnalysisService
    {
        Task<DashboardMetrics> GetDashboardMetricsAsync(int vehicleId);
        Task<DobackAnalysis> GetAnalysisAsync(int analysisId);
        Task<List<PatternDetails>> GetPatternsAsync(int vehicleId, DateTime startDate, DateTime endDate);
        Task<TrendAnalysis> GetTrendAnalysisAsync(int vehicleId, DateTime startDate, DateTime endDate);
        Task<DobackAnalysis> CreateAnalysisAsync(int vehicleId, List<DobackData> data);
        Task<DobackAnalysis> AnalyzeDobackAsync(int vehicleId, DobackAnalysisDTO analysisDto);
    }
} 