
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IncliSafe.Api.Data;
using IncliSafe.Api.Models;
using System.Threading.Tasks;

namespace IncliSafe.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehiculosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public VehiculosController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetVehiculos()
        {
            var vehiculos = await _context.Vehiculos.ToListAsync();
            return Ok(vehiculos);
        }

        [HttpPost]
        public async Task<IActionResult> CreateVehiculo(Vehiculo vehiculo)
        {
            _context.Vehiculos.Add(vehiculo);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetVehiculos), new { id = vehiculo.Id }, vehiculo);
        }
    }
}
