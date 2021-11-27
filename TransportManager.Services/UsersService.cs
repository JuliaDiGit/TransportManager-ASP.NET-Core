using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TransportManager.Authorization.Helpers;
using TransportManager.Data.Repositories.Abstract;
using TransportManager.Domain;
using TransportManager.Models;
using TransportManager.Services.Abstract;

namespace TransportManager.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IMapper _mapper;

        public UsersService(IUsersRepository usersRepository, IMapper mapper)
        {
            _usersRepository = usersRepository ?? throw new ArgumentNullException(nameof(usersRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        ///     GetUserByLoginAsync ищет пользователя по логину
        /// </summary>
        /// <param name="login">Логин пользователя, которого нужно найти</param>
        /// <param name="userLogin">Логин авторизаванного пользователя, который запрашивает поиск</param>
        /// <returns></returns>
        public async Task<UserResponse> GetUserByLoginAsync(string login, string userLogin)
        {
            if (login == null) throw new ArgumentNullException(nameof(login));
            if (userLogin == null) throw new ArgumentNullException(nameof(userLogin));

            var userEntity = await _usersRepository.GetUserByLoginAsync(login);

            return _mapper.Map<UserResponse>(userEntity);
        }

        public async Task<UserResponse> AddUserAsync(UserRequestModel userRequestModel, string userLogin)
        {
            if (userRequestModel == null) throw new ArgumentNullException(nameof(userRequestModel));
            if (userLogin == null) throw new ArgumentNullException(nameof(userLogin));

            var user = _mapper.Map<UserRequest>(userRequestModel);

            // хешируем пароль
            user.Password = Hash.HashString(user.Password);

            var userEntity = await _usersRepository.AddUserAsync(user);

            return _mapper.Map<UserResponse>(userEntity);
        }

        public async Task<UserResponse> UpdateUserAsync(UserRequestModel userRequestModel, string userLogin)
        {
            if (userRequestModel == null) throw new ArgumentNullException(nameof(userRequestModel));
            if (userLogin == null) throw new ArgumentNullException(nameof(userLogin));

            var user = _mapper.Map<UserRequest>(userRequestModel);

            // хешируем пароль
            user.Password = Hash.HashString(user.Password);

            var userEntity = await _usersRepository.UpdateUserAsync(user);

            return _mapper.Map<UserResponse>(userEntity);
        }

        /// <summary>
        ///     DeleteUserByLoginAsync удаляет пользователя по логину
        /// </summary>
        /// <param name="login">Логин пользователя, которого нужно найти</param>
        /// <param name="userLogin">Логин авторизаванного пользователя, который запрашивает поиск</param>
        /// <returns></returns>
        public async Task<UserResponse> DeleteUserByLoginAsync(string login, string userLogin)
        {
            if (login == null) throw new ArgumentNullException(nameof(login));
            if (userLogin == null) throw new ArgumentNullException(nameof(userLogin));

            var userEntity = await _usersRepository.DeleteUserByLoginAsync(login);

            return _mapper.Map<UserResponse>(userEntity);
        }

        public async Task<List<UserResponse>> GetAllUsersAsync(string userLogin)
        {
            if (userLogin == null) throw new ArgumentNullException(nameof(userLogin));

            return (await _usersRepository.GetAllUsersAsync())?
                                          .Select(userEntity => _mapper.Map<UserResponse>(userEntity))
                                          .ToList();
        }

        /// <summary>
        ///     RemoveUserByLoginAsync скрывает пользователя по логину
        /// </summary>
        /// <param name="login">Логин пользователя, которого нужно найти</param>
        /// <param name="userLogin">Логин авторизаванного пользователя, который запрашивает поиск</param>
        /// <returns></returns>
        public async Task<UserResponse> RemoveUserByLoginAsync(string login, string userLogin)
        {
            if (login == null) throw new ArgumentNullException(nameof(login));
            if (userLogin == null) throw new ArgumentNullException(nameof(userLogin));

            var userEntity = await _usersRepository.RemoveUserByLoginAsync(login);

            return _mapper.Map<UserResponse>(userEntity);
        }
    }
}
