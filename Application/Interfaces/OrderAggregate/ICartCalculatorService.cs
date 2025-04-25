using Application.Dtos.OrderAggregate;

namespace Application.Interfaces.OrderAggregate
{
    public interface ICartCalculatorService
    {
       Task<Cart> ComputeCart(List<CartItem> cartItems);
    }
}