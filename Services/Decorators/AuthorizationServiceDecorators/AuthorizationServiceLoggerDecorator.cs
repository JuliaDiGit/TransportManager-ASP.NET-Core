using System;
using System.Threading.Tasks;
using Domain.Authorization;
using Microsoft.Extensions.Logging;
using Models.Authorization;
using Services.Abstract;

namespace Services.Decorators.AuthorizationServiceDecorators
{
    public class AuthorizationServiceLoggerDecorator : IAuthorizationService
    {
        private readonly ILogger<AuthorizationServiceLoggerDecorator> _logger;
        private readonly IAuthorizationService _inner;

        public AuthorizationServiceLoggerDecorator(ILogger<AuthorizationServiceLoggerDecorator> logger, IAuthorizationService inner)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _inner = inner ?? throw new ArgumentNullException(nameof(inner));
        }

        public async Task<UserAuthenticateResponse> Register(UserRegistrationRequestModel userRegistrationRequestModel)
        {
            try
            {
                var userAuthenticateResponse = await _inner.Register(userRegistrationRequestModel);

                string status = userAuthenticateResponse == null
                                ? Resources.Status_Fail
                                : Resources.Status_Success;

                _logger.LogInformation($"{userRegistrationRequestModel.Login} - " +
                                       $"{Resources.Operation_Register} - " +
                                       $"{status}");

                return userAuthenticateResponse;
            }
            catch (Exception e)
            {
                _logger.LogError($"{userRegistrationRequestModel.Login} - " +
                                 $"{Resources.Operation_Register} - " +
                                 $"{e.GetType()} - " + 
                                 $"{e.Message}");

                throw;
            }
        }

        public async Task<UserAuthenticateResponse> Login(UserAuthenticateRequestModel userAuthenticateRequestModel)
        {
            try
            {
                var userAuthenticateResponse = await _inner.Login(userAuthenticateRequestModel);

                string status = userAuthenticateResponse == null
                                ? Resources.Status_Fail
                                : Resources.Status_Success;

                _logger.LogInformation($"{userAuthenticateRequestModel.Login} - " +
                                       $"{Resources.Operation_Login} - " +
                                       $"{status}");

                return userAuthenticateResponse;
            }
            catch (Exception e)
            {
                _logger.LogError($"{userAuthenticateRequestModel.Login} - " +
                                 $"{Resources.Operation_Login} - " +
                                 $"{e.GetType()} - " +
                                 $"{e.Message}");

                throw;
            }
        }
    }
}
