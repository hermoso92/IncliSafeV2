using IncliSafe.Shared.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace IncliSafe.Shared.Models.Auth
{
    public class ResetPasswordRequest
    {
        [Required]
        public required string Token { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public required string NewPassword { get; set; } = string.Empty;

        [Required]
        [Compare("NewPassword")]
        public required string ConfirmPassword { get; set; } = string.Empty;
    }
} 

