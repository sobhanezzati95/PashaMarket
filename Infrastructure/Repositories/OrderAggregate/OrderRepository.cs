using Domain.Contracts.Repositories.OrderAggregate;
using Domain.Entities.OrderAggregate;
using Infrastructure.DbContexts;

namespace Infrastructure.Repositories.OrderAggregate
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
