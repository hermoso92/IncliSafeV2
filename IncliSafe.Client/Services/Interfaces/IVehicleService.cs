using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IncliSafe.Shared.Models.Analysis;
using IncliSafe.Shared.Models.Core;
using IncliSafe.Shared.DTOs;

namespace IncliSafe.Client.Services.Interfaces
{
    public interface IVehicleService
    {
        Task<List<Vehicle>> GetVehiculosAsync();
        Task<Vehicle> GetVehiculoAsync(int id);
        Task<Vehicle> CreateVehiculoAsync(VehiculoDTO vehiculo);
        Task<Vehicle> UpdateVehiculoAsync(int id, VehiculoDTO vehiculo);
        Task DeleteVehiculoAsync(int id);
        Task<List<AnalysisPrediction>> GetPredictionsAsync(int vehicleId);
        Task<TrendAnalysis> GetTrendAnalysisAsync(int vehicleId, DateTime startDate, DateTime endDate);
    }
} 