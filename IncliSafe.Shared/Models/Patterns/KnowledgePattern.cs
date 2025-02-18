namespace IncliSafe.Shared.Models.Patterns
{
    public class KnowledgePattern
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string PatternName { get; set; } = string.Empty;  // Para compatibilidad
        public string Description { get; set; } = string.Empty;
        public string Pattern { get; set; } = string.Empty;
        public string PatternType { get; set; } = string.Empty;  // Para compatibilidad
        public string DetectionCriteria { get; set; } = string.Empty;
        public List<string> RecommendedActions { get; set; } = new();
        public int TimesDetected { get; set; }
        public decimal Confidence { get; set; }
        public decimal Threshold { get; set; }
        public string Category { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastModified { get; set; }
        
        public virtual ICollection<DetectedPattern> DetectedPatterns { get; set; } = new List<DetectedPattern>();
    }
} 