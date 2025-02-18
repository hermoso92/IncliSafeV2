public class PatternDistribution
{
    public string Name { get; set; } = string.Empty;
    public decimal Confidence { get; set; }
    public string Type { get; set; } = string.Empty;
    public int Count { get; set; }
    public Dictionary<string, decimal> Distribution { get; set; } = new();
} 