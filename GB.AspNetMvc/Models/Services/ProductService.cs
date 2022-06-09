using GB.AspNetMvc.Models.DTO;
using GB.AspNetMvc.Models.Repository.Interfaces;
using GB.AspNetMvc.Models.Services.Interfaces;

namespace GB.AspNetMvc.Models.Services
{
    public class ProductService : IProductService
    {
        private readonly ICatalogRepository _catalogRepository;
        private readonly object _locker = new ();

        public ProductService(ICatalogRepository catalogRepository)
        {
            _catalogRepository = catalogRepository;
        }

        public List<ProductDto> GetProducts()
        {
            lock (_locker)
            {
                var products = _catalogRepository.GetAllProducts();
                return products.Select(product =>
                    new ProductDto
                    {
                        Name = product.Name,
                        Category = product.Category
                    }).ToList(); 
            }
        }

        public void AddProduct(ProductDto productDto)
        {
            lock (_locker)
            {
                var product = new Product
                {
                    Id = Guid.NewGuid(),
                    Name = productDto.Name,
                    Category = productDto.Category,
                };
                _catalogRepository.AddProduct(product); 
            }
        }

        public void DeleteProduct(Guid id)
        {
            lock (_locker)
            {
                _catalogRepository.DeleteProduct(id); 
            }
        }
    }
}
