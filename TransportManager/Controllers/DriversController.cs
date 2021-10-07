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
    ///     Контроллер водителей
    /// </summary>
    [ApiController]
    [Route("drivers")]
    [Authorize]
    public class DriversController : ControllerBase
    {
        private readonly IDriversService _driversService;
        private readonly IMapper _mapper;

        public DriversController(IDriversService driversService, IMapper mapper)
        {
            _driversService = driversService ?? throw new ArgumentNullException(nameof(driversService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        ///     Получить водителя
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetDriverByIdAsync(int id)
        {
            try
            {
                if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));

                var userLogin = HttpContext.User.Identity.Name;

                var driver = await _driversService.GetDriverByIdAsync(id, userLogin);

                if (driver == null) return NotFound();

                var driverModel = _mapper.Map<DriverModel>(driver);

                return Ok(driverModel);
            }
            catch (Exception e)
            {
                if (e is Npgsql.PostgresException) return BadRequest("Ошибка работы БД");
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        ///     Добавить водителя
        /// </summary>
        /// <param name="driverModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddDriverAsync(DriverModel driverModel)
        {
            try
            {
                if (driverModel == null) throw new ArgumentNullException(nameof(driverModel));

                var userLogin = HttpContext.User.Identity.Name;

                var driver = await _driversService.AddDriverAsync(driverModel, userLogin);
                var addedDriverModel = _mapper.Map<DriverModel>(driver);

                return Ok(addedDriverModel);
            }
            catch (Exception e)
            {
                if (e is Npgsql.PostgresException) return BadRequest("Ошибка работы БД");
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        ///     Обновить водителя
        /// </summary>
        /// <param name="driverModel"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateDriverAsync(DriverModel driverModel)
        {
            try
            {
                if (driverModel == null) throw new ArgumentNullException(nameof(driverModel));

                var userLogin = HttpContext.User.Identity.Name;

                var updDriver = await _driversService.UpdateDriverAsync(driverModel, userLogin);

                if (updDriver == null) return NotFound();

                var updDriverModel = _mapper.Map<DriverModel>(updDriver);

                return Ok(updDriverModel);
            }
            catch (Exception e)
            {
                if (e is Npgsql.PostgresException) return BadRequest("Ошибка работы БД");
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        ///     Удалить водителя
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteDriverByIdAsync(int id)
        {
            try
            {
                if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));

                var userLogin = HttpContext.User.Identity.Name;

                var driver = await _driversService.DeleteDriverByIdAsync(id, userLogin);

                if (driver == null) return NotFound();

                var driverModel = _mapper.Map<DriverModel>(driver);

                return Ok(driverModel);
            }
            catch (Exception e)
            {
                if (e is Npgsql.PostgresException) return BadRequest("Ошибка работы БД");
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        ///     Получить всех водителей
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("get_all")]
        public async Task<IActionResult> GetAllDriversAsync()
        {
            try
            {
                var userLogin = HttpContext.User.Identity.Name;

                var drivers = await _driversService.GetAllDriversAsync(userLogin);

                if (drivers == null) return NotFound();

                var driverModels = drivers.Select(driver => _mapper.Map<DriverModel>(driver))
                                          .ToList();

                return Ok(driverModels);
            }
            catch (Exception e)
            {
                if (e is Npgsql.PostgresException) return BadRequest("Ошибка работы БД");
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        ///     Скрыть водителя
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("remove")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveDriverByIdAsync(int id)
        {
            try
            {
                if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));

                var userLogin = HttpContext.User.Identity.Name;

                var driver = await _driversService.RemoveDriverByIdAsync(id, userLogin);

                if (driver == null) return NotFound();

                var driverModel = _mapper.Map<DriverModel>(driver);

                return Ok(driverModel);
            }
            catch (Exception e)
            {
                if (e is Npgsql.PostgresException) return BadRequest("Ошибка работы БД");
                return BadRequest(e.Message);
            }
        }
    }
}