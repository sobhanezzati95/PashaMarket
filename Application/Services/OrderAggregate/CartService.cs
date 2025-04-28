using Application.Dtos.OrderAggregate;
using Application.Interfaces.OrderAggregate;

namespace Application.Services.OrderAggregate
{
    public class CartService : ICartService
    {
        public Cart Cart { get; set; }

        public Cart Get()
        {
            return Cart;
        }

        public void Set(Cart cart)
        {
            Cart = cart;
        }
    }
}