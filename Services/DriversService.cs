using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Data.Repositories.Abstract;
using Domain;
using Models;
using Services.Abstract;

namespace Services
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

        public async Task<Driver> GetDriverByIdAsync(int id, string userLogin)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));

            var driverEntity = await _driversRepository.GetDriverAsync(id);

            return _mapper.Map<Driver>(driverEntity);
        }

        public async Task<Driver> AddDriverAsync(DriverModel driverModel, string userLogin)
        {
            if (driverModel == null) throw new ArgumentNullException(nameof(driverModel));

            var driver = _mapper.Map<Driver>(driverModel);
            var driverEntity = await _driversRepository.AddDriverAsync(driver);

            return _mapper.Map<Driver>(driverEntity);
        }

        public async Task<Driver> UpdateDriverAsync(DriverModel driverModel, string userLogin)
        {
            if (driverModel == null) throw new ArgumentNullException(nameof(driverModel));

            var driver = _mapper.Map<Driver>(driverModel);
            var driverEntity = await _driversRepository.UpdateDriverAsync(driver);

            return _mapper.Map<Driver>(driverEntity);
        }

        public async Task<Driver> DeleteDriverByIdAsync(int id, string userLogin)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));

            var driver = await _driversRepository.DeleteDriverAsync(id);

            return _mapper.Map<Driver>(driver);
        }

        public async Task<List<Driver>> GetAllDriversAsync(string userLogin)
        {
            return (await _driversRepository.GetAllDriversAsync())?
                                            .Select(driverEntity => _mapper.Map<Driver>(driverEntity))
                                            .ToList();
        }

        public async Task<Driver> RemoveDriverByIdAsync(int id, string userLogin)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));

            var driverEntity = await _driversRepository.RemoveDriverAsync(id);

            return _mapper.Map<Driver>(driverEntity);
        }
    }
}