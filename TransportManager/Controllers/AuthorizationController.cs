using System;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Models.Authorization;
using Models.Validation;
using Services.Abstract;

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
            try
            {
                if (userRegistrationRequestModel == null)
                    throw new ArgumentNullException(nameof(userRegistrationRequestModel));

                ValidationFilter.Validate(userRegistrationRequestModel);

                var userResponse = await _authorizationService.Register(userRegistrationRequestModel);

                var userResponseModel = _mapper.Map<UserAuthenticateResponseModel>(userResponse);

                return Ok(userResponseModel);
            }
            catch (Exception e)
            {
                if (e is Npgsql.PostgresException) return BadRequest("Ошибка работы БД");
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        ///     Авторизоваться
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(UserAuthenticateRequestModel userAuthenticateRequestModel)
        {
            try
            {
                if (userAuthenticateRequestModel == null)
                    throw new ArgumentNullException(nameof(userAuthenticateRequestModel));

                var userResponse = await _authorizationService.Login(userAuthenticateRequestModel);

                var userResponseModel = _mapper.Map<UserAuthenticateResponseModel>(userResponse);

                return Ok(userResponseModel);
            }
            catch (Exception e)
            {
                if (e is Npgsql.PostgresException) return BadRequest("Ошибка работы БД");
                return BadRequest(e.Message);
            }
        }
    }
}
