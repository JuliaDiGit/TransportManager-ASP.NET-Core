using AutoMapper;
using Domain;
using Entities;
using Models;

namespace TransportManager.Mappings
{
    public class VehicleProfile : Profile
    {
        public VehicleProfile()
        {
            CreateMap<Vehicle, VehicleModel>();
            CreateMap<VehicleModel, Vehicle>();
            CreateMap<Vehicle, VehicleEntity>();
            CreateMap<VehicleEntity, Vehicle>();
        }
    }
}
