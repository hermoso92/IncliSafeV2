using System.ComponentModel.DataAnnotations;

namespace IncliSafe.Api.Models
{
    public class Vehiculo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La placa es requerida")]
        [StringLength(10)]
        public string Placa { get; set; }

        [Required(ErrorMessage = "La marca es requerida")]
        public string Marca { get; set; }

        [Required(ErrorMessage = "El modelo es requerido")]
        [StringLength(50)]
        public string Modelo { get; set; }

        [Required]
        [Range(1900, 2100)]
        public int Año { get; set; }
        public string Estado { get; set; }
        public bool Activo { get; set; }
    }
}