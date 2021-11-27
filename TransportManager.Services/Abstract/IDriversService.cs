using System.Collections.Generic;
using System.Threading.Tasks;
using TransportManager.Domain;
using TransportManager.Models;

namespace TransportManager.Services.Abstract
{
    public interface IDriversService
    {
        Task<Driver> GetDriverAsync(int id, string userLogin);
        Task<Driver> AddDriverAsync(DriverModel driverModel, string userLogin);
        Task<Driver> UpdateDriverAsync(DriverModel driverModel, string userLogin);
        Task<Driver> DeleteDriverAsync(int id, string userLogin);
        Task<List<Driver>> GetAllDriversAsync(string userLogin);
        Task<Driver> RemoveDriverAsync(int id, string userLogin);
    }
}