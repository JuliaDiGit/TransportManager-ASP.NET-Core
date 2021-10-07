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
    public class VehiclesService : IVehiclesService
    {
        private readonly IVehiclesRepository _vehiclesRepository;
        private readonly IMapper _mapper;

        public VehiclesService(IVehiclesRepository vehiclesRepository, IMapper mapper)
        {
            _vehiclesRepository = vehiclesRepository ?? throw new ArgumentNullException(nameof(vehiclesRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Vehicle> GetVehicleByIdAsync(int id, string userLogin)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));

            var vehicleEntity = await _vehiclesRepository.GetVehicleAsync(id);

            return _mapper.Map<Vehicle>(vehicleEntity);
        }

        public async Task<Vehicle> AddVehicleAsync(VehicleModel vehicleModel, string userLogin)
        {
            if (vehicleModel == null) throw new ArgumentNullException(nameof(vehicleModel));
            
            var vehicle = _mapper.Map<Vehicle>(vehicleModel);
            var vehicleEntity = await _vehiclesRepository.AddVehicleAsync(vehicle);

            return _mapper.Map<Vehicle>(vehicleEntity);
        }

        public async Task<Vehicle> UpdateVehicleAsync(VehicleModel vehicleModel, string userLogin)
        {
            if (vehicleModel == null) throw new ArgumentNullException(nameof(vehicleModel));

            var vehicle = _mapper.Map<Vehicle>(vehicleModel);
            var vehicleEntity = await _vehiclesRepository.UpdateVehicleAsync(vehicle);

            return _mapper.Map<Vehicle>(vehicleEntity);
        }

        public async Task<Vehicle> DeleteVehicleByIdAsync(int id, string userLogin)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));

            var vehicleEntity = await _vehiclesRepository.DeleteVehicleAsync(id);

            return _mapper.Map<Vehicle>(vehicleEntity);
        }

        public async Task<List<Vehicle>> GetAllVehiclesAsync(string userLogin)
        { 
            return (await _vehiclesRepository.GetAllVehiclesAsync())
                                             .Select(entity => _mapper.Map<Vehicle>(entity))
                                             .ToList();
        }

        public async Task<Vehicle> RemoveVehicleByIdAsync(int id, string userLogin)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));

            var vehicleEntity = await _vehiclesRepository.RemoveVehicleAsync(id);

            return _mapper.Map<Vehicle>(vehicleEntity);
        }
    }
}