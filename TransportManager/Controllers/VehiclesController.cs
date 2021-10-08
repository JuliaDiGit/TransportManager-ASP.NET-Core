using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Abstract;

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
        public async Task<IActionResult> GetVehicleByIdAsync(int id)
        {
            try
            {
                if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));

                var userLogin = HttpContext.User.Identity.Name;

                var vehicle = await _vehiclesService.GetVehicleByIdAsync(id, userLogin);

                if (vehicle == null) return NotFound();

                var vehicleModel = _mapper.Map<VehicleModel>(vehicle);

                return Ok(vehicleModel);
            }
            catch (Exception e)
            {
                if (e is Npgsql.PostgresException) return BadRequest("Ошибка работы с БД");
                return BadRequest(e.Message);
            }
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
            try
            {
                if (vehicleModel == null) throw new ArgumentNullException(nameof(vehicleModel));

                var userLogin = HttpContext.User.Identity.Name;

                vehicleModel.GovernmentNumber = vehicleModel.GovernmentNumber.ToUpper();

                var vehicle = await _vehiclesService.AddVehicleAsync(vehicleModel, userLogin);
                var addedVehicleModel = _mapper.Map<VehicleModel>(vehicle);

                return Ok(addedVehicleModel);
            }
            catch (Exception e)
            {
                if (e is Npgsql.PostgresException) return BadRequest("Ошибка работы с БД");
                return BadRequest(e.Message);
            }
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
            try
            {
                if (vehicleModel == null) throw new ArgumentNullException(nameof(vehicleModel));

                var userLogin = HttpContext.User.Identity.Name;

                var updVehicle = await _vehiclesService.UpdateVehicleAsync(vehicleModel, userLogin);

                if (updVehicle == null) return NotFound();

                var updVehicleModel = _mapper.Map<VehicleModel>(updVehicle);

                return Ok(updVehicleModel);
            }
            catch (Exception e)
            {
                if (e is Npgsql.PostgresException) return BadRequest("Ошибка работы с БД");
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        ///     Удалить ТС 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteVehicleByIdAsync(int id)
        {
            try
            {
                if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));

                var userLogin = HttpContext.User.Identity.Name;

                var vehicle = await _vehiclesService.DeleteVehicleByIdAsync(id, userLogin);

                if (vehicle == null) return NotFound();

                var vehicleModel = _mapper.Map<VehicleModel>(vehicle);

                return Ok(vehicleModel);
            }
            catch (Exception e)
            {
                if (e is Npgsql.PostgresException) return BadRequest("Ошибка работы с БД");
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        ///     Получить все ТС
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("get_all")]
        public async Task<IActionResult> GetAllVehiclesAsync()
        {
            try
            {
                var userLogin = HttpContext.User.Identity.Name;

                var vehicles = await _vehiclesService.GetAllVehiclesAsync(userLogin);

                if (vehicles == null) return NotFound();

                var vehicleModels = vehicles.Select(vehicle => _mapper.Map<VehicleModel>(vehicle))
                    .ToList();

                return Ok(vehicleModels);
            }
            catch (Exception e)
            {
                if (e is Npgsql.PostgresException) return BadRequest("Ошибка работы с БД");
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        ///     Скрыть ТС
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("remove")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveVehicleByIdAsync(int id)
        {
            try
            {
                if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));

                var userLogin = HttpContext.User.Identity.Name;

                var vehicle = await _vehiclesService.RemoveVehicleByIdAsync(id, userLogin);

                if (vehicle == null) return NotFound();

                var vehicleModel = _mapper.Map<VehicleModel>(vehicle);

                return Ok(vehicleModel);
            }
            catch (Exception e)
            {
                if (e is Npgsql.PostgresException) return BadRequest("Ошибка работы с БД");
                return BadRequest(e.Message);
            }
        }
    }
}