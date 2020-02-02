using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SportsStore.Models
{
    public class EfOrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public EfOrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<Order> Orders => 
            _context.Orders
                .Include(o => o.Lines)
                .ThenInclude(l => l.Product);


        public void SaveOrder(Order order)
        {
            _context.AttachRange(order.Lines.Select(l => l.Product));
            
            if (order.Id == 0)
                _context.Orders.Add(order);

            _context.SaveChanges();
        }
    }
}