using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Presistence
{
    internal static class SpecificationsEvaluator
    {
        //Create Query - Build Query
        public static IQueryable<TEntity> CreateQuery<TEntity,TKey>(this IQueryable<TEntity> entryQuery, ISpecifications<TEntity,TKey> specifications) where TEntity : BaseEntity<TKey>
        {
            var query = entryQuery;
            if(specifications is not null)
            {
                if(specifications.Criteria is not null)
                    query=query.Where(specifications.Criteria);

                if(specifications.IncludeExpressions is not null && specifications.IncludeExpressions.Any())
                {
                    //foreach (var includeExp in specifications.IncludeExpressions)
                    //    query=query.Include(includeExp);

                    query = specifications.IncludeExpressions.Aggregate(query, (currentQuery, includeExp) => currentQuery.Include(includeExp));
                    
                }
                if(specifications.OrderBy is not null)
                {
                    query=query.OrderBy(specifications.OrderBy);
                }
                if(specifications.OrderByDesc is not null)
                {
                    query=query.OrderByDescending(specifications.OrderByDesc);
                }
                if (specifications.IsPaginated)
                {
                    query=query.Skip(specifications.Skip).Take(specifications.Take);
                }
            }


            return query;
        }

    }
}       
