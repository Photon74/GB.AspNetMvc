using GB.AspNetMvc.Models.DTO;
using System.Threading;

namespace GB.AspNetMvc.Models.Services.Interfaces
{
    public interface IProductService
    {
        List<ProductDto> GetProducts();
        Task AddProduct(ProductDto productDto, CancellationToken cancellationToken);
        void DeleteProduct(Guid id);
        ProductDto? GetProductById(Guid id);
        void EditProduct(ProductDto productDto);
    }
}
