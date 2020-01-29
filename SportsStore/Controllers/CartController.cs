using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Infrastructure;
using SportsStore.Models;
using SportsStore.Views.ViewModels;

namespace SportsStore.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly Cart _cart;

        public CartController(IProductRepository productRepository, Cart cart)
        {
            _productRepository = productRepository;
            _cart = cart;
        }

        public ViewResult Index(string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = _cart,
                ReturnUrl = returnUrl
            });
        }

        public RedirectToActionResult AddToCart(int id, string returnUrl)
        {
            var product = _productRepository.Products.FirstOrDefault(x => x.Id == id);

            if (product != null)
            {
                _cart.AddItem(product, 1);
            } 

            return RedirectToAction("index", new {returnUrl});
        }

        public RedirectToActionResult RemoveFromCart(int productId, string returnUrl)
        {
            var product = _productRepository.Products.FirstOrDefault(x => x.Id == productId);

            if (product != null)
            {
                _cart.RemoveLine(product);
            }


            return RedirectToAction("index", new { returnUrl });
        }
    }
}