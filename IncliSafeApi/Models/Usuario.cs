
using System.ComponentModel.DataAnnotations;

namespace IncliSafe.Api.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        
        [Required]
        public string Nombre { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        public string Password { get; set; }
        
        public string Rol { get; set; }
        public bool Activo { get; set; }
    }
}
