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

        public DobackController(
            ApplicationDbContext context,
            ILogger<DobackController> logger,
            IKnowledgeBaseService knowledgeService,
            IDobackService dobackService)
        {
            _context = context;
            _logger = logger;
            _knowledgeService = knowledgeService;
            _dobackService = dobackService;
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
            var vehicle = await _context.Vehiculos
                .FirstOrDefaultAsync(v => v.Id == vehicleId && v.UserId == GetCurrentUserId());
            
            if (vehicle == null)
                return NotFound();

            var latest = await _context.DobackAnalyses
                .Where(a => a.VehicleId == vehicleId)
                .OrderByDescending(a => a.Timestamp)
                .FirstOrDefaultAsync();

            return latest?.Result ?? new AnalysisResult();
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
                return BadRequest(new { message = ex.Message });
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
                    AccelerometerX = Convert.ToDouble(dataLine[0]),
                    AccelerometerY = Convert.ToDouble(dataLine[1]),
                    AccelerometerZ = Convert.ToDouble(dataLine[2]),
                    Roll = Convert.ToDouble(dataLine[3]),
                    Pitch = Convert.ToDouble(dataLine[4]),
                    Yaw = Convert.ToDouble(dataLine[5]),
                    Speed = Convert.ToDouble(dataLine[6]),
                    StabilityIndex = Convert.ToDecimal(dataLine[7]),
                    Temperature = Convert.ToDouble(dataLine[8]),
                    Humidity = Convert.ToDouble(dataLine[9]),
                    TimeAntWifi = Convert.ToDouble(dataLine[10]),
                    USCycle1 = Convert.ToDouble(dataLine[11]),
                    USCycle2 = Convert.ToDouble(dataLine[12]),
                    USCycle3 = Convert.ToDouble(dataLine[13]),
                    USCycle4 = Convert.ToDouble(dataLine[14]),
                    USCycle5 = Convert.ToDouble(dataLine[15]),
                    AccMagnitude = Convert.ToDouble(dataLine[16]),
                    MicrosCleanCAN = Convert.ToDouble(dataLine[17]),
                    MicrosSD = Convert.ToDouble(dataLine[18]),
                    ErrorsCAN = int.Parse(dataLine[19]),
                    Steer = Convert.ToDouble(dataLine[20]),
                    Timestamp = DateTime.UtcNow,
                    SafetyScore = 0.9M,
                    MaintenanceScore = 0.85M
                };
            }
            catch
            {
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

        private async Task<AnalysisResult> CreateAnalysisResult(List<DobackData> data, List<DetectedPattern> patterns)
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

        private DetectedPattern CreatePattern(KnowledgePattern pattern, decimal confidence)
        {
            return new DetectedPattern
            {
                PatternName = pattern.Name,
                Description = pattern.Description,
                ConfidenceScore = confidence,
                DetectedValues = new List<string> { "value1", "value2" },
                RecommendedActions = pattern.RecommendedActions,
                DetectionTime = DateTime.UtcNow,
                VehicleId = GetCurrentUserId()
            };
        }
    }
} 