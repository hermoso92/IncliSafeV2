using System.Collections.Generic;
using System.Threading.Tasks;
using IncliSafe.Shared.Models.Analysis;

namespace IncliSafeApi.Services.Interfaces
{
    public interface IMaintenancePredictionService
    {
        Task<List<AnalysisPrediction>> PredictMaintenanceAsync(int vehicleId);
    }
} 