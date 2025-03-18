using IncliSafe.Shared.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace IncliSafe.Shared.Models.Auth
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "Formato de email inválido")]
        public required string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es requerida")]
        public required string Password { get; set; } = string.Empty;

        public required bool RememberMe { get; set; }

        public required string Nombre { get; set; } = string.Empty;
    }
} 

