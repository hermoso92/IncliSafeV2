using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using IncliSafeApi.Data;
using IncliSafeApi.Services.Interfaces;
using IncliSafe.Shared.Models.Entities;

namespace IncliSafeApi.Services
{
    public class LicenseService : ILicenseService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<LicenseService> _logger;

        public LicenseService(ApplicationDbContext context, IConfiguration configuration, ILogger<LicenseService> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<(bool Success, string Message)> ActivateLicenseAsync(string licenseKey, string companyName, string email)
        {
            try
            {
                var existingLicense = await _context.Licenses
                    .FirstOrDefaultAsync(l => l.LicenseKey == licenseKey);

                if (existingLicense != null)
                {
                    return (false, "La licencia ya está registrada");
                }

                var license = new License
                {
                    LicenseKey = licenseKey,
                    CompanyName = companyName,
                    Email = email,
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddYears(1),
                    IsActive = true,
                    Type = LicenseType.Standard
                };

                _context.Licenses.Add(license);
                await _context.SaveChangesAsync();

                return (true, "Licencia activada correctamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error activando licencia");
                return (false, "Error al activar la licencia");
            }
        }

        public async Task<LicenseStatus> GetLicenseStatusAsync()
        {
            var license = await _context.Licenses
                .OrderByDescending(l => l.CreatedAt)
                .FirstOrDefaultAsync();

            if (license == null)
            {
                return new LicenseStatus
                {
                    IsActive = false,
                    Message = "No hay licencia activa"
                };
            }

            var isExpired = DateTime.UtcNow > license.ExpiresAt;

            return new LicenseStatus
            {
                IsActive = license.IsActive && !isExpired,
                Message = isExpired ? "La licencia ha expirado" : "Licencia activa",
                ExpirationDate = license.ExpiresAt
            };
        }

        private LicenseType? ValidateLicenseKey(string licenseKey)
        {
            var parts = licenseKey.Split('-');
            if (parts.Length != 5)
                return null;

            return parts[0].ToUpper() switch
            {
                "PRO" => LicenseType.Professional,
                "ENT" => LicenseType.Enterprise,
                _ => null
            };
        }

        public async Task<License?> GetLicenseAsync(int id)
        {
            return await _context.Licenses
                .Include(l => l.Vehiculo)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<License> CreateLicenseAsync(License license)
        {
            license.Type = LicenseType.Standard;
            license.CreatedAt = DateTime.UtcNow;
            license.ExpiryDate = DateTime.UtcNow.AddYears(1);
            license.IsActive = true;
            
            _context.Licenses.Add(license);
            await _context.SaveChangesAsync();
            return license;
        }

        public async Task<bool> UpdateLicenseAsync(License license)
        {
            try
            {
                _context.Entry(license).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await LicenseExistsAsync(license.Id))
                    return false;
                throw;
            }
        }

        public async Task<bool> DeleteLicenseAsync(int id)
        {
            var license = await _context.Licenses.FindAsync(id);
            if (license == null)
                return false;

            _context.Licenses.Remove(license);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<License?> GetVehicleLicenseAsync(int vehicleId)
        {
            return await _context.Licenses
                .Include(l => l.Vehiculo)
                .FirstOrDefaultAsync(l => l.VehiculoId == vehicleId && l.IsActive);
        }

        public async Task<List<License>> GetActiveLicensesAsync()
        {
            return await _context.Licenses
                .Include(l => l.Vehiculo)
                .Where(l => l.IsActive && l.ExpiryDate > DateTime.UtcNow)
                .ToListAsync();
        }

        public async Task<bool> ValidateLicenseAsync(string licenseNumber)
        {
            var license = await _context.Licenses
                .FirstOrDefaultAsync(l => l.LicenseNumber == licenseNumber);

            return license != null && 
                   license.IsActive && 
                   license.ExpiryDate > DateTime.UtcNow;
        }

        private async Task<bool> LicenseExistsAsync(int id)
        {
            return await _context.Licenses.AnyAsync(l => l.Id == id);
        }
    }
} 