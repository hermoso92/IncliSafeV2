using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IncliSafeApi.Data;
using IncliSafe.Shared.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace IncliSafeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VehiculosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<VehiculosController> _logger;
        private readonly JsonSerializerOptions _jsonOptions;

        public VehiculosController(ApplicationDbContext context, ILogger<VehiculosController> logger)
        {
            _context = context;
            _logger = logger;
            _jsonOptions = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true
            };
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vehiculo>>> GetVehiculos()
        {
            try
            {
                var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
                _logger.LogInformation($"Obteniendo vehículos para usuario {userId}");

                var vehiculos = await _context.Vehiculos
                    .Where(v => v.UserId == userId)
                    .Include(v => v.Inspecciones)
                    .Include(v => v.Licenses)
                    .ToListAsync();
                
                return Ok(vehiculos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener vehículos");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Vehiculo>> GetVehiculo(int id)
        {
            try
            {
                var vehiculo = await _context.Vehiculos
                    .Include(v => v.Inspecciones)
                    .Include(v => v.Licenses)
                    .FirstOrDefaultAsync(v => v.Id == id);

                if (vehiculo == null)
                {
                    _logger.LogWarning("Vehículo no encontrado: {Id}", id);
                    return NotFound($"No se encontró el vehículo con ID: {id}");
                }

                return Ok(vehiculo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener vehículo {Id}", id);
                return StatusCode(500, "Error interno al obtener el vehículo");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Vehiculo>> PostVehiculo(Vehiculo vehiculo)
        {
            try
            {
                var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
                if (userId == 0)
                {
                    return BadRequest("Usuario no válido");
                }

                vehiculo.UserId = userId;
                
                // Validaciones adicionales
                if (string.IsNullOrWhiteSpace(vehiculo.Placa))
                    return BadRequest("La placa es requerida");
                    
                if (string.IsNullOrWhiteSpace(vehiculo.Marca))
                    return BadRequest("La marca es requerida");
                    
                if (string.IsNullOrWhiteSpace(vehiculo.Modelo))
                    return BadRequest("El modelo es requerido");
                    
                if (vehiculo.Año <= 1900 || vehiculo.Año > DateTime.Now.Year + 1)
                    return BadRequest("El año no es válido");
                    
                if (string.IsNullOrWhiteSpace(vehiculo.Color))
                    return BadRequest("El color es requerido");

                // Verificar si ya existe un vehículo con la misma placa
                if (await _context.Vehiculos.AnyAsync(v => v.Placa == vehiculo.Placa))
                {
                    return BadRequest("Ya existe un vehículo con esa placa");
                }

                _context.Vehiculos.Add(vehiculo);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetVehiculos), new { id = vehiculo.Id }, vehiculo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear vehículo");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutVehiculo(int id, Vehiculo vehiculo)
        {
            if (id != vehiculo.Id)
            {
                return BadRequest("ID no coincide");
            }

            try
            {
                var existingVehiculo = await _context.Vehiculos.FindAsync(id);
                if (existingVehiculo == null)
                {
                    return NotFound();
                }

                // Verificar que el usuario sea el propietario
                var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
                if (existingVehiculo.UserId != userId)
                {
                    return Forbid();
                }

                // Actualizar solo los campos permitidos
                existingVehiculo.Placa = vehiculo.Placa;
                existingVehiculo.Marca = vehiculo.Marca;
                existingVehiculo.Modelo = vehiculo.Modelo;
                existingVehiculo.Año = vehiculo.Año;
                existingVehiculo.Color = vehiculo.Color;

                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar vehículo");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DeleteVehiculo(int id)
        {
            try
            {
                var vehiculo = await _context.Vehiculos
                    .Include(v => v.Inspecciones)
                    .Include(v => v.Licenses)
                    .FirstOrDefaultAsync(v => v.Id == id);

                if (vehiculo == null)
                {
                    return NotFound();
                }

                // Eliminar inspecciones relacionadas
                _context.Inspecciones.RemoveRange(vehiculo.Inspecciones);
                
                // Actualizar licencias relacionadas
                foreach (var license in vehiculo.Licenses)
                {
                    license.VehiculoId = 0;
                    license.Vehiculo = null;
                }

                _context.Vehiculos.Remove(vehiculo);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar vehículo");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("{id}/inspecciones")]
        public async Task<ActionResult<IEnumerable<Inspeccion>>> GetInspecciones(int id)
        {
            var vehiculo = await _context.Vehiculos
                .Include(v => v.Inspecciones)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (vehiculo == null)
                return NotFound();

            return Ok(vehiculo.Inspecciones);
        }

        private async Task<bool> VehiculoExists(int id)
        {
            return await _context.Vehiculos.AnyAsync(e => e.Id == id);
        }
    }
}
