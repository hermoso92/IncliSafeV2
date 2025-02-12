
using System.ComponentModel.DataAnnotations;

namespace IncliSafe.Api.Models
{
    public class Vehiculo
    {
        public int Id { get; set; }
        
        [Required]
        public string Placa { get; set; }
        
        [Required]
        public string Marca { get; set; }
        
        [Required]
        public string Modelo { get; set; }
        
        public int Año { get; set; }
        
        public string Estado { get; set; }
        
        public bool Activo { get; set; }
    }
}
