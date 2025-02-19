using System;
using System.Collections.Generic;
using System.Linq;
using IncliSafe.Shared.Models.Analysis;
using IncliSafe.Shared.Models.Analysis.Core;

namespace IncliSafe.Client.Services.Extensions
{
    public static class DobackDataExtensions
    {
        public static double GetPropertyValue(this DobackData data, string propertyName)
        {
            return propertyName switch
            {
                "AccelerationX" => Convert.ToDouble(data.AccelerationX),
                "AccelerationY" => Convert.ToDouble(data.AccelerationY),
                "AccelerationZ" => Convert.ToDouble(data.AccelerationZ),
                "Roll" => Convert.ToDouble(data.Roll),
                "Pitch" => Convert.ToDouble(data.Pitch),
                "Yaw" => Convert.ToDouble(data.Yaw),
                "Speed" => Convert.ToDouble(data.Speed),
                "StabilityIndex" => Convert.ToDouble(data.StabilityIndex),
                "Temperature" => Convert.ToDouble(data.Temperature),
                "Humidity" => Convert.ToDouble(data.Humidity),
                "TimeAntWifi" => Convert.ToDouble(data.TimeAntWifi),
                "USCycle1" => Convert.ToDouble(data.USCycle1),
                "USCycle2" => Convert.ToDouble(data.USCycle2),
                "USCycle3" => Convert.ToDouble(data.USCycle3),
                "USCycle4" => Convert.ToDouble(data.USCycle4),
                "USCycle5" => Convert.ToDouble(data.USCycle5),
                "MicrosCleanCAN" => Convert.ToDouble(data.MicrosCleanCAN),
                "MicrosSD" => Convert.ToDouble(data.MicrosSD),
                "Steer" => Convert.ToDouble(data.Steer),
                "SafetyScore" => Convert.ToDouble(data.SafetyScore),
                "MaintenanceScore" => Convert.ToDouble(data.MaintenanceScore),
                _ => 0.0
            };
        }

        public static decimal GetValue(this ICollection<DobackData> data, string property)
        {
            if (!data.Any()) return 0;

            return property switch
            {
                "StabilityIndex" => data.Average(d => d.StabilityIndex),
                "Speed" => data.Average(d => d.Speed),
                "Roll" => data.Average(d => d.Roll),
                "Pitch" => data.Average(d => d.Pitch),
                "Yaw" => data.Average(d => d.Yaw),
                _ => 0
            };
        }

        public static decimal GetStabilityIndex(this ICollection<DobackData> data) => GetValue(data, "StabilityIndex");
        public static decimal GetSpeed(this ICollection<DobackData> data) => GetValue(data, "Speed");
        public static decimal GetRoll(this ICollection<DobackData> data) => GetValue(data, "Roll");
        public static decimal GetPitch(this ICollection<DobackData> data) => GetValue(data, "Pitch");
        public static decimal GetYaw(this ICollection<DobackData> data) => GetValue(data, "Yaw");

        public static List<decimal> GetValues(this ICollection<DobackData> data, string property)
        {
            if (data == null) return new List<decimal>();
            
            return data.Select(d => GetValue(new[] { d }, property)).ToList();
        }

        public static List<double> GetPropertyValues(this ICollection<DobackData> data, string propertyName)
        {
            return data.Select(d => d.GetPropertyValue(propertyName)).ToList();
        }

        public static List<double> ToDoubleList(this IEnumerable<decimal> values)
        {
            return values.Select(v => Convert.ToDouble(v)).ToList();
        }

        public static List<double> GetDataSeries(this ICollection<DobackData> data, string property)
        {
            return data.Select(d => property switch
            {
                "AccelerationX" => Convert.ToDouble(d.AccelerationX),
                "AccelerationY" => Convert.ToDouble(d.AccelerationY),
                "AccelerationZ" => Convert.ToDouble(d.AccelerationZ),
                "Roll" => Convert.ToDouble(d.Roll),
                "Pitch" => Convert.ToDouble(d.Pitch),
                "Yaw" => Convert.ToDouble(d.Yaw),
                "Speed" => Convert.ToDouble(d.Speed),
                "StabilityIndex" => Convert.ToDouble(d.StabilityIndex),
                _ => 0.0
            }).ToList();
        }

        public static decimal ToDecimal(this double value) => Convert.ToDecimal(value);
        public static double ToDouble(this decimal value) => Convert.ToDouble(value);
    }
} 