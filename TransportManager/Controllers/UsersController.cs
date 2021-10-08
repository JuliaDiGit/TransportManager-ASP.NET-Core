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
    ///     Контроллер пользователей
    /// </summary>
    [ApiController]
    [Route("users")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;
        private readonly IMapper _mapper;

        public UsersController(IUsersService usersService, IMapper mapper)
        {
            _usersService = usersService ?? throw new ArgumentNullException(nameof(usersService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        ///     Получить пользователя
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetUserByLoginAsync(string login)
        {
            try
            {
                if (login == null) throw new ArgumentNullException(nameof(login));

                var userLogin = HttpContext.User.Identity.Name;

                var userResponse = await _usersService.GetUserByLoginAsync(login, userLogin);

                if (userResponse == null) return NotFound();

                var userModel = _mapper.Map<UserResponseModel>(userResponse);

                return Ok(userModel);
            }
            catch (Exception e)
            {
                if (e is Npgsql.PostgresException) return BadRequest("Ошибка работы с БД");
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        ///     Добавить пользователя
        /// </summary>
        /// <param name="userRequestModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddUserAsync(UserRequestModel userRequestModel)
        {
            try
            {
                if (userRequestModel == null) throw new ArgumentNullException(nameof(userRequestModel));

                var userLogin = HttpContext.User.Identity.Name;

                var userResponse = await _usersService.AddUserAsync(userRequestModel, userLogin);
                var addedUserModel = _mapper.Map<UserResponseModel>(userResponse);

                return Ok(addedUserModel);
            }
            catch (Exception e)
            {
                if (e is Npgsql.PostgresException) return BadRequest("Ошибка работы с БД");
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        ///     Обновить данные пользователя
        /// </summary>
        /// <param name="userRequestModel"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUserAsync(UserRequestModel userRequestModel)
        {
            try
            {
                if (userRequestModel == null) throw new ArgumentNullException(nameof(userRequestModel));

                var userLogin = HttpContext.User.Identity.Name;

                var updUserResponse = await _usersService.UpdateUserAsync(userRequestModel, userLogin);

                if (updUserResponse == null) return NotFound();

                var updUserModel = _mapper.Map<UserResponseModel>(updUserResponse);

                return Ok(updUserModel);
            }
            catch (Exception e)
            {
                if (e is Npgsql.PostgresException) return BadRequest("Ошибка работы с БД");
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        ///     Удалить пользователя
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUserAsync(int id)
        {
            try
            {
                if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));

                var userLogin = HttpContext.User.Identity.Name;

                var userResponse = await _usersService.DeleteUserByIdAsync(id, userLogin);

                if (userResponse == null) return NotFound();

                var userModel = _mapper.Map<UserResponseModel>(userResponse);

                return Ok(userModel);
            }
            catch (Exception e)
            {
                if (e is Npgsql.PostgresException) return BadRequest("Ошибка работы с БД");
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        ///     Получить всех пользователей
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("get_all")]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            try
            {
                var userLogin = HttpContext.User.Identity.Name;

                var users = await _usersService.GetAllUsersAsync(userLogin);

                if (users == null) return NotFound();

                var usersModels = users.Select(user => _mapper.Map<UserResponseModel>(user))
                                       .ToList();

                return Ok(usersModels);
            }
            catch (Exception e)
            {
                if (e is Npgsql.PostgresException) return BadRequest("Ошибка работы с БД");
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        ///     Скрыть пользователя
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("remove")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveUserByIdAsync(int id)
        {
            try
            {
                if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));

                var userLogin = HttpContext.User.Identity.Name;

                var userResponse = await _usersService.RemoveUserByIdAsync(id, userLogin);

                if (userResponse == null) return NotFound();

                var userModel = _mapper.Map<UserResponseModel>(userResponse);

                return Ok(userModel);
            }
            catch (Exception e)
            {
                if (e is Npgsql.PostgresException) return BadRequest("Ошибка работы с БД");
                return BadRequest(e.Message);
            }
        }
    }
}
