using GB.AspNetMvc.Models.Repository.Interfaces;

namespace GB.AspNetMvc.Models.Repository
{
    public class CatalogInMemory : ICatalogRepository
    {
        private List<Product> Products { get; set; }
        //private List Values { get; set; }


        public IReadOnlyList<Product> GetAllProducts()
        {
            return Products;
        }

        public void AddProduct(Product product)
        {
            if(Products == null) Products = new List<Product>();
            Products.Add(product);
        }

        public void DeleteProduct(Guid id)
        {
            var product = Products.FirstOrDefault(x => x.Id == id);
            if (product != null) Products.Remove(product);
        }
    }
}
