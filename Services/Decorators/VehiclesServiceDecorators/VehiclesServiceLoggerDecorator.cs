using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Microsoft.Extensions.Logging;
using Models;
using Services.Abstract;

namespace Services.Decorators.VehiclesServiceDecorators
{
    public class VehiclesServiceLoggerDecorator : IVehiclesService
    {
        private readonly ILogger<VehiclesServiceLoggerDecorator> _logger;
        private readonly IVehiclesService _inner;
        private const string Id = "Id";

        public VehiclesServiceLoggerDecorator(ILogger<VehiclesServiceLoggerDecorator> logger, IVehiclesService inner)
        {
            _logger = logger;
            _inner = inner;
        }

        public async Task<Vehicle> AddVehicleAsync(VehicleModel vehicleModel, string userLogin)
        {
            try
            {
                Vehicle vehicle = await _inner.AddVehicleAsync(vehicleModel, userLogin);

                string status, addedVehicleId;

                if (vehicle == null)
                {
                    status = Resources.Status_Fail;
                    addedVehicleId = "";
                }
                else
                {
                    status = Resources.Status_Success;
                    addedVehicleId = $"{Id} {vehicle.Id}";
                }

                _logger.LogInformation($"{userLogin} - " +
                                       $"{Resources.Operation_AddVehicle} " +
                                       $"({addedVehicleId}) " +
                                       $"- {status}");

                return vehicle;
            }
            catch (Exception e)
            {
                _logger.LogError($"{userLogin} - " +
                                 $"{Resources.Operation_AddVehicle} - " +
                                 $"{e.GetType()}");

                throw;
            }
        }

        public async Task<Vehicle> UpdateVehicleAsync(VehicleModel vehicleModel, string userLogin)
        {
            try
            {
                Vehicle vehicle = await _inner.UpdateVehicleAsync(vehicleModel, userLogin);

                string status = vehicle == null
                                ? Resources.Status_Fail
                                : Resources.Status_Success;

                _logger.LogInformation($"{userLogin} - " +
                                       $"{Resources.Operation_UpdateVehicle} " +
                                       $"({Id} {vehicleModel.Id}) - " +
                                       $"{status}");

                return vehicle;
            }
            catch (Exception e)
            {
                _logger.LogError($"{userLogin} - " +
                                 $"{Resources.Operation_UpdateVehicle} - " +
                                 $"{e.GetType()}");

                throw;
            }
        }

        public async Task<Vehicle> DeleteVehicleByIdAsync(int id, string userLogin)
        {
            try
            {
                Vehicle vehicle = await _inner.DeleteVehicleByIdAsync(id, userLogin);

                string status = vehicle == null
                                ? Resources.Status_Fail
                                : Resources.Status_Success;

                _logger.LogInformation($"{userLogin} - " +
                                       $"{Resources.Operation_DeleteVehicle} " +
                                       $"({Id} {id}) - " +
                                       $"{status}");

                return vehicle;
            }
            catch (Exception e)
            {
                _logger.LogError($"{userLogin} - " +
                                 $"{Resources.Operation_DeleteVehicle} - " +
                                 $"{e.GetType()}");

                throw;
            }
        }

        public async Task<Vehicle> GetVehicleByIdAsync(int id, string userLogin)
        {
            try
            {
                Vehicle vehicle = await _inner.GetVehicleByIdAsync(id, userLogin);

                string status = vehicle == null
                                ? Resources.Status_NotFound
                                : Resources.Status_Success;

                _logger.LogInformation($"{userLogin} - " +
                                       $"{Resources.Operation_GetVehicle} " +
                                       $"({Id} {id}) - " +
                                       $"{status}");

                return vehicle;
            }
            catch (Exception e)
            {
                _logger.LogError($"{userLogin} - " +
                                 $"{Resources.Operation_GetVehicle} - " +
                                 $"{e.GetType()}");

                throw;
            }
        }

        public async Task<List<Vehicle>> GetAllVehiclesAsync(string userLogin)
        {
            try
            {
                List<Vehicle> vehicles = await _inner.GetAllVehiclesAsync(userLogin);

                _logger.LogInformation($"{userLogin} - " +
                                       $"{Resources.Operation_GetAllVehicles} - " +
                                       $"{Resources.Status_Success}");

                return vehicles;
            }
            catch (Exception e)
            {
                _logger.LogError($"{userLogin} - " +
                                 $"{Resources.Operation_GetAllVehicles} - " +
                                 $"{e.GetType()}");

                throw;
            }
        }

        public async Task<Vehicle> RemoveVehicleByIdAsync(int id, string userLogin)
        {
            try
            {
                Vehicle vehicle = await _inner.RemoveVehicleByIdAsync(id, userLogin);

                string status = vehicle == null
                                ? Resources.Status_Fail
                                : Resources.Status_Success;

                _logger.LogInformation($"{userLogin} - " +
                                       $"{Resources.Operation_RemoveVehicle} " +
                                       $"({Id} {id}) - " +
                                       $"{status}");

                return vehicle;
            }
            catch (Exception e)
            {
                _logger.LogError($"{userLogin} - " +
                                 $"{Resources.Operation_RemoveVehicle} - " +
                                 $"{e.GetType()}");
                throw;
            }
        }
    }
}