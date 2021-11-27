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
    public class DriversRepository : IDriversRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public DriversRepository(DataContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<DriverEntity> GetDriverAsync(int id)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));

            return await _context.Drivers.Where(driver => driver.Id == id && !driver.IsDeleted)
                                         .Include(driver => driver.Vehicles.Where(vehicle => !vehicle.IsDeleted))
                                         .AsNoTracking()
                                         .FirstOrDefaultAsync();
        }

        public async Task<DriverEntity> AddDriverAsync(Driver driver)
        {
            if (driver == null) throw new ArgumentNullException(nameof(driver));

            var companyEntity = await _context.Companies.Where(company => company.CompanyId == driver.CompanyId)
                                                        .AsNoTracking()
                                                        .FirstOrDefaultAsync();

            if (companyEntity == null) throw new UserErrorException(Resources.Error_ImpossibleAddDriver +
                                                                    Resources.Error_CompanyNotFound);

            var driverEntity = _mapper.Map<DriverEntity>(driver);

            await _context.Drivers.AddAsync(driverEntity);

            await _context.SaveChangesAsync();

            return driverEntity;
        }

        public async Task<DriverEntity> UpdateDriverAsync(Driver driver)
        {
            if (driver == null) throw new ArgumentNullException(nameof(driver));

            var foundDriver = await _context.Drivers.Where(d => d.Id == driver.Id && !d.IsDeleted)
                                                    .Include(d => d.Vehicles.Where(vehicle => !vehicle.IsDeleted))
                                                    .AsNoTracking()
                                                    .FirstOrDefaultAsync();

            if (foundDriver == null) throw new UserErrorException(Resources.Error_ImpossibleUpdateDriver +
                                                                  Resources.Error_DriverNotFound);

            var companyEntity = await _context.Companies.Where(company => company.CompanyId == driver.CompanyId)
                                                        .AsNoTracking()
                                                        .FirstOrDefaultAsync();

            if (companyEntity == null) throw new UserErrorException(Resources.Error_ImpossibleUpdateDriver +
                                                                    Resources.Error_CompanyNotFound);

            var driverEntity = _mapper.Map<DriverEntity>(driver);

            // оставлем изнчальную дату создания
            driverEntity.CreatedDate = foundDriver.CreatedDate;

            var updDriver = _context.Drivers.Update(driverEntity);

            await _context.SaveChangesAsync();

            return updDriver.Entity;
        }

        public async Task<DriverEntity> DeleteDriverAsync(int id)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));

            var driverEntity = await _context.Drivers.Where(driver => driver.Id == id)
                                                     .Include(driver => driver.Vehicles)
                                                     .AsNoTracking()
                                                     .FirstOrDefaultAsync();

            if (driverEntity == null) throw new UserErrorException(Resources.Error_ImpossibleDeleteDriver +
                                                                   Resources.Error_DriverNotFound);

            var vehicles = driverEntity.Vehicles.ToList();
            vehicles.ForEach(vehicle =>
            {
                vehicle.DriverId = null;
                _context.Vehicles.Update(vehicle);
            });

            _context.Drivers.Remove(driverEntity);

            await _context.SaveChangesAsync();

            return driverEntity;
        }

        public async Task<List<DriverEntity>> GetAllDriversAsync()
        {
            return await _context.Drivers.Where(driver => !driver.IsDeleted)
                                         .Include(driver => driver.Vehicles.Where(vehicle => !vehicle.IsDeleted))
                                         .AsNoTracking()
                                         .ToListAsync();
        }

        public async Task<DriverEntity> RemoveDriverAsync(int id)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));

            var driverEntity = await _context.Drivers.Where(driver => driver.Id == id && !driver.IsDeleted)
                                                     .Include(driver => driver.Vehicles.Where(vehicle => !vehicle.IsDeleted))
                                                     .AsNoTracking()
                                                     .FirstOrDefaultAsync();

            if (driverEntity == null) throw new UserErrorException(Resources.Error_ImpossibleRemoveDriver +
                                                                   Resources.Error_DriverNotFound);

            var vehicles = driverEntity.Vehicles.ToList();
            vehicles.ForEach(vehicle =>
            {
                vehicle.DriverId = null;
                _context.Vehicles.Update(vehicle);
            });

            driverEntity.IsDeleted = true;
            driverEntity.SoftDeletedDate = DateTime.Now;

            var updDriver = _context.Update(driverEntity);

            await _context.SaveChangesAsync();

            return updDriver.Entity;
        }
    }
}