using GB.AspNetMvc.Models.DTO;

namespace GB.AspNetMvc.Models.Services.Interfaces
{
    public interface IProductService
    {
        List<ProductDto> GetProducts();
        void AddProduct(ProductDto productDto);
        void DeleteProduct(Guid id);


        //Product GetProductById(int id);
        //Product GetProductByName(string productName);
        //void UpdateProduct(Product product);
    }
}
