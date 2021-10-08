using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Data.Repositories.Abstract;
using Domain;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
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
                                           .FirstOrDefaultAsync();
        }

        public async Task<CompanyEntity> AddCompanyAsync(Company company)
        {
            if (company == null) throw new ArgumentNullException(nameof(company));

            //var entity = await _context.Companies.FindAsync(company.CompanyId);

            //if (entity != null && entity.IsDeleted) return entity;

            var companyEntity = _mapper.Map<CompanyEntity>(company);

            var addedCompany = await _context.Companies.AddAsync(companyEntity);

            await _context.SaveChangesAsync();

            return addedCompany.Entity;
        }

        public async Task<CompanyEntity> UpdateCompanyAsync(Company company)
        {
            if (company == null) throw new ArgumentNullException(nameof(company));

            var newCompanyEntity = _mapper.Map<CompanyEntity>(company);

            var updCompanyEntity = _context.Companies.Update(newCompanyEntity);

            await _context.SaveChangesAsync();

            return updCompanyEntity.Entity;
        }

        public async Task<CompanyEntity> DeleteCompanyAsync(int companyId)
        {
            if (companyId <= 0) throw new ArgumentOutOfRangeException(nameof(companyId));

            var company = await _context.Companies.Where(c => c.CompanyId == companyId)
                                                  .Include(c => c.Drivers)
                                                  .Include(c => c.Vehicles)
                                                  .FirstOrDefaultAsync();

            if (company == null) return null;

            _context.Companies.Remove(company);

            await _context.SaveChangesAsync();

            return company;
        }

        public async Task<List<CompanyEntity>> GetAllCompaniesAsync()
        {
            return await _context.Companies.Where(company => !company.IsDeleted)
                                           .Include(company => company.Drivers.Where(driver => !driver.IsDeleted))
                                           .Include(company => company.Vehicles.Where(vehicle => !vehicle.IsDeleted))
                                           .ToListAsync();


        }

        public async Task<CompanyEntity> RemoveCompanyAsync(int companyId)
        {
            if (companyId <= 0) throw new ArgumentOutOfRangeException(nameof(companyId));

            var companyEntity = await _context.Companies.Where(company => company.CompanyId == companyId && !company.IsDeleted)
                                                        .Include(company => company.Drivers.Where(driver => !driver.IsDeleted))
                                                        .Include(company => company.Vehicles.Where(vehicle => !vehicle.IsDeleted))
                                                        .FirstOrDefaultAsync();

            if (companyEntity == null) return null;

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