using AutoMapper;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.ProductModule;
using E_Commerce.Services.Exceptions.NotFoundExceptions;
using E_Commerce.Services.Specifications;
using E_Commerce.ServicesAbstraction;
using E_Commerce.Shared;
using E_Commerce.Shared.DTOs.ProductDTOs;

namespace E_Commerce.Services
{
    public class ProductService : IproductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<BrandDTO>> GetAllBrandsAsync()
        {
            var brands = await _unitOfWork.GetRepository<ProductBrand, int>().GetAllAsync();
            return _mapper.Map<IEnumerable<BrandDTO>>(brands);
        }

        public async Task<PaginatedResult<ProductDTO>> GetAllProductsAsync(ProductQueryParameters parameters)
        {
            var productRepo = _unitOfWork.GetRepository<Product, int>();
            var specs = new ProductWithBrandTypeSpecification(parameters);
            var products = await productRepo.GetAllAsync(specs);
            var dataToReturn= _mapper.Map<IEnumerable<ProductDTO>>(products);
            var countSpec = new ProductCountSpecification(parameters);
            var TotalCount = await productRepo.CountAsync(countSpec);
            return new PaginatedResult<ProductDTO>(parameters.PageIndex, dataToReturn.Count(), TotalCount, dataToReturn);
        }

        public async Task<IEnumerable<TypeDTO>> GetAllTypesAsync()
        {

            var types = await _unitOfWork.GetRepository<ProductType, int>().GetAllAsync();
            return _mapper.Map<IEnumerable<TypeDTO>>(types);
        }

        public async Task<ProductDTO> GetProductByIdAsync(int id)
        {
            var specs=new ProductWithBrandTypeSpecification(id);
            var product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(specs);
            if(product is  null)
                throw new ProductNotFoundException(id);

            return _mapper.Map<ProductDTO>(product);
        }
    }
}
