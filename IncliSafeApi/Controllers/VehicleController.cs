using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IncliSafe.Shared.Models.Analysis;
using IncliSafe.Shared.Models.Entities;
using IncliSafe.Shared.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using IncliSafeApi.Services;

namespace IncliSafeApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;
        private readonly IPredictiveAnalysisService _analysisService;
        private readonly ILogger<VehicleController> _logger;

        public VehicleController(
            IVehicleService vehicleService,
            IPredictiveAnalysisService analysisService,
            ILogger<VehicleController> logger)
        {
            _vehicleService = vehicleService;
            _analysisService = analysisService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<Vehicle>>> GetVehicles()
        {
            try
            {
                var vehicles = await _vehicleService.GetVehiculosAsync();
                return Ok(vehicles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting vehicles");
                return StatusCode(500, "Internal server error while retrieving vehicles");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Vehicle>> GetVehicle(int id)
        {
            try
            {
                var vehicle = await _vehicleService.GetVehiculoAsync(id);
                if (vehicle == null)
                {
                    return NotFound($"Vehicle with ID {id} not found");
                }
                return Ok(vehicle);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting vehicle {VehicleId}", id);
                return StatusCode(500, $"Internal server error while retrieving vehicle {id}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Vehicle>> CreateVehicle([FromBody] VehiculoDTO vehiculo)
        {
            try
            {
                var createdVehicle = await _vehicleService.CreateVehiculoAsync(vehiculo);
                return CreatedAtAction(nameof(GetVehicle), new { id = createdVehicle.Id }, createdVehicle);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating vehicle");
                return StatusCode(500, "Internal server error while creating vehicle");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Vehicle>> UpdateVehicle(int id, [FromBody] VehiculoDTO vehiculo)
        {
            try
            {
                var updatedVehicle = await _vehicleService.UpdateVehiculoAsync(id, vehiculo);
                return Ok(updatedVehicle);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating vehicle {VehicleId}", id);
                return StatusCode(500, $"Internal server error while updating vehicle {id}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteVehicle(int id)
        {
            try
            {
                var result = await _vehicleService.DeleteVehiculoAsync(id);
                if (!result)
                {
                    return NotFound($"Vehicle with ID {id} not found");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting vehicle {VehicleId}", id);
                return StatusCode(500, $"Internal server error while deleting vehicle {id}");
            }
        }

        [HttpGet("{vehicleId}/anomalies")]
        public async Task<ActionResult<List<Anomaly>>> GetAnomalies(int vehicleId)
        {
            try
            {
                var anomalies = await _vehicleService.GetAnomaliesAsync(vehicleId);
                return Ok(anomalies);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting anomalies for vehicle {VehicleId}", vehicleId);
                return StatusCode(500, $"Internal server error while retrieving anomalies for vehicle {vehicleId}");
            }
        }

        [HttpGet("{vehicleId}/predictions")]
        public async Task<ActionResult<List<AnalysisPrediction>>> GetPredictions(int vehicleId)
        {
            try
            {
                var predictions = await _vehicleService.GetPredictionsAsync(vehicleId);
                return Ok(predictions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting predictions for vehicle {VehicleId}", vehicleId);
                return StatusCode(500, $"Internal server error while retrieving predictions for vehicle {vehicleId}");
            }
        }

        [HttpGet("{vehicleId}/trends")]
        public async Task<ActionResult<TrendAnalysis>> GetTrendAnalysis(int vehicleId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var analysis = await _vehicleService.GetTrendAnalysisAsync(vehicleId, startDate, endDate);
                return Ok(analysis);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting trend analysis for vehicle {VehicleId}", vehicleId);
                return StatusCode(500, $"Internal server error while retrieving trend analysis for vehicle {vehicleId}");
            }
        }

        [HttpPost("{id}/predictions")]
        public async Task<ActionResult<AnalysisPrediction>> GeneratePrediction(int id)
        {
            var vehicle = await _vehicleService.GetVehicleByIdAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }

            var prediction = await _analysisService.GeneratePredictionAsync(vehicle);
            return Ok(prediction);
        }
    }
} 