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
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _inner = inner ?? throw new ArgumentNullException(nameof(inner));
        }

        public async Task<Driver> AddDriverAsync(DriverModel driverModel, string userLogin)
        {
            try
            {
                if (driverModel == null) throw new ArgumentNullException(nameof(driverModel));
                if (userLogin == null) throw new ArgumentNullException(nameof(userLogin));

                var driver = await _inner.AddDriverAsync(driverModel, userLogin);

                var log = driver == null
                          ? $"{userLogin} - {Resources.Operation_AddDriver} - {Resources.Status_Fail}"
                          : $"{userLogin} - {Resources.Operation_AddDriver} ({Id} {driver.Id}) - {Resources.Status_Success}";

                _logger.LogInformation(log);

                return driver;
            }
            catch (Exception e)
            {
                var log = userLogin == null 
                          ? $"{Resources.Operation_AddDriver} - {e.GetType()}" 
                          : $"{userLogin} - {Resources.Operation_AddDriver} - {e.GetType()}";

                _logger.LogError(log);

                throw;
            }
        }

        public async Task<Driver> UpdateDriverAsync(DriverModel driverModel, string userLogin)
        {
            try
            {
                if (driverModel == null) throw new ArgumentNullException(nameof(driverModel));
                if (userLogin == null) throw new ArgumentNullException(nameof(userLogin));

                var driver = await _inner.UpdateDriverAsync(driverModel, userLogin);

                var status = driver == null
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
                var log = userLogin == null 
                          ? $"{Resources.Operation_UpdateDriver} - {e.GetType()}" 
                          : $"{userLogin} - {Resources.Operation_UpdateDriver} - {e.GetType()}";

                _logger.LogError(log);

                throw;
            }
        }

        public async Task<Driver> DeleteDriverByIdAsync(int id, string userLogin)
        {
            try
            {
                if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));
                if (userLogin == null) throw new ArgumentNullException(nameof(userLogin));

                var driver = await _inner.DeleteDriverByIdAsync(id, userLogin);

                var status = driver == null
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
                var log = userLogin == null 
                          ? $"{Resources.Operation_DeleteDriver} - {e.GetType()}" 
                          : $"{userLogin} - {Resources.Operation_DeleteDriver} - {e.GetType()}";

                _logger.LogError(log);

                throw;
            }
        }

        public async Task<Driver> GetDriverByIdAsync(int id, string userLogin)
        {
            try
            {
                if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));
                if (userLogin == null) throw new ArgumentNullException(nameof(userLogin));

                var driver = await _inner.GetDriverByIdAsync(id, userLogin);

                var status = driver == null
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
                var log = userLogin == null 
                          ? $"{Resources.Operation_GetDriver} - {e.GetType()}" 
                          : $"{userLogin} - {Resources.Operation_GetDriver} - {e.GetType()}";

                _logger.LogError(log);

                throw;
            }
        }

        public async Task<List<Driver>> GetAllDriversAsync(string userLogin)
        {
            try
            {
                if (userLogin == null) throw new ArgumentNullException(nameof(userLogin));

                var drivers = await _inner.GetAllDriversAsync(userLogin);

                _logger.LogInformation($"{userLogin} - " + 
                                       $"{Resources.Operation_GetAllDrivers} - " + 
                                       $"{Resources.Status_Success}");

                return drivers;
            }
            catch (Exception e)
            {
                var log = userLogin == null 
                          ? $"{Resources.Operation_GetAllDrivers} - {e.GetType()}" 
                          : $"{userLogin} - {Resources.Operation_GetAllDrivers} - {e.GetType()}";

                _logger.LogError(log);

                throw;
            }
        }

        public async Task<Driver> RemoveDriverByIdAsync(int id, string userLogin)
        {
            try
            {
                if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));
                if (userLogin == null) throw new ArgumentNullException(nameof(userLogin));

                var driver = await _inner.RemoveDriverByIdAsync(id, userLogin);

                var status = driver == null
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
                var log = userLogin == null 
                          ? $"{Resources.Operation_RemoveDriver} - {e.GetType()}" 
                          : $"{userLogin} - {Resources.Operation_RemoveDriver} - {e.GetType()}";

                _logger.LogError(log);

                throw;
            }
        }
    }
}