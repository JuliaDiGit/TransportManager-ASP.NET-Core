using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Models;


namespace Services.Abstract
{
    public interface IDriversService
    {
        Task<Driver> GetDriverByIdAsync(int id, string userLogin);
        Task<Driver> AddDriverAsync(DriverModel driverModel, string userLogin);
        Task<Driver> UpdateDriverAsync(DriverModel driverModel, string userLogin);
        Task<Driver> DeleteDriverByIdAsync(int id, string userLogin);
        Task<List<Driver>> GetAllDriversAsync(string userLogin);
        Task<Driver> RemoveDriverByIdAsync(int id, string userLogin);
    }
}