using AutoMapper;
using Domain;
using Entities;
using Models;

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