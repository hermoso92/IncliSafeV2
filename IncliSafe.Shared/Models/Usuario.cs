using System;
using System.ComponentModel.DataAnnotations;

namespace IncliSafe.Shared.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;

        public string Rol { get; set; } = "Usuario";
        public bool Activo { get; set; } = true;
        public DateTime CreatedAt { get; set; }
    }
} 