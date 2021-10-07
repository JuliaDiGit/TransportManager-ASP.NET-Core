using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Models;

namespace Services.Abstract
{
    public interface IUsersService
    {
        Task<UserResponse> AddUserAsync(UserRequestModel userRequestModel, string userLogin);
        Task<UserResponse> UpdateUserAsync(UserRequestModel userRequestModel, string userLogin);
        Task<UserResponse> DeleteUserByIdAsync(int id, string userLogin);
        Task<UserResponse> GetUserByLoginAsync(string login, string userLogin);
        Task<List<UserResponse>> GetAllUsersAsync(string userLogin);
        Task<UserResponse> RemoveUserByIdAsync(int id, string userLogin);
    }
}
