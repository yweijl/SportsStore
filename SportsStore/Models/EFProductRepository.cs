using System.Linq;

namespace SportsStore.Models
{
    public class EfProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public EfProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<Product> Products => _context.Products;
        public void SaveProduct(Product product)
        {
            if (product.Id == 0)
            {
                _context.Add(product);
            }
            else
            {
                var dbEntry = _context.Products.FirstOrDefault(p => p.Id == product.Id);
                if (dbEntry == null) return;

                dbEntry.Name = product.Name;
                dbEntry.Description = product.Description;
                dbEntry.Price = product.Price;
                dbEntry.Category = product.Category;
            }

            _context.SaveChanges();
        }
    }
}