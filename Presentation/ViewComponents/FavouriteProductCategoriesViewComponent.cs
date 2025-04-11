using Application.Interfaces.ProductAggregate;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.ViewComponents
{
    public class MostPopularProductCategoriesViewComponent : ViewComponent
    {
        private readonly IProductCategoryApplication _productCategoryApplication;

        public MostPopularProductCategoriesViewComponent(IProductCategoryApplication productCategoryApplication)
        {
            _productCategoryApplication = productCategoryApplication;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var productCategories = await _productCategoryApplication.GetMostPopularProductCategoriesQuery();
            return View(productCategories);
        }
    }
}
