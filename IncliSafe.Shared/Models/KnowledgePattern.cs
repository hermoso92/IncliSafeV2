namespace IncliSafe.Shared.Models
{
    // Eliminar este archivo completamente ya que existe en Models/Knowledge/KnowledgePattern.cs
}

public class KnowledgeStats
{
    public int TotalPatterns { get; set; }
    public int TotalDetections { get; set; }
    public decimal AverageConfidence { get; set; }
    public DateTime LastUpdate { get; set; }
} 