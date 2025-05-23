using Application.Dtos.OrderAggregate;
using Application.Interfaces.OrderAggregate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nancy.Json;

namespace Presentation.Pages
{
    [Authorize]
    public class CheckoutModel : PageModel
    {
        public Cart Cart;
        public const string CookieName = "cart-items";
        private readonly ICartCalculatorService _cartCalculatorService;
        private readonly ICartService _cartService;
        public CheckoutModel(ICartCalculatorService cartCalculatorService, ICartService cartService)
        {
            _cartCalculatorService = cartCalculatorService;
            _cartService = cartService;
        }

        public async Task OnGet()
        {
            var serializer = new JavaScriptSerializer();
            var value = Request.Cookies[CookieName];
            var cartItems = serializer.Deserialize<List<CartItem>>(value) ?? new();
            foreach (var item in cartItems)
                item.CalculateTotalItemPrice();

            Cart = await _cartCalculatorService.ComputeCart(cartItems);
            //_cartService.Set(Cart);
        }


        public IActionResult OnPostPay(int paymentMethod)
        {
            //var cart = _cartService.Get();
            //check inventory and price , ...
            return RedirectToPage("/Checkout");
        }
    }
}
