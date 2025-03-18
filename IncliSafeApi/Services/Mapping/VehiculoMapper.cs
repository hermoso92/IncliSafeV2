using IncliSafe.Shared.Models.DTOs;
using IncliSafe.Shared.Models.Entities;
using AutoMapper;

namespace IncliSafeApi.Services.Mapping
{
    public class VehiculoMapper : Profile
    {
        public VehiculoMapper()
        {
            CreateMap<Vehiculo, VehiculoDTO>();
            CreateMap<VehiculoDTO, Vehiculo>();
        }
    }
} 