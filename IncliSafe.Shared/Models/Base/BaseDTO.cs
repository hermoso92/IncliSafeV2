namespace IncliSafe.Shared.Models.Base;

public abstract class BaseDTO
{
    public int Id { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required DateTime UpdatedAt { get; set; }
} 