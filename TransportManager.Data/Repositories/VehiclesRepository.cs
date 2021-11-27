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
                                          .AsNoTracking()
                                          .FirstOrDefaultAsync();
        }

        public async Task<VehicleEntity> AddVehicleAsync(Vehicle vehicle)
        {
            if (vehicle == null) throw new ArgumentNullException(nameof(vehicle));

            var foundVehicleByGovNumber = await _context.Vehicles.Where(v => v.GovernmentNumber == vehicle.GovernmentNumber)
                                                                 .AsNoTracking()
                                                                 .FirstOrDefaultAsync();

            if (foundVehicleByGovNumber != null) 
                throw new UserErrorException(Resources.Error_ImpossibleAddVehicle + 
                                             Resources.Error_GovNumberExists);

            if (vehicle.DriverId == 0) vehicle.DriverId = null;

            // если нет Водителя, то проверяем существует ли Компания
            if (vehicle.DriverId == null)
            {
                var companyEntity = await _context.Companies.Where(company => company.CompanyId == vehicle.CompanyId 
                                                                              && !company.IsDeleted)
                                                            .AsNoTracking()
                                                            .FirstOrDefaultAsync();

                if (companyEntity == null) throw new UserErrorException(Resources.Error_ImpossibleAddVehicle +
                                                                        Resources.Error_CompanyNotFound);
            }

            // если Водитель есть, то CompanyId берём от Водителя
            if (vehicle.DriverId != null)
            {
                var driverEntity = await _context.Drivers.Where(driver => driver.Id == vehicle.DriverId 
                                                                          && !driver.IsDeleted)
                                                         .AsNoTracking()
                                                         .FirstOrDefaultAsync();

                if (driverEntity == null) throw new UserErrorException(Resources.Error_ImpossibleAddVehicle +
                                                                       Resources.Error_DriverNotFound);

                // присваиваем ТС тот же CompanyId, что и у Водителя
                vehicle.CompanyId = driverEntity.CompanyId;
            }

            var vehicleEntity = _mapper.Map<VehicleEntity>(vehicle);

            var addedVehicle = await _context.Vehicles.AddAsync(vehicleEntity);

            await _context.SaveChangesAsync();

            return addedVehicle.Entity;
        }

        public async Task<VehicleEntity> UpdateVehicleAsync(Vehicle vehicle)
        {
            if (vehicle == null) throw new ArgumentNullException(nameof(vehicle));

            // проверяем существует ли ТС, которое нужно обновить
            var foundVehicle = await _context.Vehicles.Where(v => v.Id == vehicle.Id)
                                                      .AsNoTracking()
                                                      .FirstOrDefaultAsync();

            if (foundVehicle == null) throw new UserErrorException(Resources.Error_ImpossibleUpdateVehicle + 
                                                                   Resources.Error_VehicleNotFound);

            // проверяем, что устанавливаемый гос.номер ТС не используется на другом ТС
            var foundVehicleByGovNumber = await _context.Vehicles.Where(v => v.GovernmentNumber == vehicle.GovernmentNumber)
                                                                 .AsNoTracking()
                                                                 .FirstOrDefaultAsync();

            if (foundVehicleByGovNumber != null && foundVehicleByGovNumber.Id != vehicle.Id)
                throw new UserErrorException(Resources.Error_ImpossibleUpdateVehicle +
                                             Resources.Error_GovNumberExists);

            // если нет Водителя, то проверяем существует ли Компания
            if (vehicle.DriverId == null)
            {
                var companyEntity = await _context.Companies.Where(company => company.CompanyId == vehicle.CompanyId)
                                                            .AsNoTracking()
                                                            .FirstOrDefaultAsync();

                if (companyEntity == null) throw new UserErrorException(Resources.Error_ImpossibleUpdateVehicle +
                                                                        Resources.Error_CompanyNotFound);
            }

            // если Водитель есть, то CompanyId берём от Водителя
            if (vehicle.DriverId != null)
            {
                var driverEntity = await _context.Drivers.Where(driver => driver.Id == vehicle.DriverId
                                                                          && !driver.IsDeleted)
                                                         .AsNoTracking()
                                                         .FirstOrDefaultAsync();

                if (driverEntity == null) throw new UserErrorException(Resources.Error_ImpossibleUpdateVehicle +
                                                                       Resources.Error_DriverNotFound);

                // присваиваем ТС тот же CompanyId, что и у Водителя
                vehicle.CompanyId = driverEntity.CompanyId;
            }

            var vehicleEntity = _mapper.Map<VehicleEntity>(vehicle);

            // оставлем изнчальную дату создания
            vehicleEntity.CreatedDate = foundVehicle.CreatedDate;

            var updVehicle = _context.Vehicles.Update(vehicleEntity);

            await _context.SaveChangesAsync();

            return updVehicle.Entity;
        }

        public async Task<VehicleEntity> DeleteVehicleAsync(int id)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));

            var vehicleEntity = await _context.Vehicles.Where(vehicle => vehicle.Id == id)
                                                       .AsNoTracking()
                                                       .FirstOrDefaultAsync();

            if (vehicleEntity == null) throw new UserErrorException(Resources.Error_ImpossibleDeleteVehicle +
                                                                    Resources.Error_VehicleNotFound);

            _context.Vehicles.Remove(vehicleEntity);

            await _context.SaveChangesAsync();

            return vehicleEntity;
        }

        public async Task<List<VehicleEntity>> GetAllVehiclesAsync()
        {
            return await _context.Vehicles.Where(vehicle => !vehicle.IsDeleted)
                                          .AsNoTracking()
                                          .ToListAsync();
        }

        public async Task<VehicleEntity> RemoveVehicleAsync(int id)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));

            var vehicleEntity = await _context.Vehicles.Where(vehicle => vehicle.Id == id)
                                                       .AsNoTracking()
                                                       .FirstOrDefaultAsync();

            if (vehicleEntity == null) throw new UserErrorException(Resources.Error_ImpossibleRemoveVehicle +
                                                                    Resources.Error_VehicleNotFound);

            vehicleEntity.IsDeleted = true;
            vehicleEntity.SoftDeletedDate = DateTime.Now;

            var updVehicle = _context.Vehicles.Update(vehicleEntity);

            await _context.SaveChangesAsync();

            return updVehicle.Entity;
        }
    }
}