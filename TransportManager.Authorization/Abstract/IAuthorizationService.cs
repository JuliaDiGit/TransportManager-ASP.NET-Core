using System.Threading.Tasks;
using TransportManager.Authorization.DTO;
using TransportManager.Models.Authorization;

namespace TransportManager.Authorization.Abstract
{
    public interface IAuthorizationService
    {
        Task<UserAuthenticateResponse> Register(UserRegistrationRequestModel userRegistrationRequestModel);
        Task<UserAuthenticateResponse> Login(UserAuthenticateRequestModel userAuthenticateRequestModel);
    }
}
