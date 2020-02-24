using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using Xunit;

namespace SportsStore.Tests
{
    public class OrderControllerTests
    {
        [Fact]
        public void Cannot_Checkout_Empty_Cart()
        {
            var repo = new Mock<IOrderRepository>();

            var cart = new Cart();
            var order = new Order();

            using var target = new OrderController(cart, repo.Object);

            var result = target.Checkout(order) as ViewResult;

            repo.Verify(x => x.SaveOrder(It.IsAny<Order>()), Times.Never);
            Assert.True(string.IsNullOrEmpty(result?.ViewName));
            Assert.False(result?.ViewData.ModelState.IsValid);
        }

        [Fact]
        public void Cannot_Checkout_Invalid_ShippingDetails()
        {
            var repo = new Mock<IOrderRepository>();

            var cart = new Cart();
            cart.AddItem(new Product(), 1);

            using var target = new OrderController(cart, repo.Object);
            target.ModelState.AddModelError("error", "error");
            var result = target.Checkout(new Order()) as ViewResult;

            repo.Verify(x => x.SaveOrder(It.IsAny<Order>()), Times.Never);
            Assert.True(string.IsNullOrEmpty(result?.ViewName));
            Assert.False(result?.ViewData.ModelState.IsValid);
        }

        [Fact]
        public void Cannot_Checkout_And_Submit_Order()
        {
            var repo = new Mock<IOrderRepository>();

            var cart = new Cart();
            cart.AddItem(new Product(), 1);

            using var target = new OrderController(cart, repo.Object);
            
            var result = target.Checkout(new Order()) as RedirectToActionResult;

            repo.Verify(x => x.SaveOrder(It.IsAny<Order>()), Times.Once);
            Assert.Equal("Completed", result?.ActionName);
        }
    }
}