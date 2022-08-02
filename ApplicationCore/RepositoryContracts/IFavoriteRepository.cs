using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.RepositoryContracts
{
    public interface IFavoriteRepository
    {
        Task<List<Favorite>> GetAll(int userId);
        Task<Favorite> Add(Favorite favorite);
        Task<Favorite> Delete(Favorite favorite);
        Task<Favorite> GetFavoriteById(int UserId, int MovieId);
    }
}
