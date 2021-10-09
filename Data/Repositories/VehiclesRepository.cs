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
    public class VehiclesRepository : IVehiclesRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public VehiclesRepository(DataContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<VehicleEntity> GetVehicleAsync(int id)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));

            return await _context.Vehicles.Where(vehicle => vehicle.Id == id && !vehicle.IsDeleted)
                                          .FirstOrDefaultAsync();
        }

        public async Task<VehicleEntity> AddVehicleAsync(Vehicle vehicle)
        {
            if (vehicle == null) throw new ArgumentNullException(nameof(vehicle));

            var vehicleEntity = _mapper.Map<VehicleEntity>(vehicle);

            // если DriverId задано, то CompanyId берём от водителя
            if (vehicleEntity.DriverId != null)
            {
                var driverEntity = await _context.Drivers.Where(driver => driver.Id == vehicleEntity.DriverId && 
                                                                          !driver.IsDeleted)
                                                         .FirstOrDefaultAsync();

                if (driverEntity == null) throw new Exception($"Водитель Id {vehicleEntity.DriverId} не найден!");

                vehicleEntity.CompanyId = driverEntity.CompanyId;
            }

            var addedVehicle = await _context.Vehicles.AddAsync(vehicleEntity);

            await _context.SaveChangesAsync();

            return addedVehicle.Entity;
        }

        public async Task<VehicleEntity> UpdateVehicleAsync(Vehicle vehicle)
        {
            if (vehicle == null) throw new ArgumentNullException(nameof(vehicle));

            var vehicleEntity = _mapper.Map<VehicleEntity>(vehicle);

            // если DriverId задано, то CompanyId берём от водителя
            if (vehicleEntity.DriverId != null)
            {
                var driverEntity = await _context.Drivers.Where(driver => driver.Id == vehicleEntity.DriverId && 
                                                                          !driver.IsDeleted)
                                                         .FirstOrDefaultAsync();

                if (driverEntity == null) throw new Exception($"Водитель Id {vehicleEntity.DriverId} не найден!");

                vehicleEntity.CompanyId = driverEntity.CompanyId;
            }

            var updVehicle = _context.Vehicles.Update(vehicleEntity);

            await _context.SaveChangesAsync();

            return updVehicle.Entity;
        }

        public async Task<VehicleEntity> DeleteVehicleAsync(int id)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));

            var vehicleEntity = await _context.Vehicles.Where(vehicle => vehicle.Id == id)
                                                       .FirstOrDefaultAsync();

            if (vehicleEntity == null) throw new NullReferenceException(nameof(VehicleEntity));

            _context.Vehicles.Remove(vehicleEntity);

            await _context.SaveChangesAsync();

            return vehicleEntity;
        }

        public async Task<List<VehicleEntity>> GetAllVehiclesAsync()
        {
            return await _context.Vehicles.Where(vehicle => !vehicle.IsDeleted)
                                          .ToListAsync();
        }

        public async Task<VehicleEntity> RemoveVehicleAsync(int id)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));

            var vehicleEntity = await _context.Vehicles.Where(vehicle => vehicle.Id == id)
                                                       .FirstOrDefaultAsync();

            if (vehicleEntity == null) throw new NullReferenceException(nameof(VehicleEntity));

            vehicleEntity.IsDeleted = true;
            vehicleEntity.SoftDeletedDate = DateTime.Now;

            var updVehicle = _context.Vehicles.Update(vehicleEntity);

            await _context.SaveChangesAsync();

            return updVehicle.Entity;
        }
    }
}