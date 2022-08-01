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
    public class ReviewRepository: IReviewRepository
    {
        private readonly MovieShopDbContext _movieShopDbContext;
        public ReviewRepository(MovieShopDbContext movieShopDbContext)
        {
            _movieShopDbContext = movieShopDbContext;
        }

        public async Task<Review> Add(Review review)
        {
            _movieShopDbContext.Reviews.Add(review);
            await _movieShopDbContext.SaveChangesAsync();
            return review;
        }

        public async Task<Review> Delete(int userId, int movieId)
        {
            var review = await GetById(userId, movieId);
            _movieShopDbContext.Reviews.Remove(review);
            await _movieShopDbContext.SaveChangesAsync();
            return review;
        }

        public async Task<List<Review>> GetAll(int userId)
        {
            return await _movieShopDbContext.Reviews.Where(r=>r.UserId==userId).ToListAsync();
        }

        public async Task<Review> GetById(int userId, int movieId)
        {
            return await _movieShopDbContext.Reviews.FirstOrDefaultAsync(r => r.UserId == userId && r.MovieId == movieId);
        }

        public Task<Review> Update(Review review)
        {
            throw new NotImplementedException();
        }
    }
}
