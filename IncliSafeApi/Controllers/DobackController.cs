using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using IncliSafeApi.Data;
using IncliSafeApi.Services.Interfaces;
using System.Security.Claims;
using IncliSafe.Shared.Models;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Analysis;
using IncliSafe.Shared.Models.Patterns;
using System.Text.Json;
using IncliSafe.Shared.Models.DTOs;
using IncliSafe.Shared.Models.Analysis.Core;
using Anomaly = IncliSafe.Shared.Models.Analysis.Core.Anomaly;
using CoreMetrics = IncliSafe.Shared.Models.Analysis.Core.DashboardMetrics;
using IncliSafe.Shared.Models.Notifications;
using IncliSafe.Shared.Models.Entities;

namespace IncliSafeApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DobackController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DobackController> _logger;
        private readonly IKnowledgeBaseService _knowledgeService;
        private readonly IDobackService _dobackService;
        private readonly IDobackAnalysisService _analysisService;
        private readonly IPredictiveAnalysisService _predictiveService;

        public DobackController(
            ApplicationDbContext context,
            ILogger<DobackController> logger,
            IKnowledgeBaseService knowledgeService,
            IDobackService dobackService,
            IDobackAnalysisService analysisService,
            IPredictiveAnalysisService predictiveService)
        {
            _context = context;
            _logger = logger;
            _knowledgeService = knowledgeService;
            _dobackService = dobackService;
            _analysisService = analysisService;
            _predictiveService = predictiveService;
        }

        [HttpPost("analyze")]
        public async Task<ActionResult<DobackAnalysis>> AnalyzeFile([FromForm] int vehicleId, IFormFile file)
        {
            try
            {
                // Verificar permisos
                var vehicle = await _context.Vehiculos
                    .FirstOrDefaultAsync(v => v.Id == vehicleId && v.UserId == GetCurrentUserId());
                
                if (vehicle == null)
                    return NotFound("Vehículo no encontrado");

                // Leer y validar archivo
                using var reader = new StreamReader(file.OpenReadStream());
                var content = await reader.ReadToEndAsync();
                var dobackData = await ProcessDobackFile(content);
                
                if (dobackData == null || !dobackData.Any())
                    return BadRequest("Formato de archivo inválido");

                // Analizar datos
                var analysis = new DobackAnalysis
                {
                    VehicleId = vehicleId,
                    Timestamp = DateTime.UtcNow,
                    FileName = file.FileName,
                    Data = dobackData
                };

                // Detectar patrones
                var patterns = await _knowledgeService.GetPatterns();
                foreach (var pattern in patterns)
                {
                    if (await EvaluatePattern(dobackData, pattern))
                    {
                        analysis.DetectedPatterns.Add(CreatePattern(pattern, CalculateConfidence(dobackData, pattern)));
                    }
                }

                // Calcular resultados
                analysis.Result = await CreateAnalysisResult(dobackData.ToList(), analysis.DetectedPatterns.ToList());

                // Guardar análisis
                _context.DobackAnalyses.Add(analysis);
                await _context.SaveChangesAsync();

                return analysis;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al analizar archivo Doback");
                return StatusCode(500, "Error interno al procesar el archivo");
            }
        }

        [HttpGet("vehicle/{vehicleId}/analyses")]
        public async Task<ActionResult<List<DobackAnalysis>>> GetVehicleAnalyses(int vehicleId)
        {
            var vehicle = await _context.Vehiculos
                .FirstOrDefaultAsync(v => v.Id == vehicleId && v.UserId == GetCurrentUserId());
            
            if (vehicle == null)
                return NotFound();

            return await _context.DobackAnalyses
                .Where(a => a.VehicleId == vehicleId)
                .OrderByDescending(a => a.Timestamp)
                .Take(100) // Limitar a los últimos 100 análisis
                .ToListAsync();
        }

        [HttpGet("vehicle/{vehicleId}/latest")]
        public async Task<ActionResult<AnalysisResult>> GetLatestAnalysis(int vehicleId)
        {
            try
            {
                var result = await _dobackService.GetLatestAnalysisAsync(vehicleId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting latest analysis");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("vehicle/{vehicleId}/trends")]
        public async Task<ActionResult<Dictionary<string, double>>> GetTrends(int vehicleId)
        {
            var vehicle = await _context.Vehiculos
                .FirstOrDefaultAsync(v => v.Id == vehicleId && v.UserId == GetCurrentUserId());
            
            if (vehicle == null)
                return NotFound();

            var analyses = await _context.DobackAnalyses
                .Where(a => a.VehicleId == vehicleId)
                .OrderByDescending(a => a.Timestamp)
                .Take(100)
                .ToListAsync();

            return CalculateTrends(analyses);
        }

        [HttpGet("metrics")]
        public async Task<ActionResult<DashboardMetrics>> GetDashboardMetrics()
        {
            try
            {
                var metrics = await _dobackService.GetDashboardMetrics();
                return Ok(metrics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting dashboard metrics");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("alerts")]
        public async Task<ActionResult<IEnumerable<Alert>>> GetRecentAlerts()
        {
            var vehicleId = GetCurrentUserId();
            return Ok(await _dobackService.GetRecentAlerts(vehicleId));
        }

        [HttpGet("anomalies")]
        public async Task<ActionResult<IEnumerable<Anomaly>>> GetRecentAnomalies()
        {
            var vehicleId = GetCurrentUserId();
            return Ok(await _dobackService.GetRecentAnomalies(vehicleId));
        }

        [HttpGet("analysis/{vehicleId}")]
        public async Task<ActionResult<AnalysisResult>> GetAnalysisResult(int vehicleId)
        {
            try
            {
                var result = await _dobackService.GetAnalysisResult(vehicleId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("data")]
        public async Task<ActionResult<IEnumerable<DobackData>>> GetDobackData(DateTime? start = null, DateTime? end = null)
        {
            var vehicleId = GetCurrentUserId();
            var startDate = start ?? DateTime.UtcNow.AddDays(-7);
            var endDate = end ?? DateTime.UtcNow;
            
            return Ok(await _dobackService.GetDobackData(vehicleId, startDate, endDate));
        }

        [HttpGet("trends/{analysisId}")]
        public async Task<ActionResult<TrendAnalysis>> GetTrendAnalysis(int analysisId)
        {
            try
            {
                var trends = await _dobackService.GetTrendAnalysis(analysisId);
                return Ok(trends);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("predictions/{analysisId}")]
        public async Task<ActionResult<PredictionResult>> GetPredictions(int analysisId)
        {
            try
            {
                var predictions = await _dobackService.GetPredictions(analysisId);
                return Ok(predictions);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DobackAnalysis>> GetAnalysis(int id)
        {
            try
            {
                var analysis = await _dobackService.GetAnalysis(id);
                if (analysis == null)
                {
                    return NotFound();
                }
                return Ok(analysis);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting analysis {Id}", id);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("{id}/data")]
        public async Task<ActionResult<List<DobackData>>> GetDobackData(int id)
        {
            var data = await _dobackService.GetDobackDataAsync(id);
            return Ok(data);
        }

        [HttpPost("data")]
        public async Task<ActionResult<DobackData>> ProcessData([FromBody] DobackDataDto dto)
        {
            try
            {
                var data = new DobackData
                {
                    AccelerationX = Convert.ToDecimal(dto.AccelerationX),
                    AccelerationY = Convert.ToDecimal(dto.AccelerationY),
                    AccelerationZ = Convert.ToDecimal(dto.AccelerationZ),
                    Roll = Convert.ToDecimal(dto.Roll),
                    Pitch = Convert.ToDecimal(dto.Pitch),
                    Yaw = Convert.ToDecimal(dto.Yaw),
                    Speed = Convert.ToDecimal(dto.Speed),
                    StabilityIndex = Convert.ToDecimal(dto.StabilityIndex),
                    Temperature = Convert.ToDecimal(dto.Temperature),
                    Humidity = Convert.ToDecimal(dto.Humidity),
                    TimeAntWifi = Convert.ToDecimal(dto.TimeAntWifi),
                    Timestamp = dto.Timestamp
                };

                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing data");
                return BadRequest("Error processing data");
            }
        }

        [HttpPost]
        public async Task<ActionResult<DobackAnalysis>> CreateAnalysis(DobackAnalysis analysis)
        {
            try
            {
                analysis.Timestamp = DateTime.UtcNow;
                foreach (var data in analysis.Data)
                {
                    data.Timestamp = DateTime.UtcNow;
                }

                var result = await _dobackService.CreateAnalysisAsync(analysis);
                return CreatedAtAction(nameof(GetAnalysis), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear análisis");
                return StatusCode(500, "Error interno al crear el análisis");
            }
        }

        [HttpGet("vehicle/{vehicleId}/history")]
        public async Task<ActionResult<List<DobackAnalysis>>> GetAnalysisHistory(int vehicleId)
        {
            try
            {
                var history = await _dobackService.GetAnalysisHistoryAsync(vehicleId);
                return Ok(history);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener historial para vehículo {VehicleId}", vehicleId);
                return StatusCode(500, "Error interno al obtener el historial");
            }
        }

        [HttpGet("{id}/patterns")]
        public async Task<ActionResult<List<IncliSafe.Shared.Models.Patterns.DetectedPattern>>> GetDetectedPatterns(int id)
        {
            try
            {
                var patterns = await _dobackService.GetDetectedPatternsAsync(id);
                return Ok(patterns);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener patrones para análisis {Id}", id);
                return StatusCode(500, "Error interno al obtener patrones");
            }
        }

        [HttpGet("latest/{vehicleId}")]
        public async Task<ActionResult<DobackAnalysis>> GetLatestAnalysis(int vehicleId)
        {
            var analysis = await _context.DobackAnalyses
                .Where(a => a.VehicleId == vehicleId)
                .OrderByDescending(a => a.Timestamp)
                .FirstOrDefaultAsync();

            if (analysis == null)
                return NotFound();

            return Ok(analysis);
        }

        [HttpPost("{id}/process")]
        public async Task<ActionResult<DobackAnalysis>> ProcessData(int id, [FromBody] List<DobackData> data)
        {
            try
            {
                if (id == int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0"))
                {
                    return Forbid();
                }

                var analysis = await _dobackService.ProcessDobackDataAsync(data);
                return Ok(analysis);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing data for {Id}", id);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        private async Task<List<DobackData>> ProcessDobackFile(string content)
        {
            var dataList = new List<DobackData>();
            var lines = content.Split('\n');
            
            await Task.Run(() =>
            {
                foreach (var line in lines.Skip(1))
                {
                    var data = ParseDobackLine(line);
                    if (data != null)
                    {
                        dataList.Add(data);
                    }
                }
            });
            
            return dataList;
        }

        private DobackData? ParseDobackLine(string line)
        {
            try
            {
                var dataLine = line.Split(';');
                if (dataLine.Length < 22) return null;

                return new DobackData
                {
                    AccelerationX = Convert.ToDecimal(dataLine[0]),
                    AccelerationY = Convert.ToDecimal(dataLine[1]),
                    AccelerationZ = Convert.ToDecimal(dataLine[2]),
                    Roll = Convert.ToDecimal(dataLine[3]),
                    Pitch = Convert.ToDecimal(dataLine[4]),
                    Yaw = Convert.ToDecimal(dataLine[5]),
                    Speed = Convert.ToDecimal(dataLine[6]),
                    StabilityIndex = Convert.ToDecimal(dataLine[7]),
                    Temperature = Convert.ToDecimal(dataLine[8]),
                    Humidity = Convert.ToDecimal(dataLine[9]),
                    TimeAntWifi = Convert.ToDecimal(dataLine[10]),
                    Timestamp = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error parsing Doback line");
                return null;
            }
        }

        private async Task<bool> EvaluatePattern(List<DobackData> data, KnowledgePattern pattern)
        {
            if (data == null || !data.Any() || string.IsNullOrEmpty(pattern.Pattern))
                return false;

            try
            {
                var dataString = await Task.Run(() => JsonSerializer.Serialize(data));
                return await Task.Run(() => Regex.IsMatch(dataString, pattern.Pattern, RegexOptions.Compiled));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error evaluating pattern {PatternId}", pattern.Id);
                return false;
            }
        }

        private decimal CalculateConfidence(List<DobackData> data, KnowledgePattern pattern)
        {
            // Implementar cálculo de confianza basado en los datos y el patrón
            return 0.85M; // Ahora devolvemos un decimal literal
        }

        private async Task<AnalysisResult> CreateAnalysisResult(List<DobackData> data, List<IncliSafe.Shared.Models.Patterns.DetectedPattern> patterns)
        {
            var cycle = await _context.Cycles.FindAsync(data.First().CycleId);
            if (cycle == null)
                throw new InvalidOperationException("Cycle not found");

            return new AnalysisResult
            {
                StabilityScore = Convert.ToDecimal(data.Average(d => d.StabilityIndex)),
                SafetyScore = 0.9M,
                EfficiencyScore = 0.85M,
                MaintenanceScore = Convert.ToDecimal(data.Average(d => d.MaintenanceScore)),
                AnalysisTime = DateTime.UtcNow,
                VehicleId = cycle.VehicleId,
                Recommendations = patterns.SelectMany(p => p.RecommendedActions).ToList()
            };
        }

        private Dictionary<string, double> CalculateTrends(List<DobackAnalysis> analyses)
        {
            var data = analyses.SelectMany(a => a.Data).ToList();
            return new Dictionary<string, double>
            {
                { "stability", Convert.ToDouble(data.Average(d => d.StabilityIndex)) },
                { "safety", Convert.ToDouble(data.Average(d => d.SafetyScore)) }
            };
        }

        private int GetCurrentUserId()
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        }

        private IncliSafe.Shared.Models.Patterns.DetectedPattern CreatePattern(KnowledgePattern pattern, decimal confidence)
        {
            return new IncliSafe.Shared.Models.Patterns.DetectedPattern
            {
                PatternName = pattern.Name,
                Description = pattern.Description,
                ConfidenceScore = confidence,
                DetectedValues = new List<double>(),
                RecommendedActions = pattern.RecommendedActions,
                DetectionTime = DateTime.UtcNow,
                VehicleId = GetCurrentUserId()
            };
        }

        [HttpPost("analyze/{vehicleId}")]
        public async Task<ActionResult<DobackAnalysis>> AnalyzeDoback(int vehicleId, [FromBody] DobackAnalysisDTO analysisDto)
        {
            var analysis = await _dobackService.AnalyzeDobackAsync(vehicleId, analysisDto);
            return Ok(analysis);
        }

        [HttpPost("trends/{vehicleId}")]
        public async Task<ActionResult<TrendAnalysis>> AnalyzeTrends(int vehicleId, [FromBody] TrendAnalysisDTO trendDto)
        {
            var analysis = await _predictiveService.AnalyzeTrends(vehicleId, trendDto.StartDate, trendDto.EndDate);
            return Ok(analysis);
        }
    }
} 