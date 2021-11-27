using System.Collections.Generic;
using System.Threading.Tasks;
using TransportManager.Domain;
using TransportManager.Models;

namespace TransportManager.Services.Abstract
{
    public interface IVehiclesService
    {
        Task<Vehicle> GetVehicleAsync(int id, string userLogin);
        Task<Vehicle> AddVehicleAsync(VehicleModel vehicleModel, string userLogin);
        Task<Vehicle> UpdateVehicleAsync(VehicleModel vehicleModel, string userLogin);
        Task<Vehicle> DeleteVehicleAsync(int id, string userLogin);
        Task<List<Vehicle>> GetAllVehiclesAsync(string userLogin);
        Task<Vehicle> RemoveVehicleAsync(int id, string userLogin);
    }
}