using IncliSafe.Shared.Models.Enums;
using System;

namespace IncliSafe.Shared.Models.Common
{
    public abstract class BaseEntity
    {
        public required int Id { get; set; }
        public required DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public required bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }
    }
} 





