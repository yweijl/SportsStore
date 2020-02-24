using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using Xunit;

namespace SportsStore.Tests
{
    public class AdminControllerTests
    {
        [Fact]
        public void Can_Delete_Products()
        {
            var products = Products.AsQueryable();
            var repo = new Mock<IProductRepository>();
            repo.Setup(x => x.Products).Returns(products);

            var target = new AdminController(repo.Object);
            target.Delete(products.Single(x => x.Id == 2).Id);
            repo.Verify(x => x.DeleteProduct(2));
        }

        [Fact]
        public void Index_Contains_All_Products()
        {
            var products = Products;
            var repo = new Mock<IProductRepository>();
            repo.Setup(x => x.Products).Returns(products.AsQueryable());

            using var target = new AdminController(repo.Object);
            var result = GetViewModel<IEnumerable<Product>>(target.Index())?.ToArray();

            Assert.Equal(3, result?.Length);
            Assert.Equal("P1", result?[0].Name);
            Assert.Equal("P2", result?[1].Name);
            Assert.Equal("P3", result?[2].Name);
        }

        [Fact]
        public void Can_Edit_Product()
        {
            var products = Products;
            var repo = new Mock<IProductRepository>();
            repo.Setup(x => x.Products).Returns(products.AsQueryable());
            using var target = new AdminController(repo.Object);

            var p1 = GetViewModel<Product>(target.Edit(1));
            var p2 = GetViewModel<Product>(target.Edit(2));
            var p3 = GetViewModel<Product>(target.Edit(3));

            Assert.Equal(1, p1.Id);
            Assert.Equal(2, p2.Id);
            Assert.Equal(3, p3.Id);
        }

        [Fact]
        public void Can_Edit_NonExistent_Product()
        {
            var products = Products;
            var repo = new Mock<IProductRepository>();
            repo.Setup(x => x.Products).Returns(products.AsQueryable());
            using var target = new AdminController(repo.Object);

            var result = GetViewModel<Product>(target.Edit(4));
            Assert.Null(result);
        }

        [Fact]
        public void Can_Save_Valid_Changes()
        {
            var repo = new Mock<IProductRepository>();
            var tempData = new Mock<ITempDataDictionary>();
            using var target = new AdminController(repo.Object){TempData = tempData.Object};

            var product = new Product {Name = "Test"};
            var result = target.Edit(product);
            
            repo.Verify(x => x.SaveProduct(product));
            Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", (result as RedirectToActionResult)?.ActionName);
        }

        [Fact]
        public void Cannot_Save_Invalid_Changes()
        {
            var repo = new Mock<IProductRepository>();
            using var target = new AdminController(repo.Object);

            var product = new Product { Name = "Test" };


            target.ModelState.AddModelError("error", "error");
            var result = target.Edit(product);


            repo.Verify(x => x.SaveProduct(It.IsAny<Product>()),Times.Never);
            Assert.IsType<ViewResult>(result);
        }

        private static IEnumerable<Product> Products => new[]
        {
            new Product {Id = 1, Name = "P1"},
            new Product {Id = 2, Name = "P2"},
            new Product {Id = 3, Name = "P3"}
        };

        private static T GetViewModel<T>(IActionResult result) where T : class
        {
            return (result as ViewResult)?.ViewData.Model as T;
        }
    }
}