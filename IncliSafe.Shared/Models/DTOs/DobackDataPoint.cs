namespace IncliSafe.Shared.Models.DTOs
{
    public class DobackDataPoint
    {
        public required DateTime Timestamp { get; set; }
        public required decimal Value { get; set; }
        public string? Unit { get; set; }
        public string? Label { get; set; }
        public Dictionary<string, object> Metadata { get; set; } = new();
    }
} 