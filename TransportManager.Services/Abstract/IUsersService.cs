using System.Collections.Generic;
using System.Threading.Tasks;
using TransportManager.Domain;
using TransportManager.Models;

namespace TransportManager.Services.Abstract
{
    public interface IUsersService
    {
        Task<UserResponse> GetUserByLoginAsync(string login, string userLogin);
        Task<UserResponse> AddUserAsync(UserRequestModel userRequestModel, string userLogin);
        Task<UserResponse> UpdateUserAsync(UserRequestModel userRequestModel, string userLogin);
        Task<UserResponse> DeleteUserByLoginAsync(string login, string userLogin);
        Task<List<UserResponse>> GetAllUsersAsync(string userLogin);
        Task<UserResponse> RemoveUserByLoginAsync(string login, string userLogin);
    }
}
