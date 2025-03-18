using IncliSafe.Shared.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace IncliSafe.Shared.Models.Auth
{
    public class ChangePasswordRequest
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; } = string.Empty;

        [Required]
        public required string OldPassword { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public required string NewPassword { get; set; } = string.Empty;

        [Required]
        [Compare("NewPassword")]
        public required string ConfirmPassword { get; set; } = string.Empty;

        public required string CurrentPassword { get; set; } = string.Empty;
    }
} 

