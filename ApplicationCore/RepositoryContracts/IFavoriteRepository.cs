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
        Task<Favorite> GetById(int id);
        Task<List<Favorite>> GetAll();
        Task<Favorite> Add(Favorite favorite);
        Task<Favorite> Update(Favorite favorite);
        Task<Favorite> Delete(Favorite favorite);
    }
}
