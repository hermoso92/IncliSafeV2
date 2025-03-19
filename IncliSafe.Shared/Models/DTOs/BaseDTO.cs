using System;

namespace IncliSafe.Shared.Models.DTOs;

public abstract class BaseDTO
{
    public virtual int Id { get; set; }
    public virtual DateTime CreatedAt { get; set; }
} 