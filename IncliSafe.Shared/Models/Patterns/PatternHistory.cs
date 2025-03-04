using System;
using IncliSafe.Shared.Models.Entities;

namespace IncliSafe.Shared.Models.Patterns
{
    public class PatternHistory
    {
        public int Id { get; set; }
        public int PatternId { get; set; }
        public DateTime DetectionTime { get; set; }
        public decimal Confidence { get; set; }
        public string Description { get; set; } = string.Empty;
        public int VehicleId { get; set; }
        
        public virtual KnowledgePattern Pattern { get; set; } = null!;
        public virtual Vehiculo Vehicle { get; set; } = null!;
    }
} 