using System.Collections.Generic;
using System.Threading.Tasks;
using TransportManager.Domain;
using TransportManager.Entities;

namespace TransportManager.Data.Repositories.Abstract
{
    public interface IDriversRepository
    {
        Task<DriverEntity> GetDriverAsync(int id);
        Task<DriverEntity> AddDriverAsync(Driver driver);
        Task<DriverEntity> UpdateDriverAsync(Driver driver);
        Task<DriverEntity> DeleteDriverAsync(int id);
        Task<List<DriverEntity>> GetAllDriversAsync();
        Task<DriverEntity> RemoveDriverAsync(int id);
    }
}