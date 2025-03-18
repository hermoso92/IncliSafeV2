using IncliSafe.Shared.Models.Enums;
using IncliSafe.Shared.Models.Core;

namespace IncliSafe.Shared.Models.Entities
{
    public class Usuario : BaseEntity
    {
        public required string Nombre { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public required string Role { get; set; }
        public required bool IsActive { get; set; }
    }

    public class Vehiculo : BaseEntity
    {
        public required string Nombre { get; set; }
        public required string Placa { get; set; }
        public required string Marca { get; set; }
        public required string Modelo { get; set; }
        public required string Color { get; set; }
        public int Año { get; set; }
        public required string Tipo { get; set; }
        public required string Estado { get; set; }
        public required int OwnerId { get; set; }
        public required bool Activo { get; set; } = true;
        public DateTime? UltimaInspeccion { get; set; }
        public required string UserId { get; set; } = string.Empty;
        public required DateTime UltimoMantenimiento { get; set; }
        public List<Mantenimiento> Mantenimientos { get; set; } = new();
    }

    public class Mantenimiento : BaseEntity
    {
        public required int VehiculoId { get; set; }
        public required Vehiculo Vehiculo { get; set; }
        public required DateTime Fecha { get; set; }
        public required string Descripcion { get; set; }
        public required MaintenanceType Tipo { get; set; }
        public required string Responsable { get; set; }
        public required decimal Costo { get; set; }
    }

    public enum MaintenanceType
    {
        Preventivo,
        Correctivo,
        Predictivo,
        Rutinario
    }
} 

