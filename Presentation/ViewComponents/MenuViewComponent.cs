using Application.Dtos;
using Application.Interfaces.ProductAggregate;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.ViewComponents;
public class MenuViewComponent(IProductCategoryApplication productCategoryApplication) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(CancellationToken cancellationToken = default)
    {
        var result = new MenuModel
        {
            ProductCategories = await productCategoryApplication.GetCategoriesQuery(cancellationToken)
        };
        return View(result);
    }
}