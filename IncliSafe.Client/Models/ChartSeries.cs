namespace IncliSafe.Client.Models
{
    public class ChartSeries
    {
        public string Name { get; set; } = string.Empty;
        public double[] Data { get; set; } = Array.Empty<double>();
    }
} 