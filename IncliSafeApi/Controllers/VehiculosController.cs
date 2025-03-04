using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IncliSafeApi.Data;
using IncliSafe.Shared.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using System.Text.Json.Serialization;
using IncliSafe.Shared.Models.Entities;
using System.Security.Claims;
using IncliSafe.Shared.Models.DTOs;
using IncliSafeApi.Services.Interfaces;
using System.Linq;
using IncliSafe.Shared.Models.Analysis;
using IncliSafe.Shared.Models.Notifications;
using IncliSafe.Shared.Exceptions;
using IncliSafe.Shared.Models.Analysis.Core;
namespace IncliSafeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VehiculosController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;
        private readonly IDobackAnalysisService _analysisService;
        private readonly ILogger<VehiculosController> _logger;
        private readonly IMeterService _metricsService;
        private readonly ITrendAnalysisService _trendAnalysisService;
        private readonly IMaintenancePredictionService _maintenancePredictionService;
        private readonly ApplicationDbContext _context;

        public VehiculosController(
            IVehicleService vehicleService,
            IDobackAnalysisService analysisService,
            ILogger<VehiculosController> logger,
            IMeterService metricsService,
            ITrendAnalysisService trendAnalysisService,
            IMaintenancePredictionService maintenancePredictionService,
            ApplicationDbContext context)
        {
            _vehicleService = vehicleService;
            _analysisService = analysisService;
            _logger = logger;
            _metricsService = metricsService;
            _trendAnalysisService = trendAnalysisService;
            _maintenancePredictionService = maintenancePredictionService;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VehiculoDTO>>> GetVehiculos()
        {
            try
            {
                var userId = GetCurrentUserId();
                var vehicles = await _vehicleService.GetVehiclesAsync(userId);
                return Ok(vehicles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting vehicles");
                return StatusCode(500, "Error interno al obtener vehículos");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VehiculoDTO>> GetVehicle(int id)
        {
            if (id != int.Parse(GetCurrentUserId()))
            {
                return Forbid();
            }

            var vehicle = await _vehicleService.GetVehicleAsync(id, GetCurrentUserId());
            if (vehicle == null)
            {
                return NotFound();
            }

            return Ok(vehicle);
        }

        [HttpPost]
        public async Task<ActionResult<VehiculoDTO>> CreateVehicle(VehiculoDTO dto)
        {
            var vehicle = await _vehicleService.CreateVehicleAsync(dto);
            return CreatedAtAction(nameof(GetVehicle), new { id = vehicle.Id }, vehicle);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVehicle(int id, VehiculoDTO dto)
        {
            if (id != int.Parse(GetCurrentUserId()))
            {
                return Forbid();
            }

            var result = await _vehicleService.UpdateVehicleAsync(id, dto);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehiculo(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                var success = await _vehicleService.DeleteVehicleAsync(id, userId);

                if (!success)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting vehicle {VehicleId}", id);
                return StatusCode(500, "Error interno al eliminar el vehículo");
            }
        }

        [HttpGet("{id}/license/validate")]
        public async Task<ActionResult<LicenseValidationDTO>> ValidateLicense(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                var vehicle = await _vehicleService.GetVehicleAsync(id, userId);
                if (vehicle == null)
                    return NotFound();

                var license = await _vehicleService.GetLicenseAsync(id);
                if (license == null)
                    return NotFound();

                return Ok(LicenseValidationDTO.FromLicense(license));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating license for vehicle {VehicleId}", id);
                return StatusCode(500, "Error interno al validar la licencia");
            }
        }

        [HttpGet("{id}/inspecciones")]
        public async Task<ActionResult<IEnumerable<InspeccionDTO>>> GetInspecciones(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                var vehicle = await _vehicleService.GetVehicleAsync(id, userId);
                if (vehicle == null)
                    return NotFound();

                var inspecciones = await _vehicleService.GetInspeccionesAsync(id);
                return Ok(inspecciones);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting inspections for vehicle {VehicleId}", id);
                return StatusCode(500, "Error interno al obtener inspecciones");
            }
        }

        [HttpPost("{id}/inspecciones")]
        public async Task<IActionResult> AddInspeccion(int id, InspeccionDTO inspeccionDto)
        {
            try
            {
                var userId = GetCurrentUserId();
                var vehicle = await _vehicleService.GetVehicleAsync(id, userId);
                if (vehicle == null)
                    return NotFound();

                var success = await _vehicleService.AddInspeccionAsync(id, inspeccionDto);
                if (!success)
                    return BadRequest();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding inspection for vehicle {VehicleId}", id);
                return StatusCode(500, "Error interno al agregar inspección");
            }
        }

        [HttpGet("{id}/license")]
        public async Task<ActionResult<LicenseDTO>> GetLicense(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                var vehicle = await _vehicleService.GetVehicleAsync(id, userId);
                if (vehicle == null)
                    return NotFound();

                var license = await _vehicleService.GetLicenseAsync(id);
                if (license == null)
                    return NotFound();

                return Ok(license);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting license for vehicle {VehicleId}", id);
                return StatusCode(500, "Error interno al obtener la licencia");
            }
        }

        [HttpPut("{id}/license")]
        public async Task<IActionResult> UpdateLicense(int id, LicenseDTO licenseDto)
        {
            try
            {
                var userId = GetCurrentUserId();
                var vehicle = await _vehicleService.GetVehicleAsync(id, userId);
                if (vehicle == null)
                    return NotFound();

                var success = await _vehicleService.UpdateLicenseAsync(id, licenseDto);
                if (!success)
                    return BadRequest();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating license for vehicle {VehicleId}", id);
                return StatusCode(500, "Error interno al actualizar la licencia");
            }
        }

        [HttpPost("{id}/license")]
        public async Task<ActionResult<LicenseDTO>> CreateLicense(int id, [FromBody] LicenseType type)
        {
            try
            {
                var userId = GetCurrentUserId();
                var vehicle = await _vehicleService.GetVehicleAsync(id, userId);
                if (vehicle == null)
                    return NotFound();

                var license = await _vehicleService.CreateLicenseAsync(id, type);
                return CreatedAtAction(nameof(GetLicense), new { id }, license);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating license for vehicle {VehicleId}", id);
                return StatusCode(500, "Error interno al crear la licencia");
            }
        }

        [HttpGet("{id}/stats")]
        public async Task<ActionResult<VehicleStatsDTO>> GetVehicleStats(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                var vehicle = await _vehicleService.GetVehicleAsync(id, userId);
                if (vehicle == null)
                    return NotFound();

                var stats = await _vehicleService.GetVehicleStatsAsync(id);
                return Ok(stats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting stats for vehicle {VehicleId}", id);
                return StatusCode(500, "Error interno al obtener estadísticas");
            }
        }

        [HttpGet("{id}/stats/monthly")]
        public async Task<ActionResult<List<MonthlyStatsDTO>>> GetMonthlyStats(
            int id, 
            [FromQuery] DateTime? startDate, 
            [FromQuery] DateTime? endDate)
        {
            try
            {
                var userId = GetCurrentUserId();
                var vehicle = await _vehicleService.GetVehicleAsync(id, userId);
                if (vehicle == null)
                    return NotFound();

                var start = startDate ?? DateTime.UtcNow.AddMonths(-6);
                var end = endDate ?? DateTime.UtcNow;

                var stats = await _vehicleService.GetMonthlyStatsAsync(id, start, end);
                return Ok(stats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting monthly stats for vehicle {VehicleId}", id);
                return StatusCode(500, "Error interno al obtener estadísticas mensuales");
            }
        }

        [HttpGet("{id}/summary")]
        public async Task<ActionResult<VehicleSummaryDTO>> GetVehicleSummary(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                var vehicle = await _vehicleService.GetVehicleAsync(id, userId);
                if (vehicle == null)
                    return NotFound();

                var summary = await _vehicleService.GetVehicleSummaryAsync(id);
                return Ok(summary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting summary for vehicle {VehicleId}", id);
                return StatusCode(500, "Error interno al obtener resumen del vehículo");
            }
        }

        [HttpGet("{id}/alerts")]
        public async Task<ActionResult<List<VehicleAlertDTO>>> GetVehicleAlerts(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                var vehicle = await _vehicleService.GetVehicleAsync(id, userId);
                if (vehicle == null)
                    return NotFound();

                var alerts = await _vehicleService.GetVehicleAlertsAsync(id);
                return Ok(alerts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting alerts for vehicle {VehicleId}", id);
                return StatusCode(500, "Error interno al obtener alertas");
            }
        }

        [HttpPost("{id}/alerts")]
        public async Task<ActionResult<VehicleAlertDTO>> CreateAlert(int id, VehicleAlertDTO alertDto)
        {
            try
            {
                var userId = GetCurrentUserId();
                var vehicle = await _vehicleService.GetVehicleAsync(id, userId);
                if (vehicle == null)
                    return NotFound();

                alertDto.VehicleId = id;
                var alert = await _vehicleService.CreateAlertAsync(id, alertDto);
                return CreatedAtAction(nameof(GetVehicleAlerts), new { id }, alert);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating alert for vehicle {VehicleId}", id);
                return StatusCode(500, "Error interno al crear alerta");
            }
        }

        [HttpPut("{id}/alerts/{alertId}/read")]
        public async Task<IActionResult> MarkAlertAsRead(int id, int alertId)
        {
            try
            {
                var userId = GetCurrentUserId();
                var vehicle = await _vehicleService.GetVehicleAsync(id, userId);
                if (vehicle == null)
                    return NotFound();

                var success = await _vehicleService.MarkAlertAsReadAsync(id, alertId);
                if (!success)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking alert as read for vehicle {VehicleId}", id);
                return StatusCode(500, "Error interno al marcar alerta como leída");
            }
        }

        [HttpPost("{id}/alerts/generate")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> GenerateAlerts(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                var vehicle = await _vehicleService.GetVehicleAsync(id, userId);
                if (vehicle == null)
                    return NotFound();

                var alertService = HttpContext.RequestServices.GetRequiredService<IAlertGenerationService>();
                
                await alertService.GenerateInspectionAlertAsync(id);
                await alertService.GenerateMaintenanceAlertAsync(id);
                await alertService.GenerateLicenseExpirationAlertAsync(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating alerts for vehicle {VehicleId}", id);
                return StatusCode(500, "Error interno al generar alertas");
            }
        }

        [HttpGet("{id}/metrics")]
        public async Task<ActionResult<VehicleMetricsDTO>> GetVehicleMetrics(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                var vehicle = await _vehicleService.GetVehicleAsync(id, userId);
                if (vehicle == null)
                    return NotFound();

                var metrics = await _metricsService.GetVehicleMetricsAsync(id);
                return Ok(metrics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting metrics for vehicle {VehicleId}", id);
                return StatusCode(500, "Error interno al obtener métricas del vehículo");
            }
        }

        [HttpGet("{id}/trend-analysis")]
        public async Task<ActionResult<TrendAnalysisEntity>> GetTrendAnalysis(int id)
        {
            try
            {
                var trends = await _analysisService.GetTrendAnalysis(id);
                return Ok(trends);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting trend analysis for vehicle {Id}", id);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("{id}/trend-metrics")]
        public async Task<ActionResult<List<TrendMetric>>> GetTrendMetrics(int id)
        {
            try
            {
                var metrics = await _metricsService.GetTrendMetricsAsync(id);
                return Ok(metrics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting trend metrics for vehicle {Id}", id);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("{id}/maintenance-prediction")]
        public async Task<ActionResult<MaintenancePredictionDTO>> GetMaintenancePrediction(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                var vehicle = await _vehicleService.GetVehicleAsync(id, userId);
                if (vehicle == null)
                    return NotFound();

                var prediction = await _maintenancePredictionService.PredictMaintenanceAsync(id);
                return Ok(prediction);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting maintenance prediction for vehicle {VehicleId}", id);
                return StatusCode(500, "Error interno al obtener predicción de mantenimiento");
            }
        }

        [HttpGet("{id}/maintenance-predictions")]
        public async Task<ActionResult<List<MaintenancePredictionDTO>>> GetMaintenancePredictionHistory(
            int id,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate)
        {
            try
            {
                var userId = GetCurrentUserId();
                var vehicle = await _vehicleService.GetVehicleAsync(id, userId);
                if (vehicle == null)
                    return NotFound();

                var start = startDate ?? DateTime.UtcNow.AddMonths(-1);
                var end = endDate ?? DateTime.UtcNow;

                var predictions = await _context.Predictions
                    .Where(p => p.VehicleId == id &&
                           p.PredictionType == PredictionType.Maintenance &&
                           p.CreatedAt >= start &&
                           p.CreatedAt <= end)
                    .OrderByDescending(p => p.CreatedAt)
                    .Select(p => new MaintenancePredictionDTO
                    {
                        VehicleId = p.VehicleId,
                        PredictedMaintenanceDate = p.PredictedDate,
                        MaintenanceProbability = p.Probability,
                        RiskLevel = p.RiskLevel,
                        Recommendations = p.Recommendations.Split('|').ToList()
                    })
                    .ToListAsync();

                return Ok(predictions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting maintenance prediction history for vehicle {VehicleId}", id);
                return StatusCode(500, "Error interno al obtener historial de predicciones");
            }
        }

        [HttpGet("{id}/analyses")]
        public async Task<ActionResult<List<DobackAnalysis>>> GetAnalyses(int id)
        {
            try
            {
                var analyses = await _analysisService.GetAnalysesAsync(id);
                return Ok(analyses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting analyses for vehicle {Id}", id);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("{id}/predictions")]
        public async Task<ActionResult<List<IncliSafe.Shared.Models.Analysis.Core.Prediction>>> GetPredictions(int id)
        {
            try
            {
                var predictions = await _analysisService.GetPredictionsAsync(id);
                return Ok(predictions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting predictions for vehicle {Id}", id);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("{id}/anomalies")]
        public async Task<ActionResult<List<Anomaly>>> GetAnomalies(int id)
        {
            try
            {
                var anomalies = await _analysisService.GetAnomaliesAsync(id);
                return Ok(anomalies);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting anomalies for vehicle {Id}", id);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        private string GetCurrentUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0";
        }

        private async Task<bool> VehiculoExists(int id)
        {
            return await _vehicleService.ExistsAsync(id);
        }
    }
}

