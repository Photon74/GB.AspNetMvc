using GB.AspNetMvc.Models.Repository.Interfaces;
using System.Collections.Concurrent;

namespace GB.AspNetMvc.Models.Repository
{
    public class CatalogInMemory : ICatalogRepository
    {
        private readonly ILogger<CatalogInMemory> _logger;
        private ConcurrentDictionary<Guid, ProductEntity> Products { get; set; } = new();

        public CatalogInMemory(ILogger<CatalogInMemory> logger)
        {
            _logger = logger;
        }

        public IReadOnlyList<Product> GetAllProducts()
        {
            try
            {
                var res = Products.Select(product =>
                        new Product
                        {
                            Id = product.Key,
                            Name = product.Value.Name,
                            Category = product.Value.Category,
                        }).ToList();

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

        public Product GetProductById(Guid id)
        {
            var res = Products[id];
            var product = new Product
            {
                Id = id,
                Name = res.Name,
                Category = res.Category,
            };
            return product;
        }

        public void UpdateProduct(Product product)
        {
            try
            {
                var oldProduct = Products[product.Id];
                var productEntity = new ProductEntity
                {
                    Name = product.Name,
                    Category = product.Category,
                };
                Products.TryUpdate(product.Id, productEntity, oldProduct);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Не удалось обновить товар: {@product}", product);
                throw;
            }
        }
    }
}
