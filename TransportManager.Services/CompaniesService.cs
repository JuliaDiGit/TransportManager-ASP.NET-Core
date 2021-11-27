using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TransportManager.Data.Repositories.Abstract;
using TransportManager.Domain;
using TransportManager.Models;
using TransportManager.Services.Abstract;

namespace TransportManager.Services
{
    public class CompaniesService : ICompaniesService
    {
        private readonly ICompaniesRepository _companiesRepository;
        private readonly IMapper _mapper;

        public CompaniesService(ICompaniesRepository companiesRepository, IMapper mapper)
        {
            _companiesRepository = companiesRepository ?? throw new ArgumentNullException(nameof(companiesRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Company> GetCompanyByCompanyIdAsync(int companyId, string userLogin)
        {
            if (companyId <= 0) throw new ArgumentOutOfRangeException(nameof(companyId));
            if (userLogin == null) throw new ArgumentNullException(nameof(userLogin));

            var companyEntity = await _companiesRepository.GetCompanyAsync(companyId);

            return _mapper.Map<Company>(companyEntity);
        }

        public async Task<Company> AddCompanyAsync(CompanyModel companyModel, string userLogin)
        {
            if (companyModel == null) throw new ArgumentNullException(nameof(companyModel));
            if (userLogin == null) throw new ArgumentNullException(nameof(userLogin));

            var company = _mapper.Map<Company>(companyModel);
            var companyEntity = await _companiesRepository.AddCompanyAsync(company);

            return _mapper.Map<Company>(companyEntity);

        }

        public async Task<Company> UpdateCompanyAsync(CompanyModel companyModel, string userLogin)
        {
            if (companyModel == null) throw new ArgumentNullException(nameof(companyModel));
            if (userLogin == null) throw new ArgumentNullException(nameof(userLogin));

            var company = _mapper.Map<Company>(companyModel);
            var companyEntity = await _companiesRepository.UpdateCompanyAsync(company);
            var updCompany = _mapper.Map<Company>(companyEntity);

            return _mapper.Map<Company>(updCompany);

        }

        public async Task<Company> DeleteCompanyByCompanyIdAsync(int companyId, string userLogin)
        {
            if (companyId <= 0) throw new ArgumentOutOfRangeException(nameof(companyId));
            if (userLogin == null) throw new ArgumentNullException(nameof(userLogin));

            var companyEntity = await _companiesRepository.DeleteCompanyAsync(companyId);

            return _mapper.Map<Company>(companyEntity);
        }

        public async Task<List<Company>> GetAllCompaniesAsync(string userLogin)
        {
            if (userLogin == null) throw new ArgumentNullException(nameof(userLogin));

            return (await _companiesRepository.GetAllCompaniesAsync())?
                                              .Select(companyEntity => _mapper.Map<Company>(companyEntity))
                                              .ToList();
        }

        public async Task<Company> RemoveCompanyByCompanyIdAsync(int companyId, string userLogin)
        {
            if (companyId <= 0) throw new ArgumentOutOfRangeException(nameof(companyId));
            if (userLogin == null) throw new ArgumentNullException(nameof(userLogin));

            var companyEntity = await _companiesRepository.RemoveCompanyAsync(companyId);

            return _mapper.Map<Company>(companyEntity);
        }
    }
}