using Application.Interfaces.ProductAggregate;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.ViewComponents
{
    public class LatestProductsViewComponent : ViewComponent
    {
        private readonly IProductApplication _productApplication;

        public LatestProductsViewComponent(IProductApplication productApplication)
        {
            _productApplication = productApplication;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var productCategories = await _productApplication.GetLatestProductsQuery();
            return View(productCategories);
        }
    }
}
