using E_Commerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Contracts
{
    public interface IUnitOfWork
    {
        IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>()where TEntity:BaseEntity<TKey>;
        Task<int> SaveChangesAsync();
    }
}
