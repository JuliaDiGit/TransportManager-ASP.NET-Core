using System.Threading.Tasks;
using Authorization.DTO;
using Models.Authorization;

namespace Authorization.Abstract
{
    public interface IAuthorizationService
    {
        Task<UserAuthenticateResponse> Register(UserRegistrationRequestModel userRegistrationRequestModel);
        Task<UserAuthenticateResponse> Login(UserAuthenticateRequestModel userAuthenticateRequestModel);
    }
}
