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
        Task<Purchase> GetById(int id);
        Task<List<Purchase>> GetAll();
        Task<Purchase> Add(Purchase purchase);
        Task<Purchase> Update(Purchase purchase);
        Task<Purchase> Delete(Purchase purchase);
    }
}
