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
    public class FavoriteRepository: IFavoriteRepository
    {
        private readonly MovieShopDbContext _movieShopDbContext;
        public FavoriteRepository(MovieShopDbContext movieShopDbContext)
        {
            _movieShopDbContext = movieShopDbContext;
        }

        public async Task<Favorite> Add(Favorite favorite)
        {
            _movieShopDbContext.Favorites.Add(favorite);
            await _movieShopDbContext.SaveChangesAsync();
            return favorite;
        }

        public async Task<Favorite> Delete(Favorite favorite)
        {
            _movieShopDbContext.Favorites.Remove(favorite);
            await _movieShopDbContext.SaveChangesAsync();
            return favorite;
        }

        public async Task<List<Favorite>> GetAll(int userId)
        {
            return await _movieShopDbContext.Favorites.
                Where(f=>f.UserId==userId).Include(p=>p.Movie).ToListAsync();
        }

        public async Task<Favorite> GetFavoriteById(int userId, int movieId)
		{
            var fav = await _movieShopDbContext.Favorites.FirstOrDefaultAsync(f=>f.UserId == userId && f.MovieId==movieId);
            return fav;
		}
    }
}
