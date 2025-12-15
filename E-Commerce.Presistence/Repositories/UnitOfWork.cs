using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities;
using E_Commerce.Presistence.Data.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Presistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDbContext _dbContext;
        private readonly Dictionary<string, object> _repositories = [];

        public UnitOfWork(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
            var entityType= typeof(TEntity).Name;
            if (_repositories.TryGetValue(entityType, out object? repository))
                return (IGenericRepository<TEntity,TKey>)repository;

            var newRepository = new GenericRepository<TEntity, TKey>(_dbContext);
            _repositories.Add(entityType, newRepository);
            return newRepository;


        }

        public async Task<int> SaveChangesAsync()
            =>await _dbContext.SaveChangesAsync();
        
        
    }
}
