using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TransportManager.Common.Exceptions;
using TransportManager.Models;
using TransportManager.Services.Abstract;

namespace TransportManager.Controllers
{
    /// <summary>
    ///     Контроллер транспортных средств
    /// </summary>
    [ApiController]
    [Route("vehicles")]
    [Authorize]
    public class VehiclesController : ControllerBase
    {
        private readonly IVehiclesService _vehiclesService;
        private readonly IMapper _mapper;

        public VehiclesController(IVehiclesService vehiclesService, IMapper mapper)
        {
            _vehiclesService = vehiclesService ?? throw new ArgumentNullException(nameof(vehiclesService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        ///     Получить ТС
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetVehicleAsync(int id)
        {
            if (id <= 0) throw new UserErrorException(Resources.Error_IncorrectId);

            var userLogin = HttpContext.User.Identity.Name;

            var vehicle = await _vehiclesService.GetVehicleAsync(id, userLogin);

            if (vehicle == null) return NoContent();

            var vehicleModel = _mapper.Map<VehicleModel>(vehicle);

            return Ok(vehicleModel);
        }

        /// <summary>
        ///     Добавить ТС
        /// </summary>
        /// <param name="vehicleModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddVehicleAsync(VehicleModel vehicleModel)
        {
            if (vehicleModel == null) throw new ArgumentNullException(nameof(vehicleModel));
            if (vehicleModel.Id != 0) throw new UserErrorException(Resources.Error_IdAssignment);
            if (vehicleModel.IsDeleted) throw new UserErrorException(Resources.Error_IsDeletedTrue);
            if (vehicleModel.SoftDeletedDate != null)
                throw new UserErrorException(Resources.Error_SoftDeletedDateAssignment);

            var userLogin = HttpContext.User.Identity.Name;

            var vehicle = await _vehiclesService.AddVehicleAsync(vehicleModel, userLogin);
            var addedVehicleModel = _mapper.Map<VehicleModel>(vehicle);

            return Ok(addedVehicleModel.Id);
        }

        /// <summary>
        ///     Обновить ТС
        /// </summary>
        /// <param name="vehicleModel"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateVehicleAsync(VehicleModel vehicleModel)
        {
            if (vehicleModel == null) throw new ArgumentNullException(nameof(vehicleModel));
            if (vehicleModel.Id <= 0) throw new UserErrorException(Resources.Error_IncorrectId);
            if (vehicleModel.IsDeleted) throw new UserErrorException(Resources.Error_IsDeletedTrue);
            if (vehicleModel.SoftDeletedDate != null)
                throw new UserErrorException(Resources.Error_SoftDeletedDateAssignment);

            var userLogin = HttpContext.User.Identity.Name;

            var updVehicle = await _vehiclesService.UpdateVehicleAsync(vehicleModel, userLogin);

            if (updVehicle == null) return NoContent();

            var updVehicleModel = _mapper.Map<VehicleModel>(updVehicle);

            return Ok(updVehicleModel.Id);
        }

        /// <summary>
        ///     Удалить ТС 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteVehicleAsync(int id)
        {
            if (id <= 0) throw new UserErrorException(Resources.Error_IncorrectId);

            var userLogin = HttpContext.User.Identity.Name;

            var vehicle = await _vehiclesService.DeleteVehicleAsync(id, userLogin);

            if (vehicle == null) return NoContent();

            var vehicleModel = _mapper.Map<VehicleModel>(vehicle);

            return Ok(vehicleModel.Id);
        }

        /// <summary>
        ///     Получить все ТС
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("get_all")]
        public async Task<IActionResult> GetAllVehiclesAsync()
        {
            var userLogin = HttpContext.User.Identity.Name;

            var vehicles = await _vehiclesService.GetAllVehiclesAsync(userLogin);

            if (vehicles == null || vehicles.Count == 0) return NoContent();

            var vehicleModels = vehicles.Select(vehicle => _mapper.Map<VehicleModel>(vehicle))
                                        .ToList();

            return Ok(vehicleModels);
        }

        /// <summary>
        ///     Скрыть ТС
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("remove")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveVehicleAsync(int id)
        {
            if (id <= 0) throw new UserErrorException(Resources.Error_IncorrectId);

            var userLogin = HttpContext.User.Identity.Name;

            var vehicle = await _vehiclesService.RemoveVehicleAsync(id, userLogin);

            if (vehicle == null) return NoContent();

            var vehicleModel = _mapper.Map<VehicleModel>(vehicle);

            return Ok(vehicleModel.Id);
        }
    }
}