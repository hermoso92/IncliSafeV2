using System;
using System.Threading.Tasks;
using IncliSafe.Shared.Models.Analysis;

namespace IncliSafeApi.Services.Interfaces
{
    public interface ITrendAnalysisService
    {
        Task<TrendAnalysis> AnalyzeTrendsAsync(int vehicleId, DateTime startDate, DateTime endDate);
    }
} 