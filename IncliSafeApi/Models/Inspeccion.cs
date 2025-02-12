
using System;
using System.ComponentModel.DataAnnotations;

namespace IncliSafe.Api.Models
{
    public class Inspeccion
    {
        public int Id { get; set; }
        
        [Required]
        public int VehiculoId { get; set; }
        public Vehiculo Vehiculo { get; set; }
        
        [Required]
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
        
        public DateTime FechaInspeccion { get; set; }
        public string Resultado { get; set; }
        public string Observaciones { get; set; }
    }
}
