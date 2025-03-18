using System;
using System.Collections.Generic;
using System.Linq;
using IncliSafe.Shared.Models.Analysis.Core;

namespace IncliSafe.Client.Services.Extensions
{
    public static class DobackAnalysisExtensions
    {
        public static DobackAnalysis ToDobackAnalysis(this List<DobackData> data, int vehicleId)
        {
            var analysis = new DobackAnalysis
            {
                VehicleId = vehicleId,
                AnalysisDate = DateTime.UtcNow,
                Data = data,
                StabilityScore = CalculateStabilityScore(data),
                SafetyScore = CalculateSafetyScore(data),
                MaintenanceScore = CalculateMaintenanceScore(data)
            };

            return analysis;
        }

        private static double CalculateStabilityScore(List<DobackData> data)
        {
            if (data == null || !data.Any())
                return 0;

            // Implementar lógica para calcular el score de estabilidad
            return 0.0;
        }

        private static double CalculateSafetyScore(List<DobackData> data)
        {
            if (data == null || !data.Any())
                return 0;

            // Implementar lógica para calcular el score de seguridad
            return 0.0;
        }

        private static double CalculateMaintenanceScore(List<DobackData> data)
        {
            if (data == null || !data.Any())
                return 0;

            // Implementar lógica para calcular el score de mantenimiento
            return 0.0;
        }

        public static decimal GetValue(this DobackAnalysis analysis, string property)
        {
            return analysis.Data.GetValue(property);
        }

        public static decimal GetAccelerationX(this DobackAnalysis analysis) => analysis.GetValue("AccelerationX");
        public static decimal GetAccelerationY(this DobackAnalysis analysis) => analysis.GetValue("AccelerationY");
        public static decimal GetAccelerationZ(this DobackAnalysis analysis) => analysis.GetValue("AccelerationZ");
        public static decimal GetRoll(this DobackAnalysis analysis) => analysis.GetValue("Roll");
        public static decimal GetPitch(this DobackAnalysis analysis) => analysis.GetValue("Pitch");
        public static decimal GetYaw(this DobackAnalysis analysis) => analysis.GetValue("Yaw");
        public static decimal GetSpeed(this DobackAnalysis analysis) => analysis.GetValue("Speed");
        public static decimal GetStabilityIndex(this DobackAnalysis analysis) => analysis.GetValue("StabilityIndex");
        public static decimal GetTemperature(this DobackAnalysis analysis) => analysis.GetValue("Temperature");
        public static decimal GetHumidity(this DobackAnalysis analysis) => analysis.GetValue("Humidity");
        public static decimal GetTimeAntWifi(this DobackAnalysis analysis) => analysis.GetValue("TimeAntWifi");
        public static decimal GetUSCycle1(this DobackAnalysis analysis) => analysis.GetValue("USCycle1");
        public static decimal GetUSCycle2(this DobackAnalysis analysis) => analysis.GetValue("USCycle2");
        public static decimal GetUSCycle3(this DobackAnalysis analysis) => analysis.GetValue("USCycle3");
        public static decimal GetUSCycle4(this DobackAnalysis analysis) => analysis.GetValue("USCycle4");
        public static decimal GetUSCycle5(this DobackAnalysis analysis) => analysis.GetValue("USCycle5");
        public static decimal GetMicrosCleanCAN(this DobackAnalysis analysis) => analysis.GetValue("MicrosCleanCAN");
        public static decimal GetMicrosSD(this DobackAnalysis analysis) => analysis.GetValue("MicrosSD");
        public static decimal GetSteer(this DobackAnalysis analysis) => analysis.GetValue("Steer");
        public static decimal GetSafetyScore(this DobackAnalysis analysis) => analysis.GetValue("SafetyScore");
        public static decimal GetMaintenanceScore(this DobackAnalysis analysis) => analysis.GetValue("MaintenanceScore");

        public static decimal GetAverageValue(this ICollection<DobackData> data, Func<DobackData, decimal> selector)
        {
            return data.Any() ? data.Average(selector) : 0;
        }

        public static decimal GetTimeAntWifi(this ICollection<DobackData> data) => GetAverageValue(data, d => d.TimeAntWifi);
        public static decimal GetUSCycle1(this ICollection<DobackData> data) => GetAverageValue(data, d => d.USCycle1);
        public static decimal GetUSCycle2(this ICollection<DobackData> data) => GetAverageValue(data, d => d.USCycle2);
        public static decimal GetUSCycle3(this ICollection<DobackData> data) => GetAverageValue(data, d => d.USCycle3);
        public static decimal GetUSCycle4(this ICollection<DobackData> data) => GetAverageValue(data, d => d.USCycle4);
        public static decimal GetUSCycle5(this ICollection<DobackData> data) => GetAverageValue(data, d => d.USCycle5);
        public static decimal GetMicrosCleanCAN(this ICollection<DobackData> data) => GetAverageValue(data, d => d.MicrosCleanCAN);
        public static decimal GetMicrosSD(this ICollection<DobackData> data) => GetAverageValue(data, d => d.MicrosSD);
        public static decimal GetSteer(this ICollection<DobackData> data) => GetAverageValue(data, d => d.Steer);
        public static decimal GetRoll(this ICollection<DobackData> data) => GetAverageValue(data, d => d.Roll);
        public static decimal GetPitch(this ICollection<DobackData> data) => GetAverageValue(data, d => d.Pitch);
        public static decimal GetYaw(this ICollection<DobackData> data) => GetAverageValue(data, d => d.Yaw);
        public static decimal GetSpeed(this ICollection<DobackData> data) => GetAverageValue(data, d => d.Speed);
        public static decimal GetStabilityIndex(this ICollection<DobackData> data) => GetAverageValue(data, d => d.StabilityIndex);
        public static decimal GetSafetyScore(this ICollection<DobackData> data) => GetAverageValue(data, d => d.SafetyScore);
        public static decimal GetMaintenanceScore(this ICollection<DobackData> data) => GetAverageValue(data, d => d.MaintenanceScore);
        public static decimal GetTemperature(this ICollection<DobackData> data) => GetAverageValue(data, d => d.Temperature);
        public static decimal GetHumidity(this ICollection<DobackData> data) => GetAverageValue(data, d => d.Humidity);
        public static decimal GetAccelerationX(this ICollection<DobackData> data) => GetAverageValue(data, d => d.AccelerationX);
        public static decimal GetAccelerationY(this ICollection<DobackData> data) => GetAverageValue(data, d => d.AccelerationY);
        public static decimal GetAccelerationZ(this ICollection<DobackData> data) => GetAverageValue(data, d => d.AccelerationZ);

        public static decimal GetAverageAccelerationX(this DobackAnalysis analysis) => 
            analysis.Data.Average(d => d.AccelerationX);
        public static decimal GetAverageAccelerationY(this DobackAnalysis analysis) => 
            analysis.Data.Average(d => d.AccelerationY);
        public static decimal GetAverageAccelerationZ(this DobackAnalysis analysis) => 
            analysis.Data.Average(d => d.AccelerationZ);

        public static decimal GetAverageScore(this DobackAnalysis analysis)
        {
            return (analysis.StabilityScore + analysis.SafetyScore + analysis.MaintenanceScore) / 3;
        }

        public static decimal GetAverageIndex(this DobackAnalysis analysis)
        {
            return (analysis.StabilityIndex + analysis.SafetyIndex + analysis.MaintenanceIndex) / 3;
        }

        public static List<TrendData> GetTrendData(this DobackAnalysis analysis)
        {
            return analysis.Data.Select(d => new TrendData
            {
                Timestamp = d.Timestamp,
                Value = (d.StabilityIndex + d.SafetyIndex + d.MaintenanceIndex) / 3
            }).ToList();
        }

        public static List<TrendData> GetStabilityTrend(this DobackAnalysis analysis)
        {
            return analysis.Data.Select(d => new TrendData
            {
                Timestamp = d.Timestamp,
                Value = d.StabilityIndex
            }).ToList();
        }

        public static List<TrendData> GetSafetyTrend(this DobackAnalysis analysis)
        {
            return analysis.Data.Select(d => new TrendData
            {
                Timestamp = d.Timestamp,
                Value = d.SafetyIndex
            }).ToList();
        }

        public static List<TrendData> GetMaintenanceTrend(this DobackAnalysis analysis)
        {
            return analysis.Data.Select(d => new TrendData
            {
                Timestamp = d.Timestamp,
                Value = d.MaintenanceIndex
            }).ToList();
        }

        public static List<Anomaly> GetCriticalAnomalies(this DobackAnalysis analysis)
        {
            return analysis.Anomalies
                .Where(a => a.Severity > 0.7m)
                .OrderByDescending(a => a.Severity)
                .ToList();
        }

        public static List<AnalysisPrediction> GetHighProbabilityPredictions(this DobackAnalysis analysis)
        {
            return analysis.Predictions
                .Where(p => p.Probability > 0.8m)
                .OrderByDescending(p => p.Probability)
                .ToList();
        }
    }
} 