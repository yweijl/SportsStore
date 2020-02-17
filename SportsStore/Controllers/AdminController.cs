using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;

namespace SportsStore.Controllers
{
    public class AdminController : Controller
    {
        private readonly IProductRepository _repository;

        public AdminController(IProductRepository repository)
        {
            _repository = repository;
        }

        public ViewResult Index() => View(_repository.Products);

        public ViewResult Edit(int productId) => 
            View(_repository.Products.FirstOrDefault(p => p.Id == productId));

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (!ModelState.IsValid) return View(product);
            
            _repository.SaveProduct(product);
            TempData["Message"] = $"{product.Name} has been saved";
            return RedirectToAction("Index");

        }
    }
}