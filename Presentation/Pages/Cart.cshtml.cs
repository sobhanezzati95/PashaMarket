using Application.Dtos.OrderAggregate;
using Application.Interfaces.ProductAggregate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nancy.Json;

namespace Presentation.Pages;
public class CartModel(IProductApplication productApplication) : PageModel
{
    public List<CartItem> CartItems = new List<CartItem>();
    public const string CookieName = "cart-items";
    public async Task OnGet(CancellationToken cancellationToken = default)
    {
        var serializer = new JavaScriptSerializer();
        var value = Request.Cookies[CookieName];
        var cartItems = serializer.Deserialize<List<CartItem>>(value) ?? new();
        CartItems = await productApplication.CheckInventoryStatus(cartItems, cancellationToken);
    }
    public async Task<IActionResult> OnGetGoToCheckout(CancellationToken cancellationToken = default)
    {
        var serializer = new JavaScriptSerializer();
        var value = Request.Cookies[CookieName];
        var cartItems = serializer.Deserialize<List<CartItem>>(value) ?? new();
        foreach (var item in cartItems)
            item.TotalPrice = item.UnitPrice * item.Count;

        CartItems = await productApplication.CheckInventoryStatus(cartItems, cancellationToken);
        return RedirectToPage(CartItems.Any(x => !x.IsInStock) ? "/Cart" : "/Checkout", cancellationToken);
    }
}