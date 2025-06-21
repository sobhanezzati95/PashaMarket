using Application.Dtos.OrderAggregate;
using Application.Interfaces.OrderAggregate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nancy.Json;

namespace Presentation.Pages;
[Authorize]
public class CheckoutModel(ICartCalculatorService cartCalculatorService, ICartService cartService) : PageModel
{
    public Cart Cart;
    public const string CookieName = "cart-items";
    public async Task OnGet(CancellationToken cancellationToken = default)
    {
        var serializer = new JavaScriptSerializer();
        var value = Request.Cookies[CookieName];
        var cartItems = serializer.Deserialize<List<CartItem>>(value) ?? new();
        foreach (var item in cartItems)
            item.CalculateTotalItemPrice();

        Cart = await cartCalculatorService.ComputeCart(cartItems, cancellationToken);
    }
    public IActionResult OnPostPay(int paymentMethod, CancellationToken cancellationToken = default)
    {
        //var cart = _cartService.Get();
        //check inventory and price , ...
        return RedirectToPage("/Checkout", cancellationToken);
    }
}