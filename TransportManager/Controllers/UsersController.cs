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
            if (login == null) throw new ArgumentNullException(nameof(login));

            var userLogin = HttpContext.User.Identity.Name;

            var userResponse = await _usersService.GetUserByLoginAsync(login, userLogin);

            if (userResponse == null) return NoContent();

            var userModel = _mapper.Map<UserResponseModel>(userResponse);

            return Ok(userModel);
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
            if (userRequestModel == null) throw new ArgumentNullException(nameof(userRequestModel));
            if (userRequestModel.Id != 0) throw new UserErrorException(Resources.Error_IdAssignment);
            if (userRequestModel.IsDeleted) throw new UserErrorException(Resources.Error_IsDeletedTrue);
            if (userRequestModel.SoftDeletedDate != null) 
                throw new UserErrorException(Resources.Error_SoftDeletedDateAssignment);

            var userLogin = HttpContext.User.Identity.Name;

            var userResponse = await _usersService.AddUserAsync(userRequestModel, userLogin);

            var addedUserModel = _mapper.Map<UserResponseModel>(userResponse);

            return Ok(addedUserModel.Id);
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
            if (userRequestModel == null) throw new ArgumentNullException(nameof(userRequestModel));
            if (userRequestModel.IsDeleted) throw new UserErrorException(Resources.Error_IsDeletedTrue);
            if (userRequestModel.SoftDeletedDate != null)
                throw new UserErrorException(Resources.Error_SoftDeletedDateAssignment);

            var userLogin = HttpContext.User.Identity.Name;

            var updUserResponse = await _usersService.UpdateUserAsync(userRequestModel, userLogin);

            if (updUserResponse == null) return NoContent();

            var updUserModel = _mapper.Map<UserResponseModel>(updUserResponse);

            return Ok(updUserModel.Id);
        }

        /// <summary>
        ///     Удалить пользователя
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Route("")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUserByLoginAsync(string login)
        {
            if (login == null) throw new ArgumentNullException(nameof(login));

            var userLogin = HttpContext.User.Identity.Name;

            if (login == userLogin) throw new UserErrorException(Resources.Error_CurrentAcc);

            var userResponse = await _usersService.DeleteUserByLoginAsync(login, userLogin);

            if (userResponse == null) return NoContent();

            var userModel = _mapper.Map<UserResponseModel>(userResponse);

            return Ok(userModel.Id);
        }

        /// <summary>
        ///     Получить всех пользователей
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("get_all")]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            var userLogin = HttpContext.User.Identity.Name;

            var users = await _usersService.GetAllUsersAsync(userLogin);

            if (users == null || users.Count == 0) return NoContent();

            var usersModels = users.Select(user => _mapper.Map<UserResponseModel>(user))
                                   .ToList();

            return Ok(usersModels);
        }

        /// <summary>
        ///     Скрыть пользователя
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("remove")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveUserByLoginAsync(string login)
        {
            if (login == null) throw new ArgumentNullException(nameof(login));

            var userLogin = HttpContext.User.Identity.Name;

            if (login == userLogin) throw new UserErrorException(Resources.Error_CurrentAcc);

            var userResponse = await _usersService.RemoveUserByLoginAsync(login, userLogin);

            if (userResponse == null) return NoContent();

            var userModel = _mapper.Map<UserResponseModel>(userResponse);

            return Ok(userModel.Id);
        }
    }
}
