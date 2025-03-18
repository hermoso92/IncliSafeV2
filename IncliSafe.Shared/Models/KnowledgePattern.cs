using IncliSafe.Shared.Models.Enums;
namespace IncliSafe.Shared.Models
{
    // Eliminar este archivo completamente ya que existe en Models/Knowledge/KnowledgePattern.cs
}

public class KnowledgeStats
{
    public required int TotalPatterns { get; set; }
    public required int TotalDetections { get; set; }
    public required decimal AverageConfidence { get; set; }
    public required DateTime LastUpdate { get; set; }
} 

