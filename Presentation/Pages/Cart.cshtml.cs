using Application.Dtos.OrderAggregate;
using Application.Interfaces.ProductAggregate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nancy.Json;

namespace Presentation.Pages
{
    public class CartModel : PageModel
    {
        public List<CartItem> CartItems;
        public const string CookieName = "cart-items";
        private readonly IProductApplication _productApplication;
        public CartModel(IProductApplication productApplication)
        {
            _productApplication = productApplication;
            CartItems = new List<CartItem>();

        }

        public async Task OnGet()
        {
            var serializer = new JavaScriptSerializer();
            var value = Request.Cookies[CookieName];
            var cartItems = serializer.Deserialize<List<CartItem>>(value);
            //foreach (var item in cartItems)
            //    item.CalculateTotalItemPrice();

            CartItems = await _productApplication.CheckInventoryStatus(cartItems);
        }

        public async Task<IActionResult> OnGetGoToCheckout()
        {
            var serializer = new JavaScriptSerializer();
            var value = Request.Cookies[CookieName];
            var cartItems = serializer.Deserialize<List<CartItem>>(value);
            foreach (var item in cartItems)
            {
                item.TotalPrice = item.UnitPrice * item.Count;
            }

            CartItems = await _productApplication.CheckInventoryStatus(cartItems);

            return RedirectToPage(CartItems.Any(x => !x.IsInStock) ? "/Cart" : "/Checkout");
        }
    }
}
