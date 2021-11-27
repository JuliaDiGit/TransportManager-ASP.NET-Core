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
    public class DriversService : IDriversService
    {
        private readonly IDriversRepository _driversRepository;
        private readonly IMapper _mapper;

        public DriversService(IDriversRepository driversRepository, IMapper mapper)
        {
            _driversRepository = driversRepository ?? throw new ArgumentNullException(nameof(driversRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Driver> GetDriverAsync(int id, string userLogin)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));
            if (userLogin == null) throw new ArgumentNullException(nameof(userLogin));

            var driverEntity = await _driversRepository.GetDriverAsync(id);

            return _mapper.Map<Driver>(driverEntity);
        }

        public async Task<Driver> AddDriverAsync(DriverModel driverModel, string userLogin)
        {
            if (driverModel == null) throw new ArgumentNullException(nameof(driverModel));
            if (userLogin == null) throw new ArgumentNullException(nameof(userLogin));

            var driver = _mapper.Map<Driver>(driverModel);
            var driverEntity = await _driversRepository.AddDriverAsync(driver);

            return _mapper.Map<Driver>(driverEntity);
        }

        public async Task<Driver> UpdateDriverAsync(DriverModel driverModel, string userLogin)
        {
            if (driverModel == null) throw new ArgumentNullException(nameof(driverModel));
            if (userLogin == null) throw new ArgumentNullException(nameof(userLogin));

            var driver = _mapper.Map<Driver>(driverModel);
            var driverEntity = await _driversRepository.UpdateDriverAsync(driver);

            return _mapper.Map<Driver>(driverEntity);
        }

        public async Task<Driver> DeleteDriverAsync(int id, string userLogin)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));
            if (userLogin == null) throw new ArgumentNullException(nameof(userLogin));

            var driver = await _driversRepository.DeleteDriverAsync(id);

            return _mapper.Map<Driver>(driver);
        }

        public async Task<List<Driver>> GetAllDriversAsync(string userLogin)
        {
            if (userLogin == null) throw new ArgumentNullException(nameof(userLogin));

            return (await _driversRepository.GetAllDriversAsync())?
                                            .Select(driverEntity => _mapper.Map<Driver>(driverEntity))
                                            .ToList();
        }

        public async Task<Driver> RemoveDriverAsync(int id, string userLogin)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));
            if (userLogin == null) throw new ArgumentNullException(nameof(userLogin));

            var driverEntity = await _driversRepository.RemoveDriverAsync(id);

            return _mapper.Map<Driver>(driverEntity);
        }
    }
}