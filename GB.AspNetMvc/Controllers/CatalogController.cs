using GB.AspNetMvc.Models.DTO;
using GB.AspNetMvc.Models.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GB.AspNetMvc.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IProductService _productService;

        public CatalogController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        public IActionResult AddingProduct([FromForm] ProductDto productDto)
        {
            if (!ModelState.IsValid) return View(productDto);

            _productService.AddProduct(productDto);
            return RedirectToAction("ProductsList");
        }

        [HttpGet]
        public IActionResult AddingProduct()
        {
            return View();
        }

        public IActionResult ProductsList()
        {
            var products = _productService.GetProducts();
            return View(products);
        }

        [HttpGet]
        public IActionResult EditingProduct(Guid id)
        {
            var product = _productService.GetProductById(id);
            return product == null ? NotFound() : View(product);
        }

        [HttpPost]
        public IActionResult EditingProduct(ProductDto productDto)
        {
            if (!ModelState.IsValid) return View(productDto);

            _productService.EditProduct(productDto);
            return RedirectToAction("ProductsList");
        }

        public IActionResult ProductDeleting(Guid id)
        {
            _productService.DeleteProduct(id);

            return RedirectToAction("ProductsList");
        }
    }
}
