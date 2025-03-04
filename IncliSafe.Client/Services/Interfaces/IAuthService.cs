using IncliSafe.Shared.Models;

namespace IncliSafe.Client.Services.Interfaces
{
    public interface IAuthService
    {
        Task<UserSession> Login(string username, string password);
        Task Logout();
        Task<UserSession?> GetCurrentUser();
        Task<bool> IsAuthenticated();
    }
} 