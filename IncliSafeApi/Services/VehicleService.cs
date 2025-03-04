using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using IncliSafe.Shared.Models.Entities;
using IncliSafe.Shared.Models.DTOs;
using IncliSafe.Shared.Exceptions;
using IncliSafeApi.Data;
using IncliSafeApi.Services.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace IncliSafeApi.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<VehicleService> _logger;

        public VehicleService(
            ApplicationDbContext context,
            IMapper mapper,
            ILogger<VehicleService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<VehiculoDTO>> GetVehiclesAsync(int ownerId)
        {
            var vehicles = await _context.Vehiculos
                .Include(v => v.Owner)
                .Include(v => v.License)
                .Where(v => v.OwnerId == ownerId)
                .ToListAsync();

            return vehicles.Select(v => VehiculoDTO.FromEntity(v)).ToList();
        }

        public async Task<VehiculoDTO> GetVehicleAsync(int id)
        {
            var vehicle = await _context.Vehiculos
                .Include(v => v.Owner)
                .Include(v => v.License)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (vehicle == null)
                throw new NotFoundException("Vehicle not found");

            return VehiculoDTO.FromEntity(vehicle);
        }

        public async Task<VehiculoDTO> CreateVehicleAsync(VehiculoDTO dto)
        {
            var vehicle = _mapper.Map<Vehiculo>(dto);
            _context.Vehiculos.Add(vehicle);
            await _context.SaveChangesAsync();
            return _mapper.Map<VehiculoDTO>(vehicle);
        }

        public async Task<VehiculoDTO> UpdateVehicleAsync(int id, VehiculoDTO dto)
        {
            var vehicle = await _context.Vehiculos.FindAsync(id);
            if (vehicle == null)
                throw new NotFoundException("Vehicle not found");

            vehicle.Nombre = dto.Nombre;
            vehicle.Placa = dto.Placa;
            vehicle.Marca = dto.Marca;
            vehicle.Modelo = dto.Modelo;
            vehicle.Color = dto.Color;
            vehicle.Año = dto.Año;
            vehicle.Tipo = dto.Tipo;
            vehicle.Estado = dto.Estado;
            vehicle.OwnerId = dto.OwnerId;
            vehicle.Activo = dto.Activo;
            vehicle.UltimaInspeccion = dto.UltimaInspeccion;

            await _context.SaveChangesAsync();
            return VehiculoDTO.FromEntity(vehicle);
        }

        public async Task<bool> DeleteVehicleAsync(int id)
        {
            var vehicle = await _context.Vehiculos.FindAsync(id);
            if (vehicle == null) return false;

            _context.Vehiculos.Remove(vehicle);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<InspeccionDTO>> GetInspeccionesAsync(int vehicleId)
        {
            try
            {
                var inspecciones = await _context.Inspecciones
                    .Where(i => i.VehiculoId == vehicleId)
                    .OrderByDescending(i => i.Fecha)
                    .ToListAsync();

                return inspecciones.Select(i => i.ToDTO()).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting inspections for vehicle {VehicleId}", vehicleId);
                return new List<InspeccionDTO>();
            }
        }

        public async Task<bool> AddInspeccionAsync(int vehicleId, InspeccionDTO inspeccionDto)
        {
            try
            {
                var vehicle = await _context.Vehiculos.FindAsync(vehicleId);
                if (vehicle == null)
                    return false;

                var inspeccion = inspeccionDto.ToEntity();
                inspeccion.VehiculoId = vehicleId;
                inspeccion.Fecha = DateTime.UtcNow;

                _context.Inspecciones.Add(inspeccion);
                
                vehicle.UltimaInspeccion = inspeccion.Fecha;
                
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding inspection for vehicle {VehicleId}", vehicleId);
                return false;
            }
        }

        public async Task<bool> ValidateLicenseAsync(int vehicleId)
        {
            try
            {
                var license = await _context.Licenses
                    .FirstOrDefaultAsync(l => l.VehicleId == vehicleId);

                return license != null && 
                       license.IsActive && 
                       license.ExpirationDate > DateTime.UtcNow;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating license for vehicle {VehicleId}", vehicleId);
                return false;
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Vehiculos.AnyAsync(v => v.Id == id);
        }

        public async Task<LicenseDTO?> GetLicenseAsync(int vehicleId)
        {
            var license = await _context.Licenses
                .FirstOrDefaultAsync(l => l.VehicleId == vehicleId && l.IsActive);
            return license?.ToDTO();
        }

        public async Task<bool> UpdateLicenseAsync(int vehicleId, LicenseDTO licenseDto)
        {
            try
            {
                var license = await _context.Licenses
                    .FirstOrDefaultAsync(l => l.VehicleId == vehicleId);

                if (license == null)
                    return false;

                license.Type = licenseDto.Type;
                license.ExpirationDate = licenseDto.ExpirationDate;
                license.IsActive = licenseDto.IsActive;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating license for vehicle {VehicleId}", vehicleId);
                return false;
            }
        }

        public async Task<LicenseDTO> CreateLicenseAsync(int vehicleId, LicenseType type)
        {
            try
            {
                var vehicle = await _context.Vehiculos.FindAsync(vehicleId);
                if (vehicle == null)
                    throw new InvalidOperationException($"Vehicle {vehicleId} not found");

                var license = new License
                {
                    VehicleId = vehicleId,
                    Type = type,
                    ExpirationDate = DateTime.UtcNow.AddYears(1),
                    IsActive = true
                };

                _context.Licenses.Add(license);
                await _context.SaveChangesAsync();

                return license.ToDTO();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating license for vehicle {VehicleId}", vehicleId);
                throw;
            }
        }

        public async Task<VehicleStatsDTO> GetVehicleStatsAsync(int vehicleId)
        {
            try
            {
                var vehicle = await _context.Vehiculos
                    .Include(v => v.Inspecciones)
                    .Include(v => v.Analyses)
                    .Include(v => v.License)
                    .FirstOrDefaultAsync(v => v.Id == vehicleId);

                if (vehicle == null)
                    throw new InvalidOperationException($"Vehicle {vehicleId} not found");

                var license = vehicle.License?.ToDTO();
                
                return new VehicleStatsDTO
                {
                    VehicleId = vehicleId,
                    TotalInspections = vehicle.Inspecciones.Count,
                    LastInspectionDate = vehicle.UltimaInspeccion,
                    TotalAnalyses = vehicle.Analyses.Count,
                    LastAnalysisDate = vehicle.Analyses.OrderByDescending(a => a.Timestamp).FirstOrDefault()?.Timestamp,
                    AverageStabilityScore = vehicle.Analyses.Any() 
                        ? vehicle.Analyses.Average(a => a.StabilityScore) 
                        : 0,
                    AverageSafetyScore = vehicle.Analyses.Any() 
                        ? vehicle.Analyses.Average(a => a.SafetyScore) 
                        : 0,
                    MonthlyStats = await GetMonthlyStatsAsync(
                        vehicleId, 
                        DateTime.UtcNow.AddMonths(-6), 
                        DateTime.UtcNow),
                    LicenseStatus = license != null 
                        ? LicenseValidationDTO.FromLicense(license) 
                        : new LicenseValidationDTO()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting stats for vehicle {VehicleId}", vehicleId);
                throw;
            }
        }

        public async Task<List<MonthlyStatsDTO>> GetMonthlyStatsAsync(int vehicleId, DateTime startDate, DateTime endDate)
        {
            try
            {
                var stats = await _context.DobackAnalyses
                    .Where(a => a.VehicleId == vehicleId && 
                           a.Timestamp >= startDate && 
                           a.Timestamp <= endDate)
                    .GroupBy(a => new { a.Timestamp.Year, a.Timestamp.Month })
                    .Select(g => new MonthlyStatsDTO
                    {
                        Month = new DateTime(g.Key.Year, g.Key.Month, 1),
                        AnalysesCount = g.Count(),
                        AverageStabilityScore = g.Average(a => a.StabilityScore),
                        AverageSafetyScore = g.Average(a => a.SafetyScore),
                        InspectionsCount = _context.Inspecciones.Count(i => 
                            i.VehiculoId == vehicleId && 
                            i.Fecha.Year == g.Key.Year && 
                            i.Fecha.Month == g.Key.Month)
                    })
                    .OrderByDescending(s => s.Month)
                    .ToListAsync();

                return stats;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting monthly stats for vehicle {VehicleId}", vehicleId);
                return new List<MonthlyStatsDTO>();
            }
        }

        public async Task<VehicleSummaryDTO> GetVehicleSummaryAsync(int vehicleId)
        {
            try
            {
                var vehicle = await _context.Vehiculos
                    .Include(v => v.License)
                    .Include(v => v.Analyses.OrderByDescending(a => a.Timestamp).Take(1))
                    .FirstOrDefaultAsync(v => v.Id == vehicleId);

                if (vehicle == null)
                    throw new InvalidOperationException($"Vehicle {vehicleId} not found");

                var lastAnalysis = vehicle.Analyses.FirstOrDefault();
                var license = vehicle.License?.ToDTO();

                return new VehicleSummaryDTO
                {
                    Id = vehicle.Id,
                    Nombre = vehicle.Nombre,
                    Placa = vehicle.Placa,
                    Estado = vehicle.Estado,
                    UltimaInspeccion = vehicle.UltimaInspeccion,
                    UltimoStabilityScore = lastAnalysis?.StabilityScore ?? 0,
                    UltimoSafetyScore = lastAnalysis?.SafetyScore ?? 0,
                    LicenseStatus = license != null 
                        ? LicenseValidationDTO.FromLicense(license) 
                        : new LicenseValidationDTO()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting summary for vehicle {VehicleId}", vehicleId);
                throw;
            }
        }

        public async Task<List<VehicleAlertDTO>> GetVehicleAlertsAsync(int vehicleId)
        {
            try
            {
                var alerts = await _context.VehicleAlerts
                    .Where(a => a.VehicleId == vehicleId)
                    .OrderByDescending(a => a.CreatedAt)
                    .ToListAsync();

                return alerts.Select(a => a.ToDTO()).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting alerts for vehicle {VehicleId}", vehicleId);
                return new List<VehicleAlertDTO>();
            }
        }

        public async Task<bool> MarkAlertAsReadAsync(int vehicleId, int alertId)
        {
            try
            {
                var alert = await _context.VehicleAlerts
                    .FirstOrDefaultAsync(a => a.Id == alertId && a.VehicleId == vehicleId);

                if (alert == null)
                    return false;

                alert.IsRead = true;
                alert.ReadAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking alert as read for vehicle {VehicleId}", vehicleId);
                return false;
            }
        }

        public async Task<VehicleAlertDTO> CreateAlertAsync(int vehicleId, VehicleAlertDTO alertDto)
        {
            try
            {
                var alert = alertDto.ToEntity();
                alert.VehicleId = vehicleId;
                alert.CreatedAt = DateTime.UtcNow;
                alert.IsRead = false;

                _context.VehicleAlerts.Add(alert);
                await _context.SaveChangesAsync();

                return alert.ToDTO();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating alert for vehicle {VehicleId}", vehicleId);
                throw;
            }
        }

        public async Task<License> GetActiveLicenseAsync(int vehicleId)
        {
            return await _context.Licenses
                .Where(l => l.VehicleId == vehicleId && l.IsActive && l.ExpiresAt > DateTime.UtcNow)
                .OrderByDescending(l => l.ExpiresAt)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> HasValidLicenseAsync(int vehicleId)
        {
            var license = await GetActiveLicenseAsync(vehicleId);
            return license != null && license.IsActive && license.ExpiresAt > DateTime.UtcNow;
        }
    }
} 