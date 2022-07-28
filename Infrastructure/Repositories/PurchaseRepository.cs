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

        public async Task<List<Purchase>> GetAll()
        {
            return await _movieShopDbContext.Purchases.ToListAsync();
        }

        public Task<Purchase> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Purchase> Update(Review review)
        {
            throw new NotImplementedException();
        }
    }
}
