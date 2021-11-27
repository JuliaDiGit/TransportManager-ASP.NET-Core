using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TransportManager.Domain;
using TransportManager.Models;
using TransportManager.Services.Abstract;

namespace TransportManager.Services.Decorators.UsersServiceDecorators
{
    public class UsersServiceLoggerDecorator : IUsersService
    {
        private readonly ILogger<UsersServiceLoggerDecorator> _logger;
        private readonly IUsersService _inner;
        private const string Login = "Login";

        public UsersServiceLoggerDecorator(ILogger<UsersServiceLoggerDecorator> logger, IUsersService inner)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _inner = inner ?? throw new ArgumentNullException(nameof(inner));
        }

        /// <summary>
        ///     GetUserByLoginAsync ищет пользователя по логину
        /// </summary>
        /// <param name="login">Логин пользователя, которого нужно найти</param>
        /// <param name="userLogin">Логин авторизаванного пользователя, который запрашивает поиск</param>
        /// <returns></returns>
        public async Task<UserResponse> GetUserByLoginAsync(string login, string userLogin)
        {
            try
            {
                if (login == null) throw new ArgumentNullException(nameof(login));
                if (userLogin == null) throw new ArgumentNullException(nameof(userLogin));

                var userResponse = await _inner.GetUserByLoginAsync(login, userLogin);

                var status = userResponse == null
                             ? Resources.Status_Fail
                             : Resources.Status_Success;

                _logger.LogInformation($"{userLogin} - " +
                                       $"{Resources.Operation_GetUserByLogin} " +
                                       $"({Login} {login}) - " +
                                       $"{status}");

                return userResponse;
            }
            catch (Exception e)
            {
                var log = userLogin == null
                          ? $"{Resources.Operation_GetUserByLogin} - {e.GetType()}"
                          : $"{userLogin} - {Resources.Operation_GetUserByLogin} - {e.GetType()}";

                _logger.LogError(log);

                throw;
            }
        }
        public async Task<UserResponse> AddUserAsync(UserRequestModel userRequestModel, string userLogin)
        {
            try
            {
                if (userRequestModel == null) throw new ArgumentNullException(nameof(userRequestModel));
                if (userLogin == null) throw new ArgumentNullException(nameof(userLogin));

                var userResponse = await _inner.AddUserAsync(userRequestModel, userLogin);

                var log = userResponse == null
                          ? $"{userLogin} - {Resources.Operation_AddUser} - {Resources.Status_Fail}"
                          : $"{userLogin} - {Resources.Operation_AddUser} ({Login} {userRequestModel.Login}) " +
                            $"- {Resources.Status_Success}";

                _logger.LogInformation(log);

                return userResponse;
            }
            catch (Exception e)
            {
                var log = userLogin == null
                          ? $"{Resources.Operation_AddUser} - {e.GetType()}"
                          : $"{userLogin} - {Resources.Operation_AddUser} - {e.GetType()}";

                _logger.LogError(log);

                throw;
            }
        }

        public async Task<UserResponse> UpdateUserAsync(UserRequestModel userRequestModel, string userLogin)
        {
            try
            {
                if (userRequestModel == null) throw new ArgumentNullException(nameof(userRequestModel));
                if (userLogin == null) throw new ArgumentNullException(nameof(userLogin));

                var userResponse = await _inner.UpdateUserAsync(userRequestModel, userLogin);

                var status = userResponse == null
                             ? Resources.Status_Fail
                             : Resources.Status_Success;

                _logger.LogInformation($"{userLogin} - " +
                                       $"{Resources.Operation_UpdateUser} " +
                                       $"({Login} {userRequestModel.Login}) - " + 
                                       $"{status}");

                return userResponse;
            }
            catch (Exception e)
            {
                var log = userLogin == null
                          ? $"{Resources.Operation_UpdateUser} - {e.GetType()}"
                          : $"{userLogin} - {Resources.Operation_UpdateUser} - {e.GetType()}";

                _logger.LogError(log);

                throw;
            }
        }

        /// <summary>
        ///     DeleteUserByLoginAsync удаляет пользователя по логину
        /// </summary>
        /// <param name="login">Логин пользователя, которого нужно найти</param>
        /// <param name="userLogin">Логин авторизаванного пользователя, который запрашивает поиск</param>
        /// <returns></returns>
        public async Task<UserResponse> DeleteUserByLoginAsync(string login, string userLogin)
        {
            try
            {
                if (login == null) throw new ArgumentNullException(nameof(userLogin));
                if (userLogin == null) throw new ArgumentNullException(nameof(userLogin));

                var userResponse = await _inner.DeleteUserByLoginAsync(login, userLogin);

                var status = userResponse == null
                             ? Resources.Status_Fail
                             : Resources.Status_Success;

                _logger.LogInformation($"{userLogin} - " +
                                       $"{Resources.Operation_DeleteUser} " +
                                       $"({Login} {login}) - " + 
                                       $"{status}");

                return userResponse;
            }
            catch (Exception e)
            {
                var log = userLogin == null
                          ? $"{Resources.Operation_DeleteUser} - {e.GetType()}"
                          : $"{userLogin} - {Resources.Operation_DeleteUser} - {e.GetType()}";

                _logger.LogError(log);

                throw;
            }
        }

        public async Task<List<UserResponse>> GetAllUsersAsync(string userLogin)
        {
            try
            {
                if (userLogin == null) throw new ArgumentNullException(nameof(userLogin));

                var users = await _inner.GetAllUsersAsync(userLogin);

                var status = users == null
                             ? Resources.Status_Fail
                             : Resources.Status_Success;

                _logger.LogInformation($"{userLogin} - " +
                                       $"{Resources.Operation_GetAllUsers} - " +
                                       $"{status}");

                return users;
            }
            catch (Exception e)
            {
                var log = userLogin == null
                          ? $"{Resources.Operation_GetAllUsers} - {e.GetType()}"
                          : $"{userLogin} - {Resources.Operation_GetAllUsers} - {e.GetType()}";

                _logger.LogError(log);

                throw;
            }
        }

        /// <summary>
        ///     RemoveUserByLoginAsync скрывает пользователя по логину
        /// </summary>
        /// <param name="login">Логин пользователя, которого нужно найти</param>
        /// <param name="userLogin">Логин авторизаванного пользователя, который запрашивает поиск</param>
        /// <returns></returns>
        public async Task<UserResponse> RemoveUserByLoginAsync(string login, string userLogin)
        {
            try
            {
                if (login == null) throw new ArgumentNullException(nameof(userLogin));
                if (userLogin == null) throw new ArgumentNullException(nameof(userLogin));

                var userResponse = await _inner.RemoveUserByLoginAsync(login, userLogin);

                var status = userResponse == null
                             ? Resources.Status_Fail
                             : Resources.Status_Success;

                _logger.LogInformation($"{userLogin} - " +
                                       $"{Resources.Operation_RemoveUser} " +
                                       $"({Login} {login}) - " +
                                       $"{status}");

                return userResponse;
            }
            catch (Exception e)
            {
                var log = userLogin == null
                          ? $"{Resources.Operation_RemoveUser} - {e.GetType()}"
                          : $"{userLogin} - {Resources.Operation_RemoveUser} - {e.GetType()}";

                _logger.LogError(log);

                throw;
            }
        }
    }
}
