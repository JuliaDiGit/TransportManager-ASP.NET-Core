using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TransportManager.Domain;
using TransportManager.Entities;
using Microsoft.EntityFrameworkCore;
using TransportManager.Common.Exceptions;
using TransportManager.Data.Repositories.Abstract;

namespace TransportManager.Data.Repositories
{
    public class CompaniesRepository : ICompaniesRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public CompaniesRepository(DataContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<CompanyEntity> GetCompanyAsync(int companyId)
        {
            if (companyId <= 0) throw new ArgumentOutOfRangeException(nameof(companyId));

            return await _context.Companies.Where(company => company.CompanyId == companyId && !company.IsDeleted)
                                           .Include(company => company.Drivers.Where(driver => !driver.IsDeleted))
                                           .Include(company => company.Vehicles.Where(vehicle => !vehicle.IsDeleted))
                                           .AsNoTracking()
                                           .FirstOrDefaultAsync();
        }

        public async Task<CompanyEntity> AddCompanyAsync(Company company)
        {
            if (company == null) throw new ArgumentNullException(nameof(company));

            var foundEntity = await _context.Companies.Where(c => c.CompanyId == company.CompanyId)
                                                      .AsNoTracking()
                                                      .FirstOrDefaultAsync();

            if (foundEntity != null) throw new UserErrorException(Resources.Error_ImpossibleAddCompany +
                                                                  Resources.Error_CompanyIdExists);

            var companyEntity = _mapper.Map<CompanyEntity>(company);

            var addedCompany = await _context.Companies.AddAsync(companyEntity);

            await _context.SaveChangesAsync();

            return addedCompany.Entity;
        }

        public async Task<CompanyEntity> UpdateCompanyAsync(Company company)
        {
            if (company == null) throw new ArgumentNullException(nameof(company));

            var foundCompany = await _context.Companies.Where(c => c.CompanyId == company.CompanyId && !c.IsDeleted)
                                                       .Include(c => c.Drivers.Where(driver => !driver.IsDeleted))
                                                       .Include(c => c.Vehicles.Where(vehicle => !vehicle.IsDeleted))
                                                       .AsNoTracking()
                                                       .FirstOrDefaultAsync();

            if (foundCompany == null) throw new UserErrorException(Resources.Error_ImpossibleUpdateCompany +
                                                                   Resources.Error_CompanyNotFound);

            var companyEntity = _mapper.Map<CompanyEntity>(company);

            // оставляем уже имеющийся Id
            companyEntity.Id = foundCompany.Id;

            // оставлем изначальную дату создания
            companyEntity.CreatedDate = foundCompany.CreatedDate;

            var updCompanyEntity = _context.Companies.Update(companyEntity);

            await _context.SaveChangesAsync();

            return updCompanyEntity.Entity;
        }

        public async Task<CompanyEntity> DeleteCompanyAsync(int companyId)
        {
            if (companyId <= 0) throw new ArgumentOutOfRangeException(nameof(companyId));

            var companyEntity = await _context.Companies.Where(c => c.CompanyId == companyId)
                                                        .Include(c => c.Drivers)
                                                        .Include(c => c.Vehicles)
                                                        .AsNoTracking()
                                                        .FirstOrDefaultAsync();

            if (companyEntity == null) throw new UserErrorException(Resources.Error_ImpossibleDeleteCompany +
                                                                    Resources.Error_CompanyNotFound);

            _context.Companies.Remove(companyEntity);

            await _context.SaveChangesAsync();

            return companyEntity;
        }

        public async Task<List<CompanyEntity>> GetAllCompaniesAsync()
        {
            return await _context.Companies.Where(company => !company.IsDeleted)
                                           .Include(company => company.Drivers.Where(driver => !driver.IsDeleted))
                                           .Include(company => company.Vehicles.Where(vehicle => !vehicle.IsDeleted))
                                           .AsNoTracking()
                                           .ToListAsync();
        }

        public async Task<CompanyEntity> RemoveCompanyAsync(int companyId)
        {
            if (companyId <= 0) throw new ArgumentOutOfRangeException(nameof(companyId));

            var companyEntity = await _context.Companies.Where(company => company.CompanyId == companyId && !company.IsDeleted)
                                                        .Include(company => company.Drivers.Where(driver => !driver.IsDeleted))
                                                        .Include(company => company.Vehicles.Where(vehicle => !vehicle.IsDeleted))
                                                        .AsNoTracking()
                                                        .FirstOrDefaultAsync();

            if (companyEntity == null) throw new UserErrorException(Resources.Error_ImpossibleRemoveCompany +
                                                                    Resources.Error_CompanyNotFound);

            var drivers = companyEntity.Drivers.ToList();
            drivers.ForEach(driver =>
            {
                driver.IsDeleted = true;
                driver.SoftDeletedDate = DateTime.Now;
                _context.Drivers.Update(driver);
            });

            var vehicles = companyEntity.Vehicles.ToList();
            vehicles.ForEach(vehicle =>
            {
                vehicle.IsDeleted = true;
                vehicle.SoftDeletedDate = DateTime.Now;
                _context.Vehicles.Update(vehicle);
            });

            companyEntity.IsDeleted = true;
            companyEntity.SoftDeletedDate = DateTime.Now;

            var updCompany = _context.Companies.Update(companyEntity);

            await _context.SaveChangesAsync();

            return updCompany.Entity;
        }
    }
}