using Domain.Contracts.Repositories.OrderAggregate;
using Domain.Entities.OrderAggregate;
using Infrastructure.DbContexts;

namespace Infrastructure.Repositories.OrderAggregate
{
    public class OrderItemRepository : BaseRepository<OrderItem>, IOrderItemRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderItemRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
