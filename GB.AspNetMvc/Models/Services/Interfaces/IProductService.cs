using GB.AspNetMvc.Models.DTO;

namespace GB.AspNetMvc.Models.Services.Interfaces
{
    public interface IProductService
    {
        List<ProductDto> GetProducts();
        void AddProduct(ProductDto productDto);
        void DeleteProduct(Guid id);
        ProductDto? GetProductById(Guid id);
        void EditProduct(ProductDto productDto);
    }
}
