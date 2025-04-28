using Application.Dtos.OrderAggregate;

namespace Application.Interfaces.OrderAggregate
{
    public interface IOrderApplication
    {
        Task<long> PlaceOrder(Cart cart);
        Task<double> GetAmountBy(long id);
        Task Cancel(long id);
        Task<string> PaymentSucceeded(long orderId, long refId);
        Task<List<OrderItemViewModel>> GetItems(long orderId);
        Task<List<OrderViewModel>> Search(OrderSearchModel searchModel);
    }
}