using ApplicationCore.Entities;
using ApplicationCore.RepositoryContracts;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CastRepository : ICastRepository
    {
        private readonly MovieShopDbContext _movieShopDbContext;
        public CastRepository(MovieShopDbContext dbContext)
        {
            _movieShopDbContext = dbContext;
        }

        public async Task<Cast> GetById(int id)
        {
            var castDetails = await _movieShopDbContext.Casts
                .Include(c=>c.MoviesOfCast).ThenInclude(c=>c.Movie)
                .FirstOrDefaultAsync(c => c.Id == id);
            return castDetails;
        }
    }
}
