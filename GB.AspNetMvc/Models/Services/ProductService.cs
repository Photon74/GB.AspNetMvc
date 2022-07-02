using GB.AspNetMvc.Models.DTO;
using GB.AspNetMvc.Models.Repository.Interfaces;
using GB.AspNetMvc.Models.Services.Interfaces;

namespace GB.AspNetMvc.Models.Services
{
    public class ProductService : IProductService
    {
        private readonly ICatalogRepository _catalogRepository;
        private readonly IMailSenderService _mailSenderService;
        private readonly IMediator _mediator;

        public ProductService(ICatalogRepository catalogRepository, IMailSenderService mailSenderService, IMediator mediator)
        {
            _catalogRepository = catalogRepository;
            _mailSenderService = mailSenderService;
            _mediator = mediator;
        }

        public List<ProductDto> GetProducts()
        {
            var products = _catalogRepository.GetAllProducts();
            return products == null! || products.Count == 0
                ? new List<ProductDto>()
                : products.Select(product =>
                    new ProductDto
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Category = product.Category
                    }).ToList();
        }

        public void AddProduct(ProductDto productDto)
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = productDto.Name,
                Category = productDto.Category,
            };
            var isAdded = _catalogRepository.AddProduct(product);

            _mediator.Publish(product, isAdded);

            _mailSenderService.SendMail(product);
        }

        public void DeleteProduct(Guid id)
        {
            _catalogRepository.DeleteProduct(id);
        }

        public ProductDto? GetProductById(Guid id)
        {
            var product = _catalogRepository.GetProductById(id);
            return product == null
                ? null
                : new ProductDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Category = product.Category,
                };
        }

        public void EditProduct(ProductDto productDto)
        {
            var product = new Product
            {
                Id = productDto.Id,
                Name = productDto.Name,
                Category = productDto.Category,
            };
            _catalogRepository.UpdateProduct(product);
        }
    }
}
