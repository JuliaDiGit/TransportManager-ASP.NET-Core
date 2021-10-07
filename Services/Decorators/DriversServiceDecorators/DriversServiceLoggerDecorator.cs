using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Microsoft.Extensions.Logging;
using Models;
using Services.Abstract;

namespace Services.Decorators.DriversServiceDecorators
{
    public class DriversServiceLoggerDecorator : IDriversService
    {
        private readonly ILogger<DriversServiceLoggerDecorator> _logger;
        private readonly IDriversService _inner;
        private const string Id = "Id";

        public DriversServiceLoggerDecorator(ILogger<DriversServiceLoggerDecorator> logger, IDriversService inner)
        {
            _logger = logger;
            _inner = inner;
        }

        public async Task<Driver> AddDriverAsync(DriverModel driverModel, string userLogin)
        {
            try
            {
                Driver driver = await _inner.AddDriverAsync(driverModel, userLogin);

                string status, addedDriverId;

                if (driver == null)
                {
                    status = Resources.Status_Fail;
                    addedDriverId = "";
                }
                else
                {
                    status = Resources.Status_Success;
                    addedDriverId = $"{Id} {driver.Id}";
                }

                _logger.LogInformation($"{userLogin} - " + 
                                       $"{Resources.Operation_AddDriver} " +
                                       $"({addedDriverId}) " +
                                       $"- {status}");

                return driver;
            }
            catch (Exception e)
            {
                _logger.LogError($"{userLogin} - " +
                                 $"{Resources.Operation_AddDriver} - " +
                                 $"{e.GetType()}");
                throw;
            }
        }

        public async Task<Driver> UpdateDriverAsync(DriverModel driverModel, string userLogin)
        {
            try
            {
                Driver driver = await _inner.UpdateDriverAsync(driverModel, userLogin);

                string status = driver == null
                                ? Resources.Status_Fail
                                : Resources.Status_Success;

                _logger.LogInformation($"{userLogin} - " + 
                                       $"{Resources.Operation_UpdateDriver} " +
                                       $"({Id} {driverModel.Id}) - " +
                                       $"{status}");

                return driver;
            }
            catch (Exception e)
            {
                _logger.LogError($"{userLogin} - " +
                                 $"{Resources.Operation_UpdateDriver} - " +
                                 $"{e.GetType()}");
                throw;
            }
        }

        public async Task<Driver> DeleteDriverByIdAsync(int id, string userLogin)
        {
            try
            {
                Driver driver = await _inner.DeleteDriverByIdAsync(id, userLogin);

                string status = driver == null
                                ? Resources.Status_Fail
                                : Resources.Status_Success;

                _logger.LogInformation($"{userLogin} - " + 
                                       $"{Resources.Operation_DeleteDriver} " +
                                       $"({Id} {id}) - " + 
                                       $"{status}");

                return driver;
            }
            catch (Exception e)
            {
                _logger.LogError($"{userLogin} - " +
                                 $"{Resources.Operation_DeleteDriver} - " +
                                 $"{e.GetType()}");
                throw;
            }
        }

        public async Task<Driver> GetDriverByIdAsync(int id, string userLogin)
        {
            try
            {
                Driver driver = await _inner.GetDriverByIdAsync(id, userLogin);

                string status = driver == null
                                ? Resources.Status_NotFound
                                : Resources.Status_Success;

                _logger.LogInformation($"{userLogin} - " + 
                                       $"{Resources.Operation_GetDriver} " +
                                       $"({Id} {id}) - " +
                                       $"{status}");

                return driver;
            }
            catch (Exception e)
            {
                _logger.LogError($"{userLogin} - " +
                                 $"{Resources.Operation_GetDriver} - " +
                                 $"{e.GetType()}");
                throw;
            }
        }

        public async Task<List<Driver>> GetAllDriversAsync(string userLogin)
        {
            try
            {
                List<Driver> drivers = await _inner.GetAllDriversAsync(userLogin);

                _logger.LogInformation($"{userLogin} - " + 
                                       $"{Resources.Operation_GetAllDrivers} - " + 
                                       $"{Resources.Status_Success}");

                return drivers;
            }
            catch (Exception e)
            {
                _logger.LogError($"{userLogin} - " +
                                 $"{Resources.Operation_GetAllDrivers} - " +
                                 $"{e.GetType()}");
                throw;
            }
        }

        public async Task<Driver> RemoveDriverByIdAsync(int id, string userLogin)
        {
            try
            {
                Driver driver = await _inner.RemoveDriverByIdAsync(id, userLogin);

                string status = driver == null
                                ? Resources.Status_Fail
                                : Resources.Status_Success;

                _logger.LogInformation($"{userLogin} - " + 
                                       $"{Resources.Operation_RemoveDriver} " +
                                       $"({Id} {id}) - " +
                                       $"{status}");

                return driver;
            }
            catch (Exception e)
            {
                _logger.LogError($"{userLogin} - " +
                                 $"{Resources.Operation_RemoveDriver} - " +
                                 $"{e.GetType()}");
                throw;
            }
        }
    }
}