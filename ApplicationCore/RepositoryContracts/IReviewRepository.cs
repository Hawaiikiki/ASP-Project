using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.RepositoryContracts
{
    public interface IReviewRepository
    {
        Task<Review> GetById(int userId, int movieId);
        Task<List<Review>> GetAll(int userId);
        Task<Review> Add(Review review);
        Task<Review> Update(Review review);
        Task<Review> Delete(int userId, int movieId);
    }
}
