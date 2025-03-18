using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IncliSafe.Shared.Models;

namespace IncliSafeApi.Controllers
{
    /// <summary>
    /// Interface for managing vehicle operations
    /// </summary>
    public interface IVehiculosController
    {
        /// <summary>
        /// Creates a new vehicle
        /// </summary>
        [HttpPost]
        Task<IActionResult> CreateVehiculo([FromBody] Vehiculo vehiculo);

        /// <summary>
        /// Gets all vehicles
        /// </summary>
        [HttpGet]
        Task<IActionResult> GetAll();

        /// <summary>
        /// Gets a vehicle by its ID
        /// </summary>
        [HttpGet("{id}")]
        Task<IActionResult> GetVehiculoById(int id);

        /// <summary>
        /// Updates an existing vehicle
        /// </summary>
        [HttpPut("{id}")]
        Task<IActionResult> UpdateVehiculo(int id, [FromBody] Vehiculo vehiculo);

        /// <summary>
        /// Deletes a vehicle by its ID
        /// </summary>
        [HttpDelete("{id}")]
        Task<IActionResult> DeleteVehiculo(int id);
    }
}
