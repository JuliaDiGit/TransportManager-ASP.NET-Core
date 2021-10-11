using System;
using System.Threading.Tasks;
using Authorization.Abstract;
using Authorization.DTO;
using Microsoft.Extensions.Logging;
using Models.Authorization;

namespace Authorization.AuthorizationServiceDecorators
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
                if (userRegistrationRequestModel == null)
                    throw new ArgumentNullException(nameof(userRegistrationRequestModel));

                var userAuthenticateResponse = await _inner.Register(userRegistrationRequestModel);

                var status = userAuthenticateResponse == null
                             ? Resources.Status_Fail
                             : Resources.Status_Success;

                _logger.LogInformation($"{userRegistrationRequestModel.Login} - " +
                                       $"{Resources.Operation_Register} - " +
                                       $"{status}");

                return userAuthenticateResponse;
            }
            catch (Exception e)
            {
                _logger.LogError($"{Resources.Operation_Register} - " +
                                 $"{e.GetType()} - " + 
                                 $"{e.Message}");

                throw;
            }
        }

        public async Task<UserAuthenticateResponse> Login(UserAuthenticateRequestModel userAuthenticateRequestModel)
        {
            try
            {
                if (userAuthenticateRequestModel == null)
                    throw new ArgumentNullException(nameof(userAuthenticateRequestModel));

                var userAuthenticateResponse = await _inner.Login(userAuthenticateRequestModel);

                var status = userAuthenticateResponse == null
                             ? Resources.Status_Fail
                             : Resources.Status_Success;

                _logger.LogInformation($"{userAuthenticateRequestModel.Login} - " +
                                       $"{Resources.Operation_Login} - " +
                                       $"{status}");

                return userAuthenticateResponse;
            }
            catch (Exception e)
            {
                var log = userAuthenticateRequestModel == null
                          ? $"{Resources.Operation_Login} - {e.GetType()} - {e.Message}"
                          : $"{userAuthenticateRequestModel.Login} - {Resources.Operation_Login} - {e.GetType()} - {e.Message}";

                _logger.LogError(log);

                throw;
            }
        }
    }
}
