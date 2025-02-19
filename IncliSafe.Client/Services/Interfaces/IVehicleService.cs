using System.Threading.Tasks;
using System.Collections.Generic;
using IncliSafe.Shared.Models.DTOs;
using IncliSafe.Shared.Models.Entities;
using IncliSafe.Shared.Models.Analysis.Core;

namespace IncliSafe.Client.Services.Interfaces
{
    public interface IVehicleService
    {
        Task<List<Vehiculo>> GetVehiculos();
        Task<Vehiculo?> GetVehicle(int id);
        Task<List<Vehiculo>> GetUserVehiculos(int userId);
        Task<Vehiculo?> CreateVehiculo(Vehiculo vehiculo);
        Task<bool> UpdateVehiculo(Vehiculo vehiculo);
        Task<bool> DeleteVehiculo(int id);
        Task<List<Inspeccion>> GetInspeccionesAsync(int vehiculoId);
        Task<VehiculoDTO> GetVehicleAsync(int id);
        Task<List<VehiculoDTO>> GetVehiclesAsync();
        Task<VehiculoDTO> CreateVehicleAsync(VehiculoDTO vehicle);
        Task<VehiculoDTO> UpdateVehicleAsync(VehiculoDTO vehicle);
        Task<bool> DeleteVehicleAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<List<Alert>> GetAlertsAsync(int vehicleId);
        Task<NotificationSettings> GetNotificationSettingsAsync(int vehicleId);
        Task<List<DobackAnalysis>> GetAnalysesAsync(int vehicleId);
        Task<TrendAnalysisEntity> GetTrendAnalysisAsync(int vehicleId);
        Task<List<IncliSafe.Shared.Models.Analysis.Core.Prediction>> GetPredictionsAsync(int vehicleId);
        Task<List<Anomaly>> GetAnomaliesAsync(int vehicleId);
    }
} 