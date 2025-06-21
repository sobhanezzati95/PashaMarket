using Application.Dtos.OrderAggregate;
using Application.Interfaces.OrderAggregate;
using Domain.Contracts;
using Microsoft.Extensions.Logging;

namespace Application.Services.OrderAggregate;
public class CartCalculatorService(IUnitOfWork unitOfWork, ILogger<CartCalculatorService> _logger)
    : ICartCalculatorService
{
    public async Task<Cart> ComputeCart(List<CartItem> cartItems, CancellationToken cancellationToken = default)
    {
        var cart = new Cart();
        var queryDiscount = await unitOfWork.DiscountRepository.GetAllAsQueryable(cancellationToken);

        var discounts = queryDiscount.Where(x => x.StartDate < DateTime.Now
                                                 && x.EndDate > DateTime.Now)
                                     .Select(x => new { x.DiscountRate, x.ProductId })
                                     .ToList();

        foreach (var cartItem in cartItems)
        {
            var customerDiscount = discounts.FirstOrDefault(x => x.ProductId == cartItem.Id);
            if (customerDiscount != null)
                cartItem.DiscountRate = customerDiscount.DiscountRate;

            cartItem.Discount = cartItem.TotalPrice * cartItem.DiscountRate / 100;
            cartItem.ItemPayAmount = cartItem.TotalPrice - cartItem.Discount;
            cart.Add(cartItem);
        }
        return cart;
    }
}