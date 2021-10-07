using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Models;

namespace Services.Abstract
{
    public interface IVehiclesService
    {
        Task<Vehicle> GetVehicleByIdAsync(int id, string userLogin);
        Task<Vehicle> AddVehicleAsync(VehicleModel vehicleModel, string userLogin);
        Task<Vehicle> UpdateVehicleAsync(VehicleModel vehicleModel, string userLogin);
        Task<Vehicle> DeleteVehicleByIdAsync(int id, string userLogin);
        Task<List<Vehicle>> GetAllVehiclesAsync(string userLogin);
        Task<Vehicle> RemoveVehicleByIdAsync(int id, string userLogin);
    }
}