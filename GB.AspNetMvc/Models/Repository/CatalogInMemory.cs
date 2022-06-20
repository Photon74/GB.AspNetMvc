using GB.AspNetMvc.Models.Repository.Interfaces;
using System.Collections.Concurrent;

namespace GB.AspNetMvc.Models.Repository
{
    public class CatalogInMemory : ICatalogRepository
    {
        private ConcurrentDictionary<Guid, ProductEntity>? Products { get; set; }

        public IReadOnlyList<Product> GetAllProducts()
        {
            return Products?.Select(product =>
                new Product
                {
                    Id = product.Key,
                    Name = product.Value.Name,
                    Category = product.Value.Category,
                }).ToList()!;
        }

        public void AddProduct(Product product)
        {
            Products ??= new ConcurrentDictionary<Guid, ProductEntity>();

            var productEntity = new ProductEntity
            {
                Name = product.Name,
                Category = product.Category,
            };
            Products.TryAdd(product.Id, productEntity);
        }

        public void DeleteProduct(Guid id)
        {
            if (Products != null) Products.TryRemove(id, out _);
        }
    }
}
