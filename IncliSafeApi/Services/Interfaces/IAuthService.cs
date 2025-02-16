using System.Threading.Tasks;
using IncliSafe.Shared.Models.Auth;
using IncliSafe.Shared.Models.Entities;

namespace IncliSafeApi.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> LoginAsync(string username, string password);
        Task<AuthResponse> RegisterAsync(RegisterRequest request);
        Task<bool> ValidateTokenAsync(string token);
        Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
        Task<bool> ResetPasswordAsync(string email);
        Task<UserSession?> GetUserSessionAsync(string token);
        Task<bool> RevokeTokenAsync(string token);
        Task<Usuario?> GetUserByIdAsync(int id);
    }
}