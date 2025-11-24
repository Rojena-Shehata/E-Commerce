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
        public ProductWithBrandTypeSpecification(ProductQueryParameters parameters) : base(ProductSpecificationHelper.CreateCriteria(parameters))
        {
            AddInclude(x => x.ProductBrand);
            AddInclude(x => x.ProductType);

            switch (parameters.Sort)
            {
                case ProductSortingOptions.NameAsc:
                    AddOrderBy(product=>product.Name);
                    break;
                case ProductSortingOptions.NameDesc:
                    AddOrderByDesc(product=>product.Name);
                    break;
                case ProductSortingOptions.PriceAsc:
                    AddOrderBy(product=>product.Price);
                    break;
                case ProductSortingOptions.PriceDesc:
                    AddOrderByDesc(product=>product.Price);
                    break;

                default:
                    AddOrderBy(x=>x.Name);
                    break;
            }

            ApplyPagination(parameters.PageIndex, parameters.PageSize);

        }

    }
}
