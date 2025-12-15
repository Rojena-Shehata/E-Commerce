using E_Commerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Contracts
{
    //specification pattern
    public interface ISpecifications<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        public Expression<Func<TEntity,bool>> Criteria { get;  }
        public ICollection<Expression<Func<TEntity,object>>> IncludeExpressions { get;}
        public Expression<Func<TEntity,object>> OrderBy { get;}
        public Expression<Func<TEntity,object>> OrderByDesc { get;}
        public int Skip {  get; }
        public int Take {  get; }
        public bool IsPaginated {  get; }

    }
}
