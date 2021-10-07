using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Microsoft.Extensions.Logging;
using Models;
using Services.Abstract;

namespace Services.Decorators.UsersServiceDecorators
{
    public class UsersServiceLoggerDecorator : IUsersService
    {
        private readonly ILogger<UsersServiceLoggerDecorator> _logger;
        private readonly IUsersService _inner;
        private const string Id = "Id";
        private const string Login = "Login";

        public UsersServiceLoggerDecorator(ILogger<UsersServiceLoggerDecorator> logger, IUsersService inner)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _inner = inner ?? throw new ArgumentNullException(nameof(inner));

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding.GetEncoding("windows-1251");
        }

        public async Task<UserResponse> AddUserAsync(UserRequestModel userRequestModel, string userLogin)
        {
            try
            {
                var userResponse = await _inner.AddUserAsync(userRequestModel, userLogin);

                string status, addedUserId;

                if (userResponse == null)
                {
                    status = Resources.Status_Fail;
                    addedUserId = "";
                }
                else
                {
                    status = Resources.Status_Success;
                    addedUserId = $"{Id} {userResponse.Id}";
                }

                _logger.LogInformation($"{userLogin} - " +
                                       $"{Resources.Operation_AddUser} " +
                                       $"({addedUserId}) " +
                                       $"- {status}");

                return userResponse;
            }
            catch (Exception e)
            {
                _logger.LogError($"{userLogin} - " +
                                 $"{Resources.Operation_AddUser} - " +
                                 $"{e.GetType()}");

                if (e.InnerException != null)
                {
                    _logger.LogError($"{e.InnerException.Message}");
                }

                throw;
            }
        }

        public async Task<UserResponse> UpdateUserAsync(UserRequestModel userRequestModel, string userLogin)
        {
            try
            {
                var userResponse = await _inner.UpdateUserAsync(userRequestModel, userLogin);

                string status = userResponse == null
                                ? Resources.Status_Fail
                                : Resources.Status_Success;

                _logger.LogInformation($"{userLogin} - " +
                                       $"{Resources.Operation_UpdateUser} " +
                                       $"({Id} {userRequestModel.Id}, {Login} {userRequestModel.Login}) - " + 
                                       $"{status}");

                return userResponse;
            }
            catch (Exception e)
            {
                _logger.LogError($"{userLogin} - " +
                                 $"{Resources.Operation_UpdateUser} " +
                                 $"({Id} {userRequestModel.Id}, {Login} {userRequestModel.Login}) - " +
                                 $"{e.GetType()}");

                throw;
            }
        }

        public async Task<UserResponse> DeleteUserByIdAsync(int id, string userLogin)
        {
            try
            {
                var userResponse = await _inner.DeleteUserByIdAsync(id, userLogin);

                string status = userResponse == null
                                ? Resources.Status_Fail
                                : Resources.Status_Success;

                _logger.LogInformation($"{userLogin} - " +
                                       $"{Resources.Operation_DeleteUser} " +
                                       $"({Id} {id}) - " + 
                                       $"{status}");

                return userResponse;
            }
            catch (Exception e)
            {
                _logger.LogError($"{userLogin} - " +
                                 $"{Resources.Operation_DeleteUser} " +
                                 $"({Id} {id}) - " +
                                 $"{e.GetType()}");

                throw;
            }
        }

        /// <summary>
        ///     GetUserByLoginAsync ищет пользователя по логину
        /// </summary>
        /// <param name="login">Логин пользователя, которого нужно найти</param>
        /// <param name="userLogin">Логин авторизаванного пользователя, которой запрашивает поиск</param>
        /// <returns></returns>
        public async Task<UserResponse> GetUserByLoginAsync(string login, string userLogin)
        {
            try
            {
                var userResponse = await _inner.GetUserByLoginAsync(login, userLogin);

                string status = userResponse == null
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
                _logger.LogError($"{userLogin} - " +
                                 $"{Resources.Operation_GetUserByLogin} - " +
                                 $"{e.GetType()}");

                throw;
            }
        }

        public async Task<List<UserResponse>> GetAllUsersAsync(string userLogin)
        {
            try
            {
                var users = await _inner.GetAllUsersAsync(userLogin);

                string status = users == null
                                ? Resources.Status_Fail
                                : Resources.Status_Success;

                _logger.LogInformation($"{userLogin} - " +
                                       $"{Resources.Operation_GetAllUsers} - " +
                                       $"{status}");

                return users;
            }
            catch (Exception e)
            {
                _logger.LogError($"{userLogin} - " +
                                 $"{Resources.Operation_GetAllUsers} - " +
                                 $"{e.GetType()}");

                throw;
            }
        }

        public async Task<UserResponse> RemoveUserByIdAsync(int id, string userLogin)
        {
            try
            {
                var userResponse = await _inner.RemoveUserByIdAsync(id, userLogin);

                string status = userResponse == null
                                ? Resources.Status_Fail
                                : Resources.Status_Success;

                _logger.LogInformation($"{userLogin} - " +
                                       $"{Resources.Operation_RemoveUser} " +
                                       $"({Id} {id}) - " +
                                       $"{status}");

                return userResponse;
            }
            catch (Exception e)
            {
                _logger.LogError($"{userLogin} - " +
                                 $"{Resources.Operation_RemoveUser} " +
                                 $"({Id} {id}) - " +
                                 $"{e.GetType()}");

                throw;
            }
        }
    }
}
