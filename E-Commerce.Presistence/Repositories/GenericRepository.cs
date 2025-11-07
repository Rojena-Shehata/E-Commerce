using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities;
using E_Commerce.Presistence.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Presistence.Repositories
{
    public class GenericRepository<TEntity, Tkey> : IGenericRepository<TEntity, Tkey> where TEntity : BaseEntity<Tkey>
    {
        private readonly StoreDbContext _dbContext;

        public GenericRepository(StoreDbContext  dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync() 
            =>  await _dbContext.Set<TEntity>().AsNoTracking().ToListAsync();
        

        public async Task<TEntity?> GetByIdAsync(Tkey id) 
            => await _dbContext.Set<TEntity>().FindAsync(id);



        public void Add(TEntity entity)        
           => _dbContext.Set<TEntity>().Add(entity);
        

        public void Delete(TEntity entity)
          =>  _dbContext.Set<TEntity>().Remove(entity);
        

      
        public void Update(TEntity entity)
            =>_dbContext.Set<TEntity>().Update(entity);
     
    }
}
