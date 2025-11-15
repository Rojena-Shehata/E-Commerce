using E_Commerce.Domain.Entities.ProductModule;
using E_Commerce.Shared.DTOs.ProductDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.Specifications
{
    public class ProductWithBrandTypeSpecification : BaseSpecification<Product, int>
    {
        public ProductWithBrandTypeSpecification(int id) : base(x=>x.Id==id)
        {
            AddInclude(x => x.ProductBrand);
            AddInclude(x => x.ProductType);

        }
        public ProductWithBrandTypeSpecification(ProductQueryParameters parameters) : base(CreateCriteria(parameters))
        {
            AddInclude(x => x.ProductBrand);
            AddInclude(x => x.ProductType);

        }

        private static Expression<Func<Product,bool>> CreateCriteria(ProductQueryParameters parameters)
        {
            return x => (!parameters.BrandId.HasValue || x.BrandId == parameters.BrandId.Value) 
                        &&  (!parameters.TypeId.HasValue || x.TypeId == parameters.TypeId.Value);

        }
    }
}
