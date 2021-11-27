using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TransportManager.Domain;
using TransportManager.Entities;
using Microsoft.EntityFrameworkCore;
using TransportManager.Common.Exceptions;
using TransportManager.Data.Repositories.Abstract;

namespace TransportManager.Data.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UsersRepository(DataContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<UserEntity> GetUserByLoginAsync(string login)
        {
            if (login == null) throw new ArgumentNullException(nameof(login));

            return await _context.Users.Where(user => user.Login == login && !user.IsDeleted)
                                       .AsNoTracking()
                                       .FirstOrDefaultAsync();
        }

        public async Task<UserEntity> AddUserAsync(UserRequest userRequest)
        {
            if (userRequest == null) throw new ArgumentNullException(nameof(userRequest));

            var foundUser = await _context.Users.Where(user => user.Login == userRequest.Login)
                                                .AsNoTracking()
                                                .FirstOrDefaultAsync();

            if(foundUser != null) throw new UserErrorException(Resources.Error_ImpossibleAddUser +
                                                               Resources.Error_LoginExists);

            var userEntity = _mapper.Map<UserEntity>(userRequest);

            var addedUser = await _context.Users.AddAsync(userEntity);

            await _context.SaveChangesAsync();

            return addedUser.Entity;
        }

        public async Task<UserEntity> UpdateUserAsync(UserRequest userRequest)
        {
            if (userRequest == null) throw new ArgumentNullException(nameof(userRequest));

            var foundUser = await _context.Users.Where(user => user.Login == userRequest.Login)
                                                .AsNoTracking()
                                                .FirstOrDefaultAsync();

            if (foundUser == null) throw new UserErrorException(Resources.Error_ImpossibleUpdateUser +
                                                                Resources.Error_UserNotFound);

            var userEntity = _mapper.Map<UserEntity>(userRequest);

            // оставляем уже имеющийся Id
            userEntity.Id = foundUser.Id;

            // оставлем изнчальную дату создания
            userEntity.CreatedDate = foundUser.CreatedDate;

            var updUser = _context.Users.Update(userEntity);

            await _context.SaveChangesAsync();

            return updUser.Entity;
        }

        public async Task<UserEntity> DeleteUserByLoginAsync(string login)
        {
            if (login == null) throw new ArgumentNullException(nameof(login));

            var userEntity = await _context.Users.Where(user => user.Login == login)
                                                 .AsNoTracking()
                                                 .FirstOrDefaultAsync();

            if (userEntity == null) throw new UserErrorException(Resources.Error_ImpossibleDeleteUser +
                                                                 Resources.Error_UserNotFound);

            _context.Users.Remove(userEntity);

            await _context.SaveChangesAsync();

            return userEntity;
        }

        public async Task<List<UserEntity>> GetAllUsersAsync()
        {
            return await _context.Users.Where(user => !user.IsDeleted)
                                       .AsNoTracking()
                                       .ToListAsync();
            
        }

        public async Task<UserEntity> RemoveUserByLoginAsync(string login)
        {
            if (login == null) throw new ArgumentNullException(nameof(login));

            var userEntity = await _context.Users.Where(user => user.Login == login)
                                                 .AsNoTracking()
                                                 .FirstOrDefaultAsync();

            if (userEntity == null) throw new UserErrorException(Resources.Error_ImpossibleRemoveUser +
                                                                 Resources.Error_UserNotFound);

            userEntity.IsDeleted = true;
            userEntity.SoftDeletedDate = DateTime.Now;

            var updUser = _context.Users.Update(userEntity);

            await _context.SaveChangesAsync();

            return updUser.Entity;
        }
    }
}
