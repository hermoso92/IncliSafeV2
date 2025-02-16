using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IncliSafeApi.Data;
using IncliSafe.Shared.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace IncliSafeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InspeccionesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<InspeccionesController> _logger;
        private readonly JsonSerializerOptions _jsonOptions;

        public InspeccionesController(ApplicationDbContext context, ILogger<InspeccionesController> logger)
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
        public async Task<ActionResult<IEnumerable<Inspeccion>>> GetInspecciones()
        {
            try
            {
                var userId = User.FindFirst("UserId")?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                IQueryable<Inspeccion> query = _context.Inspecciones;
                query = query.Include(i => i.Vehiculo);

                if (!User.IsInRole("Administrador"))
                {
                    query = query.Where(i => i.Vehiculo != null && i.Vehiculo.UserId == int.Parse(userId));
                }

                var inspecciones = await query.ToListAsync();
                return Ok(inspecciones);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener inspecciones");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Inspeccion>> GetInspeccion(int id)
        {
            try
            {
                var inspeccion = await _context.Inspecciones
                    .Include(i => i.Vehiculo)
                    .FirstOrDefaultAsync(i => i.Id == id);

                if (inspeccion == null)
                {
                    return NotFound();
                }

                if (!User.IsInRole("Administrador"))
                {
                    var userId = User.FindFirst("UserId")?.Value;
                    if (string.IsNullOrEmpty(userId) || 
                        (inspeccion.Vehiculo?.UserId != int.Parse(userId) && 
                         inspeccion.InspectorId != int.Parse(userId)))
                    {
                        return Forbid();
                    }
                }

                return inspeccion;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener inspección {Id}", id);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrador,Inspector")]
        public async Task<ActionResult<Inspeccion>> PostInspeccion(Inspeccion inspeccion)
        {
            try
            {
                var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
                var userRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

                // Verificar que el vehículo existe
                var vehiculo = await _context.Vehiculos.FindAsync(inspeccion.VehiculoId);
                if (vehiculo == null)
                {
                    return BadRequest("Vehículo no encontrado");
                }

                // Si es inspector, asignar su ID
                if (userRole == "Inspector")
                {
                    inspeccion.InspectorId = userId;
                }

                _context.Inspecciones.Add(inspeccion);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetInspecciones), new { id = inspeccion.Id }, inspeccion);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear inspección");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador,Inspector")]
        public async Task<IActionResult> PutInspeccion(int id, Inspeccion inspeccion)
        {
            if (id != inspeccion.Id)
            {
                return BadRequest();
            }

            try
            {
                var existingInspeccion = await _context.Inspecciones.FindAsync(id);
                if (existingInspeccion == null)
                {
                    return NotFound();
                }

                // Verificar permisos
                var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
                var userRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

                if (userRole == "Inspector" && existingInspeccion.InspectorId != userId)
                {
                    return Forbid();
                }

                _context.Entry(inspeccion).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar inspección");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DeleteInspeccion(int id)
        {
            try
            {
                var inspeccion = await _context.Inspecciones.FindAsync(id);
                if (inspeccion == null)
                {
                    return NotFound();
                }

                _context.Inspecciones.Remove(inspeccion);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar inspección");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        private async Task<bool> InspeccionExists(int id)
        {
            return await _context.Inspecciones.AnyAsync(e => e.Id == id);
        }
    }
}
