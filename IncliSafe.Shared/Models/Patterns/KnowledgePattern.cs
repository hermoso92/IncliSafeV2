public class KnowledgePattern
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Pattern { get; set; } = string.Empty;
    public string Actions { get; set; } = string.Empty;
    public List<string> RecommendedActions { get; set; } = new();
    public bool IsActive { get; set; }
    public decimal Confidence { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastModified { get; set; }
    
    // Relación con las detecciones
    public virtual ICollection<PatternDetection> PatternDetections { get; set; } = new List<PatternDetection>();
} 