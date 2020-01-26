using System;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using SportsStore.Views.ViewModels;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace SportsStore.Tests
{
    public class ProductControllerTests
    {
        private static Product[] ProductArray() =>  new []
        {
            new Product {Id = 1, Name = "P1"},
            new Product {Id = 2, Name = "P2"},
            new Product {Id = 3, Name = "P3"},
            new Product {Id = 4, Name = "P4"},
            new Product {Id = 5, Name = "P5"}
        };

        [Fact]
        public void Can_Paginate()
        {
            var products = ProductArray();
            var repo = new Mock<IProductRepository>();
            repo.Setup(x => x.Products).Returns(products.AsQueryable());

            using var controller = new ProductController(repo.Object) {PageSize = 3};
            var result = (controller.List(null, 2).ViewData.Model as ProductsListViewModel)?.Products.ToList();
            Assert.NotNull(result);

            Assert.True(result.Count == 2);
            Assert.Contains(products[3], result);
            Assert.Contains(products[4], result);
        }

        [Fact]
        public void Can_Send_Pagination_View_Model()
        {
            var repo = new Mock<IProductRepository>();
            repo.Setup(x => x.Products).Returns(ProductArray().AsQueryable);

            using var controller = new ProductController(repo.Object) { PageSize = 3 };
            var result = controller.List(null, 2).ViewData.Model as ProductsListViewModel;

            Assert.NotNull(result);

            Assert.Equal(2, result.PageInfo.CurrentPage);
            Assert.Equal(3, result.PageInfo.ItemsPerPage);
            Assert.Equal(5, result.PageInfo.TotalItems);
            Assert.Equal(2, result.PageInfo.TotalPages);
        }

        [Fact]
        public void Can_Filter_Products()
        {
            var repo = new Mock<IProductRepository>();
            repo.Setup(x => x.Products).Returns(new[]
            {
                new Product {Id = 1, Name = "P1", Category = "Cat1"},
                new Product {Id = 2, Name = "P2", Category = "Cat2"},
                new Product {Id = 3, Name = "P3", Category = "Cat1"},
                new Product {Id = 4, Name = "P4", Category = "Cat2"},
                new Product {Id = 5, Name = "P5", Category = "Cat3"},
            }.AsQueryable);

            using var controller = new ProductController(repo.Object);

            var result = (controller.List("Cat2", 1).ViewData.Model as ProductsListViewModel)?.Products.ToArray();

            Assert.Equal(2, result?.Length);
            Assert.True(result?[0].Name == "P2" && result[0].Category == "Cat2");
            Assert.True(result[1].Name == "P4" && result[0].Category == "Cat2");
        }

        [Theory]
        [InlineData("Cat1", 2)]
        [InlineData("Cat2", 2)]
        [InlineData("Cat3", 1)]
        [InlineData(null, 5)]
        public void Generate_Category_Specific_Product_Count(string category, int expectedCount)
        {
            var repo = new Mock<IProductRepository>();
            repo.Setup(x => x.Products).Returns(new[]
            {
                new Product {Id = 1, Name = "P1", Category = "Cat1"},
                new Product {Id = 2, Name = "P2", Category = "Cat2"},
                new Product {Id = 3, Name = "P3", Category = "Cat1"},
                new Product {Id = 4, Name = "P4", Category = "Cat2"},
                new Product {Id = 5, Name = "P5", Category = "Cat3"},
            }.AsQueryable);

            using var controller = new ProductController(repo.Object) {PageSize = 3};

            Func<ViewResult, ProductsListViewModel> GetModel = result => result?.ViewData?.Model as ProductsListViewModel;

            var res1 = GetModel(controller.List(category))?.PageInfo.TotalItems;

            Assert.Equal(expectedCount, res1);
        }
    }
}
