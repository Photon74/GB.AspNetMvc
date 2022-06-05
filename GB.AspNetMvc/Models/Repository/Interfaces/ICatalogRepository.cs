namespace GB.AspNetMvc.Models.Repository.Interfaces
{
    public interface ICatalogRepository
    {
        IReadOnlyList<Product> GetAllProducts();
        void AddProduct(Product product);
        void DeleteProduct(Guid id);
    }
}
