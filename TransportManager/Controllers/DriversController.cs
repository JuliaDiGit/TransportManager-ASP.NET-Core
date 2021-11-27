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
        public async Task<IActionResult> GetDriverAsync(int id)
        {

            if (id <= 0) throw new UserErrorException(Resources.Error_IncorrectId);

            var userLogin = HttpContext.User.Identity.Name;

            var driver = await _driversService.GetDriverAsync(id, userLogin);

            if (driver == null) return NoContent();

            var driverModel = _mapper.Map<DriverModel>(driver);

            return Ok(driverModel);
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
            if (driverModel == null) throw new ArgumentNullException(nameof(driverModel));
            if (driverModel.Id != 0) throw new UserErrorException(Resources.Error_IdAssignment);
            if (driverModel.IsDeleted) throw new UserErrorException(Resources.Error_IsDeletedTrue);
            if (driverModel.SoftDeletedDate != null) 
                throw new UserErrorException(Resources.Error_SoftDeletedDateAssignment);

            var userLogin = HttpContext.User.Identity.Name;

            var driver = await _driversService.AddDriverAsync(driverModel, userLogin);
            var addedDriverModel = _mapper.Map<DriverModel>(driver);

            return Ok(addedDriverModel.Id);
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
            if (driverModel == null) throw new ArgumentNullException(nameof(driverModel));
            if (driverModel.Id <= 0) throw new UserErrorException(Resources.Error_IncorrectId);
            if (driverModel.IsDeleted) throw new UserErrorException(Resources.Error_IsDeletedTrue);
            if (driverModel.SoftDeletedDate != null) 
                throw new UserErrorException(Resources.Error_SoftDeletedDateAssignment);

            var userLogin = HttpContext.User.Identity.Name;

            var updDriver = await _driversService.UpdateDriverAsync(driverModel, userLogin);

            if (updDriver == null) return NoContent();

            var updDriverModel = _mapper.Map<DriverModel>(updDriver);

            return Ok(updDriverModel.Id);
        }

        /// <summary>
        ///     Удалить водителя
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteDriverAsync(int id)
        {
            if (id <= 0) throw new UserErrorException(Resources.Error_IncorrectId);

            var userLogin = HttpContext.User.Identity.Name;

            var driver = await _driversService.DeleteDriverAsync(id, userLogin);

            if (driver == null) return NoContent();

            var driverModel = _mapper.Map<DriverModel>(driver);

            return Ok(driverModel.Id);
        }

        /// <summary>
        ///     Получить всех водителей
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("get_all")]
        public async Task<IActionResult> GetAllDriversAsync()
        {

            var userLogin = HttpContext.User.Identity.Name;

            var drivers = await _driversService.GetAllDriversAsync(userLogin);

            if (drivers == null || drivers.Count == 0) return NoContent();

            var driverModels = drivers.Select(driver => _mapper.Map<DriverModel>(driver))
                                      .ToList();

            return Ok(driverModels);

        }

        /// <summary>
        ///     Скрыть водителя
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("remove")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveDriverAsync(int id)
        {
            if (id <= 0) throw new UserErrorException(Resources.Error_IncorrectId);

            var userLogin = HttpContext.User.Identity.Name;

            var driver = await _driversService.RemoveDriverAsync(id, userLogin);

            if (driver == null) return NoContent();

            var driverModel = _mapper.Map<DriverModel>(driver);

            return Ok(driverModel.Id);
        }
    }
}