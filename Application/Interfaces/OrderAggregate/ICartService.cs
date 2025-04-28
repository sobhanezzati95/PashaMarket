using Application.Dtos.OrderAggregate;

namespace Application.Interfaces.OrderAggregate
{
    public interface ICartService
    {
        Cart Get();
        void Set(Cart cart);
    }
}