using System.Collections.Generic;
using System.Threading.Tasks;
using TransportManager.Domain;
using TransportManager.Entities;

namespace TransportManager.Data.Repositories.Abstract
{
    public interface IUsersRepository
    {
        Task<UserEntity> GetUserByLoginAsync(string login);
        Task<UserEntity> AddUserAsync(UserRequest userRequest);
        Task<UserEntity> UpdateUserAsync(UserRequest userRequest);
        Task<UserEntity> DeleteUserByLoginAsync(string login);
        Task<List<UserEntity>> GetAllUsersAsync();
        Task<UserEntity> RemoveUserByLoginAsync(string login);
    }
}
