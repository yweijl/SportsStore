using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using SportsStore.Views.ViewModels;
using System.Linq;
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
            var result = (controller.List(2).ViewData.Model as ProductsListViewModel)?.Products.ToList();
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
            var result = controller.List(2).ViewData.Model as ProductsListViewModel;

            Assert.NotNull(result);

            Assert.Equal(2, result.PageInfo.CurrentPage);
            Assert.Equal(3, result.PageInfo.ItemsPerPage);
            Assert.Equal(5, result.PageInfo.TotalItems);
            Assert.Equal(2, result.PageInfo.TotalPages);
        }
    }
}
