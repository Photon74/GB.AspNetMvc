namespace GB.AspNetMvc.Models.Repository.Interfaces
{
    public interface ICatalogRepository
    {
        IReadOnlyList<Product> GetAllProducts();
        void AddProduct(Product product);
        void DeleteProduct(Guid id);
        Product? GetProductById(Guid id);
        void UpdateProduct(Product product);
    }
}
