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

        public async Task<Review> Delete(Review review)
        {
            _movieShopDbContext.Reviews.Remove(review);
            await _movieShopDbContext.SaveChangesAsync();
            return review;
        }

        public async Task<List<Review>> GetAll()
        {
            return await _movieShopDbContext.Reviews.ToListAsync();
        }

        public Task<Review> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Review> Update(Review review)
        {
            throw new NotImplementedException();
        }
    }
}
