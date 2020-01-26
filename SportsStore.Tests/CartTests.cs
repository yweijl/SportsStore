using System.Linq;
using SportsStore.Models;
using Xunit;

namespace SportsStore.Tests
{
    public class CartTests
    {
        [Fact]
        public void Can_Add_New_Lines()
        {
            var p1 = new Product {Id = 1, Name = "P1"};
            var p2 = new Product { Id = 2, Name = "P2" };

            var cart = new Cart();

            cart.AddItem(p1, 1);
            cart.AddItem(p2, 1);

            var results = cart.Lines.ToArray();

            Assert.Equal(2, results.Count());
            Assert.Equal(p1, results[0].Product);
            Assert.Equal(p2, results[1].Product);
        }

        [Fact]
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            var p1 = new Product { Id = 1, Name = "P1" };
            var p2 = new Product { Id = 2, Name = "P2" };

            var cart = new Cart();

            cart.AddItem(p1, 1);
            cart.AddItem(p2, 1);
            cart.AddItem(p1, 10);

            var results = cart.Lines.OrderBy(x => x.Product.Id).ToArray();

            Assert.Equal(2, results.Count());
            Assert.Equal(11, results[0].Quantity);
            Assert.Equal(1, results[1].Quantity);
        }

        [Fact]
        public void Can_Remove_Lines()
        {
            var p1 = new Product { Id = 1, Name = "P1" };
            var p2 = new Product { Id = 2, Name = "P2" };
            var p3 = new Product { Id = 3, Name = "P3" };

            var cart = new Cart();

            cart.AddItem(p1, 1);
            cart.AddItem(p2, 3);
            cart.AddItem(p3, 5);
            cart.AddItem(p2, 1);

            cart.RemoveLine(p2);

            Assert.Equal(0, cart.Lines.Count(x => x.Product == p2));
            Assert.Equal(2, cart.Lines.Count());
        }

        [Fact]
        public void Can_Calculate_Card_Total()
        {
            var p1 = new Product { Id = 1, Name = "P1" , Price = 100m };
            var p2 = new Product { Id = 2, Name = "P2", Price = 50m };

            var cart = new Cart();

            cart.AddItem(p1, 1);
            cart.AddItem(p2, 1);
            cart.AddItem(p1, 3);

            var result = cart.ComputeTotalValue();

            Assert.Equal(450m, result);
        }

        [Fact]
        public void Can_Clear_Contents()
        {
            var p1 = new Product { Id = 1, Name = "P1", Price = 100m };
            var p2 = new Product { Id = 2, Name = "P2", Price = 50m };

            var cart = new Cart();

            cart.AddItem(p1, 1);
            cart.AddItem(p2, 1);
            cart.AddItem(p1, 3);

            cart.Clear();

            Assert.Empty(cart.Lines);
        }
    }
}