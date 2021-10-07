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
                                         .FirstOrDefaultAsync();
        }

        public async Task<DriverEntity> AddDriverAsync(Driver driver)
        {
            if (driver == null) throw new ArgumentNullException(nameof(driver));

            var driverEntity = _mapper.Map<DriverEntity>(driver);

            await _context.Drivers.AddAsync(driverEntity);

            await _context.SaveChangesAsync();

            return driverEntity;
        }

        public async Task<DriverEntity> UpdateDriverAsync(Driver driver)
        {
            if (driver == null) throw new ArgumentNullException(nameof(driver));

            var driverEntity = _mapper.Map<DriverEntity>(driver);

            var updDriver = _context.Drivers.Update(driverEntity);

            await _context.SaveChangesAsync();

            return updDriver.Entity;
        }

        public async Task<DriverEntity> DeleteDriverAsync(int id)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));

            var driverEntity = await _context.Drivers.Where(driver => driver.Id == id)
                                                     .Include(driver => driver.Vehicles)
                                                     .FirstOrDefaultAsync();

            if (driverEntity == null) throw new NullReferenceException(nameof(DriverEntity));

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
                                         .ToListAsync();
        }

        public async Task<DriverEntity> RemoveDriverAsync(int id)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));

            var driverEntity = await _context.Drivers.Where(driver => driver.Id == id && !driver.IsDeleted)
                                                     .Include(driver => driver.Vehicles.Where(vehicle => !vehicle.IsDeleted))
                                                     .FirstOrDefaultAsync();

            if (driverEntity == null) throw new NullReferenceException(nameof(DriverEntity));

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