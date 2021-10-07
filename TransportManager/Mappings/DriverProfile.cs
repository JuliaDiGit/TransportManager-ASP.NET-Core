using AutoMapper;
using Domain;
using Entities;
using Models;

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
