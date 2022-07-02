using GB.AspNetMvc.Models.Repository.Interfaces;
using System.Collections.Concurrent;

namespace GB.AspNetMvc.Models.Repository
{
    public class CatalogInMemory : ICatalogRepository
    {
        private readonly ILogger<CatalogInMemory> _logger;
        private ConcurrentDictionary<Guid, ProductEntity>? Products { get; set; }

        public CatalogInMemory(ILogger<CatalogInMemory> logger)
        {
            _logger = logger;
        }

        public IReadOnlyList<Product> GetAllProducts()
        {
            try
            {
                var res = Products?.Select(product =>
                        new Product
                        {
                            Id = product.Key,
                            Name = product.Value.Name,
                            Category = product.Value.Category,
                        }).ToList()!;

                _logger.LogInformation("Получен список товаров");

                return res;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Не удалось получить список товаров!");
                throw;
            }
        }

        public bool AddProduct(Product product)
        {
            try
            {
                Products ??= new ConcurrentDictionary<Guid, ProductEntity>();

                var productEntity = new ProductEntity
                {
                    Name = product.Name,
                    Category = product.Category,
                };
                var res = Products.TryAdd(product.Id, productEntity);

                _logger.LogInformation("Добавлен новый товар {@product}", product);

                return res;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Не удалось добавить товар {@product}", product);
                throw;
            }
        }

        public void DeleteProduct(Guid id)
        {
            if (Products == null) return;
            try
            {
                if (Products.TryRemove(id, out _))
                {
                    _logger.LogInformation("Удаление товара с ID: {@id}", id);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Не удалось удалить товар с ID: {@id}", id);
                throw;
            }

        }

        public Product? GetProductById(Guid id)
        {
            if (Products == null) return null;

            var res = Products.FirstOrDefault(p => p.Key == id);
            var product = new Product
            {
                Id = res.Key,
                Name = res.Value.Name,
                Category = res.Value.Category,
            };
            return product;
        }

        public void UpdateProduct(Product product)
        {
            if (Products == null) return;
            try
            {
                var oldProduct = Products.FirstOrDefault(p => p.Key == product.Id);
                var productEntity = new ProductEntity
                {
                    Name = product.Name,
                    Category = product.Category,
                };
                Products.TryUpdate(product.Id, productEntity, oldProduct.Value);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Не удалось обновить товар: {@product}", product);
                throw;
            }
        }
    }
}
