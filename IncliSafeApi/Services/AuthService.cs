using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using IncliSafeApi.Data;
using IncliSafeApi.Services.Interfaces;
using IncliSafe.Shared.Models.Auth;
using IncliSafe.Shared.Models.Entities;
using BC = BCrypt.Net.BCrypt;
using Microsoft.Extensions.Logging;

namespace IncliSafeApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthService> _logger;

        public AuthService(ApplicationDbContext context, IConfiguration configuration, ILogger<AuthService> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<AuthResponse> LoginAsync(string username, string password)
        {
            var user = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Username == username);

            if (user == null || !VerifyPassword(password, user.Password))
            {
                return new AuthResponse 
                { 
                    Success = false, 
                    Message = "Usuario o contraseña incorrectos" 
                };
            }

            var token = await GenerateJwtToken(user);
            var session = new UserSession
            {
                Id = user.Id,
                Username = user.Email,
                Name = user.Nombre,
                Role = user.Rol,
                Token = token,
                IsActive = user.Activo,
                LastLogin = DateTime.UtcNow
            };

            return new AuthResponse
            {
                Success = true,
                Message = "Login exitoso",
                Token = token,
                User = session
            };
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            if (await _context.Usuarios.AnyAsync(u => u.Email == request.Email))
            {
                return new AuthResponse 
                { 
                    Success = false, 
                    Message = "El usuario ya existe",
                    Token = string.Empty,
                    User = null
                };
            }

            var user = new Usuario
            {
                Email = request.Email,
                Nombre = request.Name ?? string.Empty,
                Password = HashPassword(request.Password),
                Rol = "Usuario",
                Activo = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Usuarios.Add(user);
            await _context.SaveChangesAsync();

            var token = await GenerateJwtToken(user);
            var session = new UserSession
            {
                Id = user.Id,
                Username = user.Email,
                Name = user.Nombre,
                Role = user.Rol,
                Token = token,
                IsActive = user.Activo,
                LastLogin = DateTime.UtcNow
            };

            return new AuthResponse
            {
                Success = true,
                Message = "Registro exitoso",
                Token = token,
                User = session
            };
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            if (string.IsNullOrEmpty(token)) 
                return false;

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = _configuration["JwtSettings:SecretKey"] ?? 
                    throw new InvalidOperationException("JWT SecretKey not configured");

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _configuration["JwtSettings:Issuer"],
                    ValidAudience = _configuration["JwtSettings:Audience"],
                    ClockSkew = TimeSpan.Zero
                };

                var principal = await Task.Run(() => 
                    tokenHandler.ValidateToken(token, validationParameters, out _));
                
                return principal != null;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            var user = await _context.Usuarios.FindAsync(userId);
            if (user == null || !VerifyPassword(currentPassword, user.Password))
            {
                return false;
            }

            user.Password = HashPassword(newPassword);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ResetPasswordAsync(string email)
        {
            var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return false;

            try
            {
                var resetToken = await GeneratePasswordResetTokenAsync(user);
                // Aquí iría la lógica para enviar el email
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al resetear contraseña para {Email}", email);
                return false;
            }
        }

        public async Task<UserSession?> GetUserSessionAsync(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = _configuration["JwtSettings:SecretKey"] ?? 
                    throw new InvalidOperationException("JWT SecretKey not configured");
                
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _configuration["JwtSettings:Issuer"],
                    ValidAudience = _configuration["JwtSettings:Audience"],
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
                var userId = int.Parse(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                
                var user = await _context.Usuarios.FindAsync(userId);
                if (user == null) return null;

                return new UserSession
                {
                    Id = user.Id,
                    Username = user.Email,
                    Name = user.Nombre,
                    Role = user.Rol,
                    Token = token,
                    IsActive = user.Activo,
                    LastLogin = user.LastLogin ?? DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al validar token");
                return null;
            }
        }

        public async Task<bool> RevokeTokenAsync(string token)
        {
            if (string.IsNullOrEmpty(token)) return false;

            try
            {
                // Agregar el token a la lista negra
                var blacklistedToken = new BlacklistedToken
                {
                    Token = token,
                    RevokedAt = DateTime.UtcNow
                };
                
                _context.BlacklistedTokens.Add(blacklistedToken);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al revocar token");
                return false;
            }
        }

        public async Task<Usuario?> GetUserByIdAsync(int id)
        {
            return await _context.Usuarios.FindAsync(id);
        }

        public async Task<string?> GenerateJwtToken(Usuario user)
        {
            if (user == null) return null;

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Rol)
            };

            var key = _configuration["JwtSettings:Key"] ?? 
                throw new InvalidOperationException("JWT Key not configured");
            var issuer = _configuration["JwtSettings:Issuer"] ?? 
                throw new InvalidOperationException("JWT Issuer not configured");
            var audience = _configuration["JwtSettings:Audience"] ?? 
                throw new InvalidOperationException("JWT Audience not configured");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }

        private async Task<string> GeneratePasswordResetTokenAsync(Usuario user)
        {
            var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            user.ResetToken = token;
            user.ResetTokenExpires = DateTime.UtcNow.AddHours(24);
            await _context.SaveChangesAsync();
            return token;
        }
    }
}
