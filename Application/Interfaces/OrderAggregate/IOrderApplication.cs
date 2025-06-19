using Application.Dtos.OrderAggregate;

namespace Application.Interfaces.OrderAggregate;
public interface IOrderApplication
{
    Task<long> PlaceOrder(Cart cart, CancellationToken cancellationToken = default);
    Task<double> GetAmountBy(long id, CancellationToken cancellationToken = default);
    Task Cancel(long id, CancellationToken cancellationToken = default);
    Task<string> PaymentSucceeded(long orderId, long refId, CancellationToken cancellationToken = default);
    Task<List<OrderItemViewModel>> GetItems(long orderId, CancellationToken cancellationToken = default);
    Task<List<OrderViewModel>> Search(OrderSearchModel searchModel, CancellationToken cancellationToken = default);
}