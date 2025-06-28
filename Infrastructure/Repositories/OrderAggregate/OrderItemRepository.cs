using Domain.Contracts.Repositories.OrderAggregate;
using Domain.Entities.OrderAggregate;
using Infrastructure.DbContexts;

namespace Infrastructure.Repositories.OrderAggregate;
public class OrderItemRepository(ApplicationDbContext context)
    : BaseRepository<OrderItem>(context), IOrderItemRepository
{ }