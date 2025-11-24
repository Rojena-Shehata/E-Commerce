using E_Commerce.ServicesAbstraction;
using E_Commerce.Shared;
using E_Commerce.Shared.DTOs.ProductDTOs;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Presentation.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IproductService _productService;

        public ProductsController(IproductService productService)
        {
            _productService = productService;
        }
        [HttpGet]
        public async Task<ActionResult<PaginatedResult<ProductDTO>>> GetAllProducts([FromQuery]ProductQueryParameters parameters)
        {
            var products=await _productService.GetAllProductsAsync(parameters);
            return Ok(products);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProductById(int id)
        {
            var product=await _productService.GetProductByIdAsync(id);
            return Ok(product);
        }
        //Get:baseURL/api/products/types
        [HttpGet("types")]
        public async Task<ActionResult<IEnumerable<TypeDTO>>> GetAllTypes()
        {
            var types=await _productService.GetAllTypesAsync();
            return Ok(types);
        }
        //Get:baseURL/api/products/types
        [HttpGet("brands")]
        public async Task<ActionResult< IEnumerable<BrandDTO>>> GetAllBrands()
        {
            var brands=await _productService.GetAllBrandsAsync();
            return Ok(brands);
        }
    }
}
