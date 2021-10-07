using System.Threading.Tasks;
using Domain.Authorization;
using Models.Authorization;

namespace Services.Abstract
{
    public interface IAuthorizationService
    {
        Task<UserAuthenticateResponse> Register(UserRegistrationRequestModel userRegistrationRequestModel);
        Task<UserAuthenticateResponse> Login(UserAuthenticateRequestModel userAuthenticateRequestModel);
    }
}
