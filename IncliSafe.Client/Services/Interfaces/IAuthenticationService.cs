using System.Threading.Tasks;
using IncliSafe.Shared.Models.Auth;
using IncliSafe.Shared.Models.Entities;

namespace IncliSafe.Client.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<bool> Login(LoginRequest request);
        Task<bool> Logout();
        Task<Usuario> GetCurrentUser();
        Task<bool> IsAuthenticated();
    }
} 