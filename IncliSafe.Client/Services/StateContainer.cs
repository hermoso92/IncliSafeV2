using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IncliSafe.Shared.Models;
using IncliSafe.Client.Services.Interfaces;

namespace IncliSafe.Client.Services
{
    public class StateContainer
    {
        private bool _isInitialized;
        private List<Vehiculo>? _vehiculos;
        private List<Inspeccion>? _inspecciones;
        
        private readonly IVehicleService _vehicleService;
        private readonly IInspectionService _inspeccionService;
        
        public event Action? OnChange;
        
        public List<Vehiculo> Vehiculos => _vehiculos ?? new();
        public List<Inspeccion> Inspecciones => _inspecciones ?? new();
        
        public StateContainer(
            IVehicleService vehicleService,
            IInspectionService inspeccionService)
        {
            _vehicleService = vehicleService;
            _inspeccionService = inspeccionService;
        }
        
        public async Task InitializeAsync()
        {
            if (!_isInitialized)
            {
                _vehiculos = await _vehicleService.GetVehiculos();
                _inspecciones = await _inspeccionService.GetInspeccionesAsync();
                _isInitialized = true;
                NotifyStateChanged();
            }
        }
        
        public void UpdateVehiculos(List<Vehiculo> vehiculos)
        {
            _vehiculos = vehiculos;
            NotifyStateChanged();
        }
        
        public void UpdateInspecciones(List<Inspeccion> inspecciones)
        {
            _inspecciones = inspecciones;
            NotifyStateChanged();
        }
        
        private void NotifyStateChanged() => OnChange?.Invoke();

        public async Task<List<Vehiculo>> GetVehiculos()
        {
            return await _vehicleService.GetVehiculos();
        }
    }
} 