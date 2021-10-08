using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Data.Repositories.Abstract;
using Domain;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
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

        public async Task<UserEntity> GetUserAsync(int id)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));

            return await _context.Users.Where(user => user.Id == id && !user.IsDeleted)
                                       .FirstOrDefaultAsync();
            
        }

        public async Task<UserEntity> AddUserAsync(UserRequest userRequest)
        {
            if (userRequest == null) throw new ArgumentNullException(nameof(userRequest));

            var userEntity = _mapper.Map<UserEntity>(userRequest);

            var addedUser = await _context.Users.AddAsync(userEntity);

            await _context.SaveChangesAsync();

            return addedUser.Entity;
        }

        public async Task<UserEntity> UpdateUserAsync(UserRequest userRequest)
        {
            if (userRequest == null) throw new ArgumentNullException(nameof(userRequest));

            var userEntity = _mapper.Map<UserEntity>(userRequest);

            var updUser = _context.Users.Update(userEntity);

            await _context.SaveChangesAsync();

            return updUser.Entity;
        }

        public async Task<UserEntity> DeleteUserAsync(int id)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));

            var userEntity = await _context.Users.Where(user => user.Id == id)
                                                 .FirstOrDefaultAsync();

            if (userEntity == null) throw new NullReferenceException(nameof(UserEntity));

            _context.Users.Remove(userEntity);

            await _context.SaveChangesAsync();

            return userEntity;
        }

        public async Task<List<UserEntity>> GetAllUsersAsync()
        {
            return await _context.Users.Where(user => !user.IsDeleted)
                                       .ToListAsync();
            
        }

        public async Task<UserEntity> GetUserByLoginAsync(string login)
        {
            if (login == null) throw new ArgumentNullException(nameof(login));

            return await _context.Users.Where(user => user.Login == login && !user.IsDeleted)
                                       .FirstOrDefaultAsync();
        }

        public async Task<UserEntity> RemoveUserAsync(int id)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));

            var userEntity = await _context.Users.Where(user => user.Id == id)
                                                 .FirstOrDefaultAsync();

            if (userEntity == null) throw new ArgumentNullException(nameof(UserEntity));

            userEntity.IsDeleted = true;
            userEntity.SoftDeletedDate = DateTime.Now;

            var updUser = _context.Users.Update(userEntity);

            await _context.SaveChangesAsync();

            return updUser.Entity;
        }
    }
}
