using E_Commerce.Presentation.Attributes;
using E_Commerce.ServicesAbstraction;
using E_Commerce.Shared;
using E_Commerce.Shared.DTOs.ProductDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Presentation.Controllers
{
    public class ProductsController : ApiBaseController
    {
        private readonly IServiceManager _serviceManager;

        public ProductsController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }
        [HttpGet]
        [RedisCache]
        public async Task<ActionResult<PaginatedResult<ProductDTO>>> GetAllProducts([FromQuery]ProductQueryParameters parameters)
        {
            var products=await _serviceManager.ProductService.GetAllProductsAsync(parameters);
            return Ok(products);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProductById(int id)
        {
            var product=await _serviceManager.ProductService.GetProductByIdAsync(id);
            return HandleResult<ProductDTO>(product);
        }
        //Get:baseURL/api/products/types
        [HttpGet("types")]
        public async Task<ActionResult<IEnumerable<TypeDTO>>> GetAllTypes()
        {
            var types=await _serviceManager.ProductService.GetAllTypesAsync();
            return Ok(types);
        }
        //Get:baseURL/api/products/types
        [HttpGet("brands")]
        public async Task<ActionResult< IEnumerable<BrandDTO>>> GetAllBrands()
        {
            var brands=await _serviceManager.ProductService.GetAllBrandsAsync();
            return Ok(brands);
        }
    }
}
