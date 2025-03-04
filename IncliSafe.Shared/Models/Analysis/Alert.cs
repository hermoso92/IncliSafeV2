public class Alert
{
    public int Id { get; set; }
    public int VehicleId { get; set; }
    public DateTime Timestamp { get; set; }
    public string Message { get; set; } = string.Empty;
    public string Severity { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public string Category { get; set; } = string.Empty;
    public virtual Vehiculo Vehicle { get; set; } = null!;
} 