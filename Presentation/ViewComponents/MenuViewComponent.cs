using Application.Dtos;
using Application.Interfaces.ProductAggregate;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.ViewComponents
{
    public class MenuViewComponent : ViewComponent
    {
        private readonly IProductCategoryApplication _productCategoryApplication;
        public MenuViewComponent(IProductCategoryApplication productCategoryApplication)
        {
            _productCategoryApplication = productCategoryApplication;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = new MenuModel
            {
                ProductCategories = await _productCategoryApplication.GetCategoriesQuery()
            };
            return View(result);
        }
    }
}
