using Domain.Contracts.Repositories.OrderAggregate;
using Domain.Entities.OrderAggregate;
using Infrastructure.DbContexts;

namespace Infrastructure.Repositories.OrderAggregate;
public class OrderRepository(ApplicationDbContext context)
    : BaseRepository<Order>(context), IOrderRepository
{ }