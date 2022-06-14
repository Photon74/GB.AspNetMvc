using GB.AspNetMvc.Models.DTO;
using GB.AspNetMvc.Models.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GB.AspNetMvc.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IProductService _productService;
        private Object obj = new();

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
            ViewBag.Products = _productService.GetProducts();
            return View();
        }

        public IActionResult ProductDeleting(Guid id)
        {
            _productService.DeleteProduct(id);

            return RedirectToAction("ProductsList");
        }

        //public List<ProductDto> Products { get; set; }
    }
}
