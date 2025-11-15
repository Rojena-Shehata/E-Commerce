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
    public class GenericRepository<TEntity, TKey>(StoreDbContext _dbContext) : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {

        
        private readonly DbSet<TEntity> _dbSet=_dbContext.Set<TEntity>();
        public async Task<IEnumerable<TEntity>> GetAllAsync() 
            =>  await _dbContext.Set<TEntity>().AsNoTracking().ToListAsync();
        

        public async Task<TEntity?> GetByIdAsync(TKey id) 
            => await _dbContext.Set<TEntity>().FindAsync(id);



        public void Add(TEntity entity)        
           => _dbContext.Set<TEntity>().Add(entity);
        

        public void Delete(TEntity entity)
          =>  _dbContext.Set<TEntity>().Remove(entity);
        

      
        public void Update(TEntity entity)
            =>_dbContext.Set<TEntity>().Update(entity);

        public async Task<IEnumerable<TEntity>> GetAllAsync(ISpecifications<TEntity, TKey> specifications)
        {
            return await _dbSet.CreateQuery<TEntity,TKey>(specifications).ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(ISpecifications<TEntity, TKey> specifications)
        {
            return await _dbSet.CreateQuery(specifications).FirstOrDefaultAsync();
        }
    }
}
