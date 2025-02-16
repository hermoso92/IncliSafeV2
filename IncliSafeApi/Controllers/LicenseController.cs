using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using IncliSafeApi.Services.Interfaces;
using IncliSafe.Shared.Models.Entities;

namespace IncliSafeApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class LicenseController : ControllerBase
    {
        private readonly ILicenseService _licenseService;

        public LicenseController(ILicenseService licenseService)
        {
            _licenseService = licenseService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<License>> GetLicense(int id)
        {
            try
            {
                var license = await _licenseService.GetLicenseAsync(id);
                if (license == null)
                    return NotFound();

                return Ok(license);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<License>> CreateLicense([FromBody] License license)
        {
            try
            {
                license.LicenseKey = Guid.NewGuid().ToString("N");
                license.CreatedAt = DateTime.UtcNow;
                license.IsActive = true;

                var createdLicense = await _licenseService.CreateLicenseAsync(license);
                return CreatedAtAction(nameof(GetLicense), new { id = createdLicense.Id }, createdLicense);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLicense(int id, [FromBody] License license)
        {
            try
            {
                if (id != license.Id)
                    return BadRequest();

                var result = await _licenseService.UpdateLicenseAsync(license);
                if (!result)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLicense(int id)
        {
            try
            {
                var result = await _licenseService.DeleteLicenseAsync(id);
                if (!result)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("vehicle/{vehicleId}")]
        public async Task<ActionResult<License>> GetVehicleLicense(int vehicleId)
        {
            try
            {
                var license = await _licenseService.GetVehicleLicenseAsync(vehicleId);
                if (license == null)
                    return NotFound();

                return Ok(license);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
} 