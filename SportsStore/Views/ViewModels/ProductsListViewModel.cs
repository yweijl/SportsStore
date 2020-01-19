using System.Collections.Generic;
using SportsStore.Models;

namespace SportsStore.Views.ViewModels
{
    public class ProductsListViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}