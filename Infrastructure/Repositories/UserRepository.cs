using ApplicationCore.Entities;
using ApplicationCore.RepositoryContracts;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MovieShopDbContext _movieShopDbContext;

        public UserRepository(MovieShopDbContext movieShopDbContext)
        {
            _movieShopDbContext = movieShopDbContext;
        }

        public async Task<User> AddUser(User user)
        {
            _movieShopDbContext.Users.Add(user);
            await _movieShopDbContext.SaveChangesAsync(); // need to call savechangesAsync to actually save to database
            return user;
        }

        public async Task<User> GetById(int id)
        {
            var user = await _movieShopDbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            // this method will be used in accountService to check if there's matching user
            var user = await _movieShopDbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            return user;
        }
    }
}
