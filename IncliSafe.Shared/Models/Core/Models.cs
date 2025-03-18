using IncliSafe.Shared.Models.Enums;
namespace IncliSafe.Shared.Models.Core
{
    // Modelos base para el Core del sistema
    public class BaseEntity
    {
        public required int Id { get; set; }
        public required DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class AuditableEntity : BaseEntity
    {
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
} 

