using System;

namespace IncliSafe.Shared.Models.DTOs.Base
{
    public abstract class BaseDTO
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; } = true;
    }
} 