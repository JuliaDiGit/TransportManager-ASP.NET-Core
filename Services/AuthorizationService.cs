using System;
using System.Threading.Tasks;
using AutoMapper;
using Data.Repositories.Abstract;
using Domain;
using Domain.Authorization;
using Enums;
using Microsoft.Extensions.Configuration;
using Models.Authorization;
using Services.Abstract;
using Services.Authenticate;
using Services.Helpers;

namespace Services
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

            var userEntity = await _usersRepository.GetUserByLoginAsync(userRegistrationRequestModel.Login);
            if(userEntity != null) throw new Exception("Login already use");

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
                throw new Exception("Incorrect username or password");
            }

            var userAuthenticateResponse = _mapper.Map<UserAuthenticateResponse>(userEntity);

            // генерируем токен
            userAuthenticateResponse.Token = _configuration.GenerateUserJwtToken(userAuthenticateResponse);

            return userAuthenticateResponse;
        }
    }
}
