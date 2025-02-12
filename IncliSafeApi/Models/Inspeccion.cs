
using System;
using System.ComponentModel.DataAnnotations;

namespace IncliSafe.Api.Models
{
    public class Inspeccion
    {
        public int Id { get; set; }
        
        [Required]
        public DateTime Fecha { get; set; }
        
        public string Estado { get; set; }
        
        public string Observaciones { get; set; }
        
        public int VehiculoId { get; set; }
        public Vehiculo Vehiculo { get; set; }
        
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
    }
}
