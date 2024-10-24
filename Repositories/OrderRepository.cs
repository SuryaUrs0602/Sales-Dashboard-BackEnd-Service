using Microsoft.EntityFrameworkCore;
using SalesDashBoardApplication.Models;
using SalesDashBoardApplication.Repositories.Contracts;

namespace SalesDashBoardApplication.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly SalesDashBoardDbContext _context;
        private readonly ILogger<OrderRepository> _logger;

        public OrderRepository(SalesDashBoardDbContext context, ILogger<OrderRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task CreateOrder(Order order)
        {
            _context.Add(order);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Added a new Order {order}");
        }

        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            _logger.LogInformation("Fetching all Order details");
            return await _context.Orders
                .Include(user => user.User)
                .Include(ord => ord.OrderItems)
                .ThenInclude(pro => pro.Product)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserId(int userId)
        {
            _logger.LogInformation("Fetching all ordres of a customer");
            return await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(user => user.User)
                .Include(ord => ord.OrderItems)
                .ThenInclude(pro => pro.Product)
                .ToListAsync();
        }
    }
}
