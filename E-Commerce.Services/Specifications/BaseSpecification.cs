using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.Specifications
{
    public abstract class BaseSpecification<TEntity, TKey> : ISpecifications<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {

        protected BaseSpecification(Expression<Func<TEntity, bool>>? criteria)
        {
            Criteria = criteria;
        }
        public Expression<Func<TEntity, bool>> Criteria { get; private set; }

        public ICollection<Expression<Func<TEntity, object>>> IncludeExpressions { get; } = [];
      
        protected void AddInclude(Expression<Func<TEntity,object>> includeExp)
        {
            IncludeExpressions.Add(includeExp);
        }
        public Expression<Func<TEntity, object>> OrderBy { get; private set; }

        public Expression<Func<TEntity, object>> OrderByDesc { get; private set; }
        protected void AddOrderBy(Expression<Func<TEntity,object>> expression)
        {
            OrderBy=expression;
        }
        protected void AddOrderByDesc(Expression<Func<TEntity,object>> expression)
        {
            OrderByDesc=expression;
        }
    }
}
