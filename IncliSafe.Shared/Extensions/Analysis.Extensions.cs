using System;
using System.Collections.Generic;
using System.Linq;
using IncliSafe.Shared.Models.Analysis;
using IncliSafe.Shared.Models.Analysis.Core;
using IncliSafe.Shared.Models.DTOs;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Shared.Extensions
{
    public static class AnalysisExtensions
    {
        public static decimal CalculateStabilityScore(this DobackData data)
        {
            if (data == null) return 0;

            var metrics = data.AdditionalMetrics;
            if (!metrics.Any()) return 0;

            decimal roll = metrics.GetValueOrDefault("Roll", 0);
            decimal pitch = metrics.GetValueOrDefault("Pitch", 0);
            decimal yaw = metrics.GetValueOrDefault("Yaw", 0);

            return (Math.Abs(roll) + Math.Abs(pitch) + Math.Abs(yaw)) / 3;
        }

        public static bool IsStable(this DobackData data, decimal threshold = 15M)
        {
            if (data == null) return false;

            var metrics = data.AdditionalMetrics;
            if (!metrics.Any()) return false;

            decimal roll = metrics.GetValueOrDefault("Roll", 0);
            decimal pitch = metrics.GetValueOrDefault("Pitch", 0);
            decimal yaw = metrics.GetValueOrDefault("Yaw", 0);

            return Math.Abs(roll) < threshold &&
                   Math.Abs(pitch) < threshold &&
                   Math.Abs(yaw) < threshold;
        }

        public static decimal CalculateSafetyScore(this DobackData data)
        {
            if (data == null) return 0;

            var metrics = data.AdditionalMetrics;
            if (!metrics.Any()) return 0;

            decimal speed = metrics.GetValueOrDefault("Speed", 0);
            decimal acceleration = metrics.GetValueOrDefault("Acceleration", 0);
            decimal braking = metrics.GetValueOrDefault("Braking", 0);
            decimal stability = CalculateStabilityScore(data);

            return (speed + acceleration + braking + stability) / 4;
        }

        public static decimal CalculateMaintenanceScore(this DobackData data)
        {
            if (data == null) return 0;

            var metrics = data.AdditionalMetrics;
            if (!metrics.Any()) return 0;

            decimal temperature = metrics.GetValueOrDefault("Temperature", 0);
            decimal humidity = metrics.GetValueOrDefault("Humidity", 0);
            decimal vibration = metrics.GetValueOrDefault("Vibration", 0);
            decimal noise = metrics.GetValueOrDefault("Noise", 0);

            return (temperature + humidity + vibration + noise) / 4;
        }

        public static TrendDirection CalculateTrendDirection(this IEnumerable<decimal> values, int minSamples = 3)
        {
            var data = values.ToList();
            if (data.Count < minSamples) return TrendDirection.Unknown;

            decimal firstHalf = data.Take(data.Count / 2).Average();
            decimal secondHalf = data.Skip(data.Count / 2).Average();
            decimal difference = secondHalf - firstHalf;
            decimal threshold = 0.1M;

            if (Math.Abs(difference) < threshold) return TrendDirection.Stable;
            if (difference > 0) return TrendDirection.Increasing;
            return TrendDirection.Decreasing;
        }

        public static PerformanceTrend CalculatePerformanceTrend(this IEnumerable<decimal> scores, decimal threshold = 0.1M)
        {
            var data = scores.ToList();
            if (!data.Any()) return PerformanceTrend.Unknown;

            decimal average = data.Average();
            decimal stdDev = CalculateStandardDeviation(data);
            decimal lastValue = data.Last();

            if (stdDev > threshold * 2) return PerformanceTrend.Inconsistent;
            if (lastValue < average - threshold) return PerformanceTrend.Critical;
            if (lastValue > average + threshold) return PerformanceTrend.Improving;
            if (Math.Abs(lastValue - average) <= threshold) return PerformanceTrend.Stable;
            return PerformanceTrend.Declining;
        }

        private static decimal CalculateStandardDeviation(List<decimal> values)
        {
            if (!values.Any()) return 0;

            decimal avg = values.Average();
            decimal sumOfSquaresOfDifferences = values.Sum(val => (val - avg) * (val - avg));
            return (decimal)Math.Sqrt((double)(sumOfSquaresOfDifferences / values.Count));
        }

        public static List<TrendData> CalculateTrend(this IEnumerable<DobackData> data, 
            Func<DobackData, decimal> metricSelector,
            string metricLabel)
        {
            return data.Select(d => new TrendData
            {
                Timestamp = d.Timestamp,
                Value = metricSelector(d),
                Label = metricLabel,
                Metrics = new Dictionary<string, decimal>
                {
                    ["Raw"] = d.Value,
                    ["Normalized"] = NormalizeValue(metricSelector(d))
                }
            }).ToList();
        }

        private static decimal NormalizeValue(decimal value, decimal min = 0M, decimal max = 100M)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        public static Dictionary<string, decimal> ExtractMetrics(this DobackData data)
        {
            var metrics = new Dictionary<string, decimal>
            {
                ["StabilityScore"] = data.CalculateStabilityScore(),
                ["SafetyScore"] = data.CalculateSafetyScore(),
                ["MaintenanceScore"] = data.CalculateMaintenanceScore()
            };

            foreach (var kvp in data.AdditionalMetrics)
            {
                metrics[kvp.Key] = kvp.Value;
            }

            return metrics;
        }
    }
} 