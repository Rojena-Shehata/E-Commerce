using E_Commerce.Domain.Entities.ProductModule;
using E_Commerce.Shared.DTOs.ProductDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.Specifications.ProductSpecifications
{
    internal static class ProductSpecificationHelper
    {

        internal static Expression<Func<Product, bool>> CreateCriteria(ProductQueryParameters parameters)
        {
            return product => (!parameters.BrandId.HasValue || product.BrandId == parameters.BrandId.Value)
                        && (!parameters.TypeId.HasValue || product.TypeId == parameters.TypeId.Value)
                        && (string.IsNullOrEmpty(parameters.Search) || product.Name.ToLower().Contains(parameters.Search.ToLower()));

        }
    }
}
