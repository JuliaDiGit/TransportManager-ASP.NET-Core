using System;
using System.Threading.Tasks;
using AutoMapper;
using TransportManager.Data.Repositories.Abstract;
using Microsoft.Extensions.Configuration;
using TransportManager.Authorization.Abstract;
using TransportManager.Authorization.DTO;
using TransportManager.Authorization.Helpers;
using TransportManager.Common.Enums;
using TransportManager.Domain;
using TransportManager.Models.Authorization;

namespace TransportManager.Authorization
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public AuthorizationService(IConfiguration configuration, IUsersRepository usersRepository, IMapper mapper)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _usersRepository = usersRepository ?? throw new ArgumentNullException(nameof(usersRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<UserAuthenticateResponse> Register(UserRegistrationRequestModel userRegistrationRequestModel)
        {
            if (userRegistrationRequestModel == null)
                throw new ArgumentNullException(nameof(userRegistrationRequestModel));

            var userRegistrationRequest = _mapper.Map<UserRegistrationRequest>(userRegistrationRequestModel);

            //при регистрации присваиваем рандомную роль
            Role[] accesses = (Role[])Enum.GetValues(typeof(Role));
            userRegistrationRequest.Role = accesses[new Random().Next(0, accesses.Length)];

            // хешируем пароль
            userRegistrationRequest.Password = Hash.HashString(userRegistrationRequest.Password);

            var user = _mapper.Map<UserRequest>(userRegistrationRequest);

            var addedUserEntity = await _usersRepository.AddUserAsync(user);
            var addedUser = _mapper.Map<UserAuthenticateResponse>(addedUserEntity);

            // генерируем токен
            addedUser.Token = _configuration.GenerateUserJwtToken(addedUser);

            return addedUser;
        }

        public async Task<UserAuthenticateResponse> Login(UserAuthenticateRequestModel userAuthenticateRequestModel)
        {
            if (userAuthenticateRequestModel == null)
                throw new ArgumentNullException(nameof(userAuthenticateRequestModel));

            var userEntity = await _usersRepository.GetUserByLoginAsync(userAuthenticateRequestModel.Login);

            // для проверки совпадения паролей используем специальный метод, т.к в БД пароль хранится хешированным
            if (userEntity == null || !Hash.VerifyHashedString(userEntity.Password, userAuthenticateRequestModel.Password))
            {
                return null;
            }

            var userAuthenticateResponse = _mapper.Map<UserAuthenticateResponse>(userEntity);

            // генерируем токен
            userAuthenticateResponse.Token = _configuration.GenerateUserJwtToken(userAuthenticateResponse);

            return userAuthenticateResponse;
        }
    }
}
