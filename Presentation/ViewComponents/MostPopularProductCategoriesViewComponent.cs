using Application.Interfaces.ProductAggregate;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.ViewComponents;
public class MostPopularProductCategoriesViewComponent(IProductCategoryApplication productCategoryApplication)
    : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(CancellationToken cancellationToken = default)
    {
        var productCategories = await productCategoryApplication.GetMostPopularProductCategoriesQuery(cancellationToken);
        return View(productCategories);
    }
}