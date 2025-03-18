using IncliSafe.Shared.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace IncliSafe.Shared.Models.Auth
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "El nombre de usuario es requerido")]
        public required string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre es requerido")]
        public required string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "Formato de email inválido")]
        public required string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es requerida")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        public required string Password { get; set; } = string.Empty;

        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
        public required string ConfirmPassword { get; set; } = string.Empty;
    }
} 

