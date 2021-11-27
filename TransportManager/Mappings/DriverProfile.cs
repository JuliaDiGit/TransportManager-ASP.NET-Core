using AutoMapper;
using TransportManager.Domain;
using TransportManager.Entities;
using TransportManager.Models;

namespace TransportManager.Mappings
{
    public class DriverProfile : Profile
    {
        public DriverProfile()
        {
            CreateMap<Driver, DriverModel>();
            CreateMap<DriverModel, Driver>();
            CreateMap<Driver, DriverEntity>();
            CreateMap<DriverEntity, Driver>();

        }
    }
}
