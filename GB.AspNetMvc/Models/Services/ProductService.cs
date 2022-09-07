﻿using GB.AspNetMvc.Models.DTO;
using GB.AspNetMvc.Models.Repository.Interfaces;
using GB.AspNetMvc.Models.Services.Interfaces;

namespace GB.AspNetMvc.Models.Services
{
    public class ProductService : IProductService
    {
        private readonly ICatalogRepository _catalogRepository;
        private readonly IMediator _mediator;
        private readonly ILogger<ProductService> _logger;

        public ProductService(ICatalogRepository catalogRepository, IMediator mediator, ILogger<ProductService> logger)
        {
            _catalogRepository = catalogRepository;
            _mediator = mediator;
            _logger = logger;
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

        public async Task AddProduct(ProductDto productDto, CancellationToken cancellationToken)
        {
            try
            {
                if (cancellationToken.IsCancellationRequested) cancellationToken.ThrowIfCancellationRequested();

                var product = new Product
                {
                    Id = Guid.NewGuid(),
                    Name = productDto.Name,
                    Category = productDto.Category,
                };
                var isAdded = _catalogRepository.AddProduct(product);

                await _mediator.Publish(product, isAdded, cancellationToken);
            }
            catch (AggregateException ae)
            {
                foreach (var e in ae.InnerExceptions)
                {
                    if(e is TaskCanceledException) _logger.LogWarning("Добавление товара отменено!", e.Message);
                }
            }
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
