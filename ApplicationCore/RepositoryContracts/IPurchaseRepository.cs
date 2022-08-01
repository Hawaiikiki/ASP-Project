using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.RepositoryContracts
{
    public interface IPurchaseRepository
    {
        Task<List<Purchase>> GetAll(int userId);
        Task<Purchase> Add(Purchase purchase);
        Task<Purchase> GetUserMovie(int userId, int movieId);
        Task<Purchase> Delete(Purchase purchase);
    }
}
