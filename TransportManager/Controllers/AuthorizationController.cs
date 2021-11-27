using System;
using System.Threading.Tasks;
using TransportManager.Authorization.Abstract;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TransportManager.Models.Authorization;

namespace TransportManager.Controllers
{
    /// <summary>
    ///     Контроллер авторизации
    /// </summary>
    [ApiController]
    [Route("auth")]
    public class AuthorizationController : ControllerBase
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IMapper _mapper;

        public AuthorizationController(IAuthorizationService authorizationService, IMapper mapper)
        {
            _authorizationService = authorizationService ?? throw new ArgumentNullException(nameof(authorizationService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        ///     Зарегистрироваться
        /// </summary>
        /// <param name="userRegistrationRequestModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(UserRegistrationRequestModel userRegistrationRequestModel)
        {
            if (userRegistrationRequestModel == null)
                throw new ArgumentNullException(nameof(userRegistrationRequestModel));

            var userResponse = await _authorizationService.Register(userRegistrationRequestModel);

            if (userResponse == null) return NoContent();

            var userResponseModel = _mapper.Map<UserAuthenticateResponseModel>(userResponse);

            return Ok(userResponseModel);
        }

        /// <summary>
        ///     Авторизоваться
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(UserAuthenticateRequestModel userAuthenticateRequestModel)
        {
            if (userAuthenticateRequestModel == null)
                throw new ArgumentNullException(nameof(userAuthenticateRequestModel));

            var userResponse = await _authorizationService.Login(userAuthenticateRequestModel);

            if (userResponse == null) return Ok("Incorrect username or password");

            var userResponseModel = _mapper.Map<UserAuthenticateResponseModel>(userResponse);

            return Ok(userResponseModel);
        }
    }
}
