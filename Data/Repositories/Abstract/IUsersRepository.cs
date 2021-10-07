using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Entities;

namespace Data.Repositories.Abstract
{
    public interface IUsersRepository
    {
        Task<UserEntity> AddUserAsync(UserRequest userRequest);
        Task<UserEntity> UpdateUserAsync(UserRequest userRequest);
        Task<UserEntity> DeleteUserAsync(int id);
        Task<UserEntity> GetUserAsync(int id);
        Task<List<UserEntity>> GetAllUsersAsync();
        Task<UserEntity> GetUserByLoginAsync(string login);
        Task<UserEntity> RemoveUserAsync(int id);
    }
}
