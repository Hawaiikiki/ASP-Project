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
    public class PurchaseRepository: IPurchaseRepository
    {
        private readonly MovieShopDbContext _movieShopDbContext;
        public PurchaseRepository(MovieShopDbContext movieShopDbContext)
        {
            _movieShopDbContext = movieShopDbContext;
        }

        public async Task<Purchase> Add(Purchase purchase)
        {
            _movieShopDbContext.Purchases.Add(purchase);
            await _movieShopDbContext.SaveChangesAsync();
            return purchase;
        }

        public async Task<Purchase> Delete(Purchase purchase)
        {
            _movieShopDbContext.Purchases.Remove(purchase);
            await _movieShopDbContext.SaveChangesAsync();
            return purchase;
        }

        public async Task<List<Purchase>> GetAll(int userId)
        {
            var purchases = await _movieShopDbContext.Purchases
                .Where(p => p.UserId == userId)
                .Include(p => p.Movie).ToListAsync();
            return purchases;
        }

        public async Task<Purchase> GetUserMovie(int userId, int movieId)
        {
            var purchase = await _movieShopDbContext.Purchases
            .Include(p => p.Movie)
            .FirstOrDefaultAsync(p => p.UserId == userId && p.MovieId == movieId);
            return purchase;
        }
    }
}
