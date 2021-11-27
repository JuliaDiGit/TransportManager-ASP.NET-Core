using AutoMapper;
using TransportManager.Domain;
using TransportManager.Entities;
using TransportManager.Models;

namespace TransportManager.Mappings
{
    public class CompanyProfile : Profile
    {
        public CompanyProfile()
        {
            CreateMap<Company, CompanyModel>();
            CreateMap<CompanyModel, Company>();
            CreateMap<Company, CompanyEntity>();
            CreateMap<CompanyEntity, Company>();
        }
    }
}