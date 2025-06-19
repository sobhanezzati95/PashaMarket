using Domain.Entities.OrderAggregate;
using Framework.Domain;

namespace Domain.Contracts.Repositories.OrderAggregate;
public interface IOrderItemRepository 
    : IRepository<OrderItem>
{ }