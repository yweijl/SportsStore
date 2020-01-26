using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Routing;
using Moq;
using SportsStore.Components;
using SportsStore.Models;
using Xunit;

namespace SportsStore.Tests
{
    public class NavigationMenuViewComponentTests
    {
        [Fact]
        public void Can_Select_Categories()
        {
            var repo = new Mock<IProductRepository>();
            repo.Setup(x => x.Products).Returns(new[]
            {
                new Product {Id = 1, Name = "P1", Category = "Apples"},
                new Product {Id = 2, Name = "P2", Category = "Apples"},
                new Product {Id = 3, Name = "P3", Category = "Plums"},
                new Product {Id = 4, Name = "P4", Category = "Oranges"},
            }.AsQueryable());

            var target = new NavigationMenuViewComponent(repo.Object);
            var result =
                ((IEnumerable<string>) (target.Invoke() as ViewViewComponentResult)?.ViewData?.Model)?.ToArray();
            
            Assert.True(new []{"Apples", "Oranges", "Plums" }.SequenceEqual(result ?? throw new InvalidOperationException()));
        }

        [Fact]
        public void Indicates_Selected_Category()
        {
            const string categoryToSelect = "Apples";
            var repo = new Mock<IProductRepository>();
            repo.Setup(x => x.Products).Returns(new[]
            {
                new Product {Id = 1, Name = "P1", Category = "Apples"},
                new Product {Id = 2, Name = "P2", Category = "Apples"},
                new Product {Id = 3, Name = "P3", Category = "Plums"},
                new Product {Id = 4, Name = "P4", Category = "Oranges"},
            }.AsQueryable);

            var target = new NavigationMenuViewComponent(repo.Object)
            {
                ViewComponentContext = new ViewComponentContext
                {
                    ViewContext = new ViewContext {RouteData = new RouteData()}
                }
            };

            target.RouteData.Values["category"] = categoryToSelect;

            var result = (string) (target.Invoke() as ViewViewComponentResult)?.ViewData["SelectedCategory"];

            Assert.Equal(categoryToSelect, result);
        }
    }
}