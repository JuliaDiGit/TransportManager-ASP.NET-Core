using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Entities;

namespace Data.Repositories.Abstract
{
    public interface IDriversRepository
    {
        Task<DriverEntity> AddDriverAsync(Driver driver);
        Task<DriverEntity> UpdateDriverAsync(Driver driver);
        Task<DriverEntity> DeleteDriverAsync(int id);
        Task<DriverEntity> GetDriverAsync(int id);
        Task<List<DriverEntity>> GetAllDriversAsync();
        Task<DriverEntity> RemoveDriverAsync(int id);
    }
}