
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IncliSafe.Api.Data;
using IncliSafe.Api.Models;
using System.Threading.Tasks;

namespace IncliSafe.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InspeccionesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public InspeccionesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetInspecciones()
        {
            var inspecciones = await _context.Inspecciones
                .Include(i => i.Vehiculo)
                .Include(i => i.Usuario)
                .ToListAsync();
            return Ok(inspecciones);
        }

        [HttpPost]
        public async Task<IActionResult> CreateInspeccion(Inspeccion inspeccion)
        {
            _context.Inspecciones.Add(inspeccion);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetInspecciones), new { id = inspeccion.Id }, inspeccion);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInspeccion(int id, Inspeccion inspeccion)
        {
            if (id != inspeccion.Id)
                return BadRequest();

            _context.Entry(inspeccion).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInspeccion(int id)
        {
            var inspeccion = await _context.Inspecciones.FindAsync(id);
            if (inspeccion == null)
                return NotFound();

            _context.Inspecciones.Remove(inspeccion);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
