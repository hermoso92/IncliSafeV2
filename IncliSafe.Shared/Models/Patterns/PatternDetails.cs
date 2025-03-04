using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Notifications;

namespace IncliSafe.Shared.Models.Patterns
{
    public class PatternDetails
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Confidence { get; set; }
        public List<double> DetectedValues { get; set; } = new();
        public List<string> RecommendedActions { get; set; } = new();
        public DateTime FirstDetected { get; set; }
        public DateTime LastDetected { get; set; }
        public int TimesDetected { get; set; }
    }
} 