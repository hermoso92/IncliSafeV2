using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using IncliSafe.Shared.Models.DTOs;
using IncliSafe.Shared.Models.Analysis;
using IncliSafeApi.Data;
using IncliSafeApi.Services.Interfaces;
using IncliSafe.Shared.Models.Entities;
using IncliSafe.Shared.Models.Analysis.Core;
using CorePrediction = IncliSafe.Shared.Models.Analysis.Core.Prediction;
using CorePredictionType = IncliSafe.Shared.Models.Analysis.Core.PredictionType;
using EntityPrediction = IncliSafe.Shared.Models.Entities.Prediction;

namespace IncliSafeApi.Services
{
    public class MaintenancePredictionService : IMaintenancePredictionService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<MaintenancePredictionService> _logger;

        public MaintenancePredictionService(
            ApplicationDbContext context,
            ILogger<MaintenancePredictionService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<AnalysisPrediction>> PredictMaintenanceAsync(int vehicleId)
        {
            try
            {
                var vehicle = await _context.Vehiculos.FindAsync(vehicleId)
                    ?? throw new InvalidOperationException($"Vehicle with ID {vehicleId} not found.");

                var latestAnalysis = await _context.DobackAnalyses
                    .Where(a => a.VehicleId == vehicleId)
                    .OrderByDescending(a => a.AnalysisDate)
                    .FirstOrDefaultAsync();

                if (latestAnalysis == null)
                    return new List<AnalysisPrediction>();

                var predictions = new List<AnalysisPrediction>();

                // Predict maintenance based on current scores
                if (latestAnalysis.MaintenanceScore < 0.7m)
                {
                    predictions.Add(new AnalysisPrediction
                    {
                        VehicleId = vehicleId,
                        Vehicle = vehicle,
                        PredictedAt = DateTime.UtcNow,
                        Type = PredictionType.Maintenance,
                        Probability = 0.8m,
                        Description = "Se recomienda mantenimiento preventivo basado en el índice de mantenimiento actual.",
                        PredictedValue = latestAnalysis.MaintenanceScore,
                        AnalysisId = latestAnalysis.Id,
                        Analysis = latestAnalysis
                    });
                }

                // Predict stability issues
                if (latestAnalysis.StabilityScore < 0.65m)
                {
                    predictions.Add(new AnalysisPrediction
                    {
                        VehicleId = vehicleId,
                        Vehicle = vehicle,
                        PredictedAt = DateTime.UtcNow,
                        Type = PredictionType.Stability,
                        Probability = 0.75m,
                        Description = "Posibles problemas de estabilidad detectados. Se recomienda revisión.",
                        PredictedValue = latestAnalysis.StabilityScore,
                        AnalysisId = latestAnalysis.Id,
                        Analysis = latestAnalysis
                    });
                }

                // Predict safety issues
                if (latestAnalysis.SafetyScore < 0.75m)
                {
                    predictions.Add(new AnalysisPrediction
                    {
                        VehicleId = vehicleId,
                        Vehicle = vehicle,
                        PredictedAt = DateTime.UtcNow,
                        Type = PredictionType.Safety,
                        Probability = 0.85m,
                        Description = "Indicadores de seguridad por debajo del umbral. Se requiere atención.",
                        PredictedValue = latestAnalysis.SafetyScore,
                        AnalysisId = latestAnalysis.Id,
                        Analysis = latestAnalysis
                    });
                }

                if (predictions.Any())
                {
                    _context.AnalysisPredictions.AddRange(predictions);
                    await _context.SaveChangesAsync();
                }

                return predictions;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error predicting maintenance for vehicle {Id}", vehicleId);
                throw;
            }
        }
    }
} 