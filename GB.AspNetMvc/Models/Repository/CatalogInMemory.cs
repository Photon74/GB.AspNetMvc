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
            var oldProduct = Products.FirstOrDefault(p => p.Key == product.Id);
            var productEntity = new ProductEntity
            {
                Name = product.Name,
                Category = product.Category,
            };
            Products.TryUpdate(product.Id, productEntity, oldProduct.Value);
        }
    }
}
